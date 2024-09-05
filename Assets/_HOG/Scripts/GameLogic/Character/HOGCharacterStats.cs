using HOG.Anims;
using HOG.Core;
using HOG.Screens;
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
        [SerializeField] int wits = 7;
        [SerializeField] int speed = 7;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;
        [SerializeField] float effectTriggeringPercentageFromAnimation = 0.9f;
        [SerializeField] float effectTriggeringAnimationEnd = 1.0f;
        private int characterNumber;
        private HOGCharacterAnims characterAnims;
        private Animator animator;
        private bool effectTriggered = false;
        Tuple<int, HOGCharacterActionBase> attackStrength;
        private HOGCharacterStats targetCharacter;
        private bool isDead = false;

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
            AddListener(HOGEventNames.OnAttack,OnTakeDamage);
            AddListener(HOGEventNames.OnGameReset, ResetStats);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttack, OnTakeDamage);
            RemoveListener(HOGEventNames.OnGameReset, ResetStats);
        }

        private void Update()
        {
            ShowEffectOnTime();
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

        public void TakeDamage(int amount, barTypes barObj)
        {
            //HOGDebug.Log($"TakeDamage, amount={amount}");
            switch (barObj)
            {
                case barTypes.integrity:// currently affecting just integrity
                    currentIntegrity -= amount;
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
            HOGDebug.Log("HandleDeath");
            while (true)
            {
                var currentStateInfo = targetCharacter.animator.GetCurrentAnimatorStateInfo(0);

                if ((currentStateInfo.IsName("AttackingBackhand") || currentStateInfo.IsName("AttackingDownward")) &&
                    currentStateInfo.normalizedTime >= effectTriggeringPercentageFromAnimation &&
                    currentStateInfo.normalizedTime < effectTriggeringAnimationEnd)
                {
                    //HOGDebug.Log("Correct timing reached, executing Die");
                    break; 
                }
                yield return null; 
            }

            Die();
        }

        public void ResetStats(object obj)
        {
            currentIntegrity = maxIntegrity;
        }
        private void OnTakeDamage(object obj)
        {
            if(obj is Tuple<int, HOGCharacterActionBase> tupleData)
            {
                effectTriggered = false;
                attackStrength = tupleData;
                targetCharacter = FindtargetCharacter(tupleData.Item1);
                if (tupleData.Item1 != characterNumber)
                {
                    //HOGDebug.Log($"ActionID={tupleData.Item2.ActionId}");
                    switch (tupleData.Item2.ActionId)
                    {
                        case HOGCharacterState.CharacterStates.Attack:
                            TakeDamage(tupleData.Item2.ActionStrength*3, barTypes.integrity);
                            break;
                        case HOGCharacterState.CharacterStates.Defense:
                            TakeDamage(tupleData.Item2.ActionStrength * 3, barTypes.integrity);
                            break;
                        case HOGCharacterState.CharacterStates.AttackBack:
                            TakeDamage(tupleData.Item2.ActionStrength * 3, barTypes.integrity);
                            break;
                    }
                    
                }
            }
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
            var characters = FindObjectsOfType<HOGCharacterStats>();
            foreach (var character in characters)
            {
                if (character.characterNumber == targetNumber)
                {
                    return character;
                }
            }
            return null;
        }

        private void ShowEffectPerStrength(Tuple<int, HOGCharacterActionBase> tupleData)
        {
            if (tupleData.Item2.ActionStrength >= megaHitTreshold)
            {
                //characterAnims.PlaySpecificEffect(0, transform, tupleData.Item2.ActionStrength);
                characterAnims.PlayRandomEffect(transform, tupleData.Item2.ActionStrength);
                InvokeEvent(HOGEventNames.OnGetHit, characterNumber);
            }
            else if (tupleData.Item2.ActionStrength >= avarageHitTreshold)
            {
                characterAnims.PlayRandomEffect(transform, tupleData.Item2.ActionStrength);
            }
            UpdateIntegritybar();
        }

        private void Die()
        {
            HOGDebug.Log("Die");
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
