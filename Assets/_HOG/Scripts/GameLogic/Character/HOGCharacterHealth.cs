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

        [SerializeField] HOGHealthBar healthBar;
        [SerializeField] HOGHealthBar recoveryRateBar;
        [SerializeField] HOGHealthBar resistanceBar;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;
        private int characterNumber;
        private HOGCharacterAnims characterAnims;

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
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAttackFinish,OnTakeDamage);
            AddListener(HOGEventNames.OnGameReset, ResetHealth);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttackFinish, OnTakeDamage);
            RemoveListener(HOGEventNames.OnGameReset, ResetHealth);
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
                    healthBar.SetHealth(currentHealth);
                    break;
                case barTypes.recoveryRate:
                    currentRecoveryRate -= amount;
                    if (currentRecoveryRate <= 0)
                    {
                        Die();
                    }
                    recoveryRateBar.SetHealth(currentRecoveryRate);
                    break;
                case barTypes.resistance:
                    currentResistance -= amount;
                    if (currentResistance <= 0)
                    {
                        Die();
                    }
                    resistanceBar.SetHealth(currentResistance);
                    break;
            }
            
        }

        public void ResetHealth(object obj)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
            currentRecoveryRate = maxRecoveryRate;
            recoveryRateBar.SetHealth(currentRecoveryRate);
            currentResistance = maxResistance;
            resistanceBar.SetHealth(currentResistance);
        }

        private void OnTakeDamage(object obj)
        {
            if(obj is Tuple<int, HOGCharacterActionBase> tupleData)
            {
                if(tupleData.Item1 != characterNumber)
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
                        case HOGCharacterState.CharacterStates.Move:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.resistance);
                            break;
                    }
                    
                    ShowEffectPerStrength(tupleData);
                }
            }
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
            HOGDebug.Log("Character died");
        }
    }
}

public enum barTypes
{
    health = 0,
    recoveryRate = 1,
    resistance = 2
}
