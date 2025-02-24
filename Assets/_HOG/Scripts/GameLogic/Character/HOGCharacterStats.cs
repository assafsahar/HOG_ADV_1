using HOG.Core;
using HOG.GameLogic;
using HOG.GameLogic.VFX;
using HOG.Screens;
using HOG.UI;
using System;
using System.Collections;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterStats: HOGMonoBehaviour
    {
        [SerializeField] private HOGCharacterUI characterUI;
        [SerializeField] int currentIntegrity = 100;
        [SerializeField] int maxIntegrity = 150;
        [SerializeField] int physics = 8;
        [SerializeField] int wits = 5;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;
        [SerializeField] float effectTriggeringPercentageFromAnimation = 0.9f;
        [SerializeField] float effectTriggeringAnimationEnd = 1.0f;
        [SerializeField] float timeBeforeDeath = 2f;
        [SerializeField] int selfHealAmount = 20;

        public int speed = 7;
        private int originalSpeed;

        private int characterNumber;
        private HOGCharacterAnims characterAnims;
        private Animator animator;
        private bool effectTriggered = false;
        Tuple<HOGCharacter, HOGCharacterActionBase> attackStrength;
        private HOGCharacterStats targetCharacter;
        private bool isDead = false;
        private float distance;

        private void Awake()
        {
            HOGCharacter hcComponent;
            var isHOGCharacter = TryGetComponent<HOGCharacter>(out hcComponent);
            characterNumber = hcComponent.characterNumber;
            HOGCharacterAnims hcAnimComponent;
            var isHOGCharacterAnim = TryGetComponent<HOGCharacterAnims>(out hcAnimComponent);
            characterAnims = hcAnimComponent;
            TryGetComponent(out animator);
        }
        private void OnEnable()
        {
            HOGDebug.Log($"OnEnable, characterNumber={characterNumber}");
            AddListener(HOGEventNames.OnAttack,OnTakeDamage);
            AddListener(HOGEventNames.OnAttack,PlayParticleEffect);
            AddListener(HOGEventNames.OnSelfHeal,OnTakeDamage);
            AddListener(HOGEventNames.OnGameReset, ResetStats);
        }

        
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttack, OnTakeDamage);
            RemoveListener(HOGEventNames.OnSelfHeal, OnTakeDamage);
            RemoveListener(HOGEventNames.OnGameReset, ResetStats);
            RemoveListener(HOGEventNames.OnAttack, PlayParticleEffect);
        }

        private void Update()
        {
            ShowEffectOnTime();
        }

        public int GetWits()
        {
            return wits;
        }
        public int GetPhysics()
        {
            return physics;
        }
        private void ShowEffectOnTime()
        {
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if ((currentStateInfo.IsName("AttackingBackhand") || currentStateInfo.IsName("AttackingDownward")) &&
                currentStateInfo.normalizedTime >= effectTriggeringPercentageFromAnimation && currentStateInfo.normalizedTime < effectTriggeringAnimationEnd)
            {
                if (!effectTriggered && attackStrength != null)
                {
                    targetCharacter.ShowEffectPerStrength(attackStrength);
                    effectTriggered = true;
                }
                else if (currentStateInfo.normalizedTime < effectTriggeringPercentageFromAnimation)
                {
                    effectTriggered = false;
                }
            }
        }

        private void PlayParticleEffect(object obj)
        {
            if (obj is Tuple<HOGCharacter, HOGCharacterActionBase> tupleData)
            {
                if (tupleData.Item1.characterNumber != characterNumber)
                {
                    InvokeEvent(HOGEventNames.OnEffectTriggered, obj);
                }
            }
        }

        private int CalculateDamage(Tuple<HOGCharacter, HOGCharacterActionBase> tupleData)
        {
            distance = HOGBattleManager.Instance.GetDistance();

            int damageMultiplier;

            if (distance <= 0) 
            {
                HOGDebug.Log($"CalculateDamage, using physics: {tupleData.Item1.GetStats().GetPhysics()}, characterNumber={characterNumber}");
                damageMultiplier = tupleData.Item1.GetStats().GetPhysics();
            }
            else 
            {
                HOGDebug.Log($"CalculateDamage, using wits: {tupleData.Item1.GetStats().GetWits()}, characterNumber={characterNumber}");
                damageMultiplier = tupleData.Item1.GetStats().GetWits();
            }
            HOGDebug.Log($"returning {tupleData.Item2.ActionStrength} * {damageMultiplier}");
            return tupleData.Item2.ActionStrength * damageMultiplier;
        }

        public void TakeDamage(int amount, barTypes barObj)
        {
            
            HOGDebug.Log($"TakeDamage, amount={amount}");
            switch (barObj)
            {
                case barTypes.integrity:// currently affecting just integrity
                    currentIntegrity -= amount;
                    if(amount < 0) // if healing
                    {
                        UpdateIntegritybar();
                        if (currentIntegrity > maxIntegrity)
                        {
                            currentIntegrity = maxIntegrity;
                        }
                    }
                    if (currentIntegrity <= 0 && !isDead)
                    {
                        StartCoroutine(HandleDeath());
                    }
                    break;
            }
            
        }

        private IEnumerator HandleDeath()
        {
            isDead = true;
            //HOGDebug.Log($"Character {characterNumber} HandleDeath started.");

            float timeout = timeBeforeDeath;
            float timer = 0f;

            while (timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            Die();
        }

        public void ResetStats(object obj)
        {
            currentIntegrity = maxIntegrity;
            isDead = false;
            UpdateIntegritybar();
            //HOGDebug.Log($"Character {characterNumber} stats reset. Integrity: {currentIntegrity}, isDead: {isDead}");
        }
        private void OnTakeDamage(object obj)
        {

            if (obj is Tuple<HOGCharacter, HOGCharacterActionBase> tupleData)
            {
                effectTriggered = false;
                attackStrength = tupleData;
                targetCharacter = FindtargetCharacter(tupleData.Item1.characterNumber);
                HOGDebug.Log($"OnTakeDamage characterNumber={characterNumber}, tupleData.Item1={tupleData.Item1}");
                if (tupleData.Item1.characterNumber != characterNumber)
                {

                    int damage = CalculateDamage(tupleData);
                    //HOGDebug.Log($"ActionID={tupleData.Item2.ActionId}");
                    switch (tupleData.Item2.ActionId)
                    {
                        case HOGCharacterState.CharacterStates.Attack:
                        case HOGCharacterState.CharacterStates.Defense:
                        case HOGCharacterState.CharacterStates.AttackBack:
                            TakeDamage(damage, barTypes.integrity);
                            break;
                        case HOGCharacterState.CharacterStates.AttackSpeed:
                            Invoke("ChangeSpeedToSix", 1f);
                            TakeDamage(damage, barTypes.integrity);
                            break;
                        
                    }

                }
                else
                {
                    HOGDebug.Log($"Character {characterNumber} is Healing itself.");
                    if(tupleData.Item2.ActionId == HOGCharacterState.CharacterStates.SelfHeal)
                        if (currentIntegrity < maxIntegrity)
                        {
                            TakeDamage(-selfHealAmount, barTypes.integrity);

                        }
                    }
            }
        }

        private void ChangeSpeedToSix()
        {
            ChangeSpeedTemp(6);
        }

        private void ChangeSpeedTemp(int newSpeed)
        {
            originalSpeed = speed;
            speed = newSpeed;
            Invoke("ReturnToOriginalSpeed", 2);
        }
        private void ReturnToOriginalSpeed()
        {
            speed = originalSpeed;
        }

        private void UpdateIntegritybar()
        {
            characterUI.UpdateIntegrityBar(currentIntegrity);
        }

        private int GetOtherCharacterNumber()
        {
            if(characterNumber == 1)
            {
                return 2;
            }
            return 1;
        }

        private HOGCharacterStats FindtargetCharacter(int attackingCharacterNumber)
        {
            int targetNumber = attackingCharacterNumber == 1 ? 2 : 1; 
            //HOGDebug.Log($"Looking for target character: {targetNumber}");

            var characters = FindObjectsOfType<HOGCharacterStats>();
            foreach (var character in characters)
            {
                //HOGDebug.Log($"Checking character: {character.characterNumber}");
                if (character.characterNumber == targetNumber) 
                {
                    //HOGDebug.Log($"Target character found: {character.characterNumber}");
                    return character;
                }
            }

            //HOGDebug.Log("Target character not found.");
            return null;
        }

        private void ShowEffectPerStrength(Tuple<HOGCharacter, HOGCharacterActionBase> otherCharacterData)
        {
            Debug.Log($"ShowEffectPerStrength, characterNumber {characterNumber}, otherCharacterData.Item1.characterNumber={otherCharacterData.Item1.characterNumber}");
            if (otherCharacterData.Item2.ActionStrength >= megaHitTreshold)
            {
                //characterAnims.PlaySpecificEffect(0, transform, otherCharacterData.Item2.ActionStrength);
                characterAnims.PlayRandomEffect(transform, otherCharacterData.Item2.ActionStrength);
                if (currentIntegrity > 0) // just if not died show hit animation
                {
                    InvokeEvent(HOGEventNames.OnGetHit, otherCharacterData);
                }
            }
            else if (otherCharacterData.Item2.ActionStrength >= avarageHitTreshold)
            {
                characterAnims.PlayRandomEffect(transform, otherCharacterData.Item2.ActionStrength);
            }
            UpdateIntegritybar();
        }

        private void Die()
        {
            //HOGDebug.Log($"Character {characterNumber} is dying.");
            InvokeEvent(HOGEventNames.OnCharacterDied, characterNumber);
            //HOGDebug.Log("Character died");
        }
    }
}

public enum barTypes
{
    integrity = 0,
    recoveryRate = 1,
    resistance = 2
}
