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
            healthBar.SetHealth(currentHealth / maxHealth);
        }

        private void Die()
        {
            Debug.Log("Character died");
        }
    }
}
