using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] int megaHitTreshold = 3;
        private int characterNumber;
        HOGCharacterAnims characterAnims;
        

        private void Awake()
        {
            characterNumber = GetComponent<HOGCharacter>().characterNumber;
            characterAnims = GetComponent<HOGCharacterAnims>();
            
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
                        characterAnims.PlayRandomEffect(transform);
                    }
                    
                }
                //Debug.Log(tupleData.Item1);
                //Debug.Log(tupleData.Item2);

            }
        }


        private void Die()
        {
            InvokeEvent(HOGEventNames.OnCharacterDied, characterNumber);
            Debug.Log("Character died");
        }
    }
}
