using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterHealth: HOGMonoBehaviour
    {
        [SerializeField] float recoveryRate = 1f;
        [SerializeField] float resistance = 1f;
        [SerializeField] float endurance = 1f;
        [SerializeField] float defenseRate = 1f;
        [SerializeField] int currentHealth;
        [SerializeField] int maxHealth;
        [SerializeField] HOGHealthBar healthBar;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;

        private int characterNumber;
        private HOGCharacterAnims characterAnims;
        
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

        public HOGCharacterHealth()
        {
            maxHealth = 20;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }

            healthBar.SetHealth(currentHealth);
        }

        public void ResetHealth(object obj)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
        private void OnTakeDamage(object obj)
        {
            if(obj is Tuple<int,int> tupleData)
            {
                if(tupleData.Item1 != characterNumber)
                {
                    TakeDamage(tupleData.Item2);
                    if(tupleData.Item2 >= megaHitTreshold)
                    {
                        characterAnims.PlaySpecificEffect(0, transform, tupleData.Item2);
                        InvokeEvent(HOGEventNames.OnGetHit, characterNumber);
                    }
                    else if(tupleData.Item2 >= avarageHitTreshold)
                    {
                        characterAnims.PlayRandomEffect(transform, tupleData.Item2);
                    }
                }
            }
        }

        private void Die()
        {
            InvokeEvent(HOGEventNames.OnCharacterDied, characterNumber);
            HOGDebug.Log("Character died");
        }
    }
}
