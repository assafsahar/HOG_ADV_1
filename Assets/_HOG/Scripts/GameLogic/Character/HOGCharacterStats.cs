using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterStats: HOGMonoBehaviour
    {
        [SerializeField] float endurance = 1f;
        [SerializeField] float defenseRate = 1f;
        [SerializeField] int currentIntegrity = 1;
        [SerializeField] int maxIntegrity;
        [SerializeField] int currentRecoveryRate = 1;
        [SerializeField] int maxRecoveryRate;
        [SerializeField] int currentResistance = 1;
        [SerializeField] int maxResistance;
        [SerializeField] int avarageHitTreshold = 2;
        [SerializeField] int megaHitTreshold = 4;
        [SerializeField] float effectTriggeringPercentageFromAnimation = 0.9f;
        [SerializeField] HOGIntegrityBar integrityBar;
        private int characterNumber;
        private HOGCharacterAnims characterAnims;
        private Animator animator;
        private bool effectTriggered = false;
        Tuple<int, HOGCharacterActionBase> attackStrength;
        private HOGCharacterStats targetCharacter;
        

        public HOGCharacterStats()
        {
            maxIntegrity = 20;
            currentIntegrity = maxIntegrity;
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
            AddListener(HOGEventNames.OnGameReset, ResetStats);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttack, OnTakeDamage);
            RemoveListener(HOGEventNames.OnGameReset, ResetStats);
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
            HOGDebug.Log($"TakeDamage, amount={amount}");
            switch (barObj)
            {
                case barTypes.integrity:
                    currentIntegrity -= amount;
                    if (currentIntegrity <= 0)
                    {
                        Die();
                    }
                    break;
                case barTypes.recoveryRate:
                    currentRecoveryRate -= amount;
                    if (currentRecoveryRate <= 0)
                    {
                        Die();
                    }
                    break;
                case barTypes.resistance:
                    currentResistance -= amount;
                    if (currentResistance <= 0)
                    {
                        Die();
                    }
                    break;
            }
            
        }

        public void ResetStats(object obj)
        {
            currentIntegrity = maxIntegrity;
            currentRecoveryRate = maxRecoveryRate;
            currentResistance = maxResistance;
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
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.integrity);
                            break;
                        case HOGCharacterState.CharacterStates.Defense:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.integrity);
                            break;
                        case HOGCharacterState.CharacterStates.AttackBack:
                            TakeDamage(tupleData.Item2.ActionStrength, barTypes.integrity);
                            break;
                    }
                    
                }
            }
        }

        private void UpdateIntegritybar()
        {
            integrityBar.SetValue(currentIntegrity);
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
