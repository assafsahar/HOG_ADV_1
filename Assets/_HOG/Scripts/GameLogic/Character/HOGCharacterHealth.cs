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
        private int characterNumber;

        private void Awake()
        {
            characterNumber = GetComponent<HOGCharacter>().characterNumber;
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAttackFinish,OnTakeDamage);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttackFinish, OnTakeDamage);
        }

        private void OnTakeDamage(object obj)
        {
            if(obj is Tuple<int,int> tupleData)
            {
                if(tupleData.Item1 != characterNumber)
                {
                    TakeDamage(tupleData.Item2);
                }
                //Debug.Log(tupleData.Item1);
                //Debug.Log(tupleData.Item2);

            }
        }

        public HOGCharacterHealth()
        {
            maxHealth = 20;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                Die();
            }
            healthBar.SetHealth(currentHealth);
        }

        private void Die()
        {
            InvokeEvent(HOGEventNames.OnCharacterDied, characterNumber);
            Debug.Log("Character died");
        }
    }
}
