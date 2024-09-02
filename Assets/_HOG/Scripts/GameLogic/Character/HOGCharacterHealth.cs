using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterHealth: HOGMonoBehaviour
    {
        [SerializeField] float endurance = 1f;
        [SerializeField] float defenseRate = 1f;
        [SerializeField] int currentHealth = 1;
        [SerializeField] int maxHealth;
        [SerializeField] int currentRecoveryRate = 1;
        [SerializeField] int maxRecoveryRate;
        [SerializeField] int currentResistance = 1;
        [SerializeField] int maxResistance;

        //[SerializeField] HOGHealthBar healthBar;
        //[SerializeField] HOGHealthBar recoveryRateBar;
        //[SerializeField] HOGHealthBar resistanceBar;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;
        [SerializeField] float effectTriggeringPercentageFromAnimation = 0.9f;
        private int characterNumber;
        private HOGCharacterAnims characterAnims;
        private Animator animator;
        private bool effectTriggered = false;
        Tuple<int, HOGCharacterActionBase> attackStrength;
        private HOGCharacterHealth targetCharacterHealth;
        

        public HOGCharacterHealth()
        {
            maxHealth = 20;
            currentHealth = maxHealth;
            maxRecoveryRate = 20;
            currentRecoveryRate = maxRecoveryRate;
            maxResistance = 20;
            currentResistance = maxResistance;
        }

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
            AddListener(HOGEventNames.OnGameReset, ResetHealth);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttack, OnTakeDamage);
            RemoveListener(HOGEventNames.OnGameReset, ResetHealth);
        }

        private void Update()
        {
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if ((currentStateInfo.IsName("AttackingBackhand") || currentStateInfo.IsName("AttackingDownward")) &&
                currentStateInfo.normalizedTime >= effectTriggeringPercentageFromAnimation && currentStateInfo.normalizedTime < 1.0f)
            {
                //HOGDebug.Log("animation completed");
                if (!effectTriggered && attackStrength != null)
                {
                    targetCharacterHealth.ShowEffectPerStrength(attackStrength);
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
            HOGDebug.Log($"TakeDamage, amount={amount}");
            switch (barObj)
            {
                case barTypes.health:
                    currentHealth -= amount;
                    if (currentHealth <= 0)
                    {
                        Die();
                    }
                    //healthBar.SetHealth(currentHealth);
                    break;
                case barTypes.recoveryRate:
                    currentRecoveryRate -= amount;
                    if (currentRecoveryRate <= 0)
                    {
                        Die();
                    }
                    //recoveryRateBar.SetHealth(currentRecoveryRate);
                    break;
                case barTypes.resistance:
                    currentResistance -= amount;
                    if (currentResistance <= 0)
                    {
                        Die();
                    }
                    //resistanceBar.SetHealth(currentResistance);
                    break;
            }
            
        }

        public void ResetHealth(object obj)
        {
            currentHealth = maxHealth;
            //healthBar.SetHealth(currentHealth);
            currentRecoveryRate = maxRecoveryRate;
            //recoveryRateBar.SetHealth(currentRecoveryRate);
            currentResistance = maxResistance;
            //resistanceBar.SetHealth(currentResistance);
        }
        private void OnTakeDamage(object obj)
        {
            if(obj is Tuple<int, HOGCharacterActionBase> tupleData)
            {
                effectTriggered = false;
                attackStrength = tupleData;
                targetCharacterHealth = FindTargetCharacterHealth(tupleData.Item1);
                if (tupleData.Item1 != characterNumber)
                {
                    HOGDebug.Log($"ActionID={tupleData.Item2.ActionId}");
                    switch (tupleData.Item2.ActionId)
                    {
                        case HOGCharacterState.CharacterStates.Attack:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.health);
                            break;
                        case HOGCharacterState.CharacterStates.Defense:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.recoveryRate);
                            break;
                        case HOGCharacterState.CharacterStates.AttackBack:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.resistance);
                            break;
                    }
                    
                }
            }
        }

        private int GetOtherCharacterNumber()
        {
            if(characterNumber == 1)
            {
                return 2;
            }
            return 1;
        }

        private HOGCharacterHealth FindTargetCharacterHealth(int attackingCharacterNumber)
        {
            int targetNumber = attackingCharacterNumber == 1 ? 2 : 1;
            var characters = FindObjectsOfType<HOGCharacterHealth>();
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
                characterAnims.PlaySpecificEffect(0, transform, tupleData.Item2.ActionStrength);
                InvokeEvent(HOGEventNames.OnGetHit, characterNumber);
            }
            else if (tupleData.Item2.ActionStrength >= avarageHitTreshold)
            {
                characterAnims.PlayRandomEffect(transform, tupleData.Item2.ActionStrength);
            }
        }

        private void Die()
        {
            InvokeEvent(HOGEventNames.OnCharacterDied, characterNumber);
            //HOGDebug.Log("Character died");
        }
    }
}

public enum barTypes
{
    health = 0,
    recoveryRate = 1,
    resistance = 2
}
