using HOG.Core;
using HOG.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterActions
    {
        [SerializeField] int requiredListLength = 3;
        private List<HOGCharacterActionBase> characterAttacks = new List<HOGCharacterActionBase>();
        private int currentSlotNumber = 0;
        private int characterType;
        private HOGUpgradableAttacksConfig allAttackData;
        private List<HOGCharacterAction> attackData;

        public HOGCharacterActions(int CharacterType, HOGUpgradableAttacksConfig AttacksData)
        {
            //HOGDebug.Log("AttacksData=" + AttacksData);
            allAttackData = AttacksData;
            UpdateAttacksData();
        }
        public void ResetList()
        {
            ClearActions();
            AddAction(attackData[0].ActionId, attackData[0].ActionStrength);
            currentSlotNumber = 0;
        }
        public void AddAction(HOGCharacterState.CharacterStates actionId, int actionStrength)
        {
            characterAttacks.Insert(0, new HOGCharacterActionBase(actionId, actionStrength));
            if(characterAttacks.Count > requiredListLength)
            {
                characterAttacks.RemoveAt(characterAttacks.Count-1);
            }
        }

        public void ReplaceAction(HOGCharacterState.CharacterStates actionId, int actionStrength, bool isTemp, int slotNumber=-1)
        {
            if (isTemp)
            {
                ReplaceActionTemp(actionId, actionStrength, isTemp, slotNumber);
            }
            else
            {
                ReplaceActionPermanent(slotNumber);
            }

        }

        private void ReplaceActionPermanent(int slotNumber)
        {
            if (slotNumber == -1)
            {
                for (int i = 0; i < characterAttacks.Count; i++)
                {
                    characterAttacks[i] = new HOGCharacterActionBase(attackData[i].ActionId, attackData[i].ActionStrength);
                }
            }
            else
            {
                characterAttacks[slotNumber] = new HOGCharacterActionBase(attackData[slotNumber].ActionId, attackData[slotNumber].ActionStrength);
            }
        }

        private void ReplaceActionTemp(HOGCharacterState.CharacterStates actionId, int actionStrength, bool isTemp, int slotNumber)
        {
            if (slotNumber == -1)
            {
                for (int i = 0; i < characterAttacks.Count; i++)
                {
                    characterAttacks[i] = new HOGCharacterActionBase(actionId, actionStrength, isTemp);
                }
            }
            else
            {
                characterAttacks[slotNumber] = new HOGCharacterActionBase(actionId, actionStrength, isTemp);
            }
        }

        public bool CanContinue()
        {
            if(currentSlotNumber < characterAttacks.Count)
            {
                return true;
            }
            currentSlotNumber = 0;
            return false;
        }
        public HOGCharacterActionBase GetAction()
        {            
            return characterAttacks[currentSlotNumber++];
        }

        public void RemoveTempAction()
        {
            //HOGDebug.Log($"removing number {currentSlotNumber - 1}");
            if(currentSlotNumber <= 0)
            {
                return;
            }
            if (characterAttacks[currentSlotNumber-1].IsTemp)
            {
                ReplaceAction(0, 0, false, currentSlotNumber-1);
            }
        }

        public void UpdateCharacterType(int type)
        {
            characterType = type;
            UpdateAttacksData();
        }

        private void ClearActions()
        {
            characterAttacks.Clear();
        }

        private void UpdateAttacksData()
        {
            foreach (var element in allAttackData.UpgradableAttacks)
            {
                if (element.CharacterType == characterType)
                {
                    
                    if(characterType == 3) // enemy random
                    {
                        attackData = CreateRandomCharacterActions();
                    }
                    else // player config
                    {
                        attackData = element.CharacterActions;
                    }
                }
            }
            ResetList();
        }

        private List<HOGCharacterAction> CreateRandomCharacterActions()
        {
            var characterActions = new List<HOGCharacterAction>();
            HOGCharacterAction action;
            while(characterActions.Count < 3) {
                action = new HOGCharacterAction();
                action.ActionId = GetRandomActionId();
                action.ActionStrength = GetRandomActionStrength();
                characterActions.Add(action);
            }

            return characterActions;
        }

        private int GetRandomActionStrength()
        {
            return Random.Range(1, 5);
        }

        private HOGCharacterState.CharacterStates GetRandomActionId()
        {
            int randomIndex = Random.Range(1, 4);
            switch (randomIndex)
            {
                case 1:
                    return HOGCharacterState.CharacterStates.Defense;
                case 2:
                    return HOGCharacterState.CharacterStates.Attack;
                case 3:
                    return HOGCharacterState.CharacterStates.AttackBack;
                default:
                    return HOGCharacterState.CharacterStates.Defense;
            }
        }

    }
}
