using HOG.Core;
using HOG.GameLogic;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterActions
    {
        [SerializeField] int requiredListLength = 3;

        private List<HOGCharacterActionBase> characterAttacks = new List<HOGCharacterActionBase>();
        private int currentSlotNumber = 0;
        private HOGAttacksUI attacksUI;
        private int characterType;
        private HOGUpgradableAttacksConfig allAttackData;
        private List<HOGCharacterAction> attackData;

        public HOGCharacterActions(HOGAttacksUI AttacksUI, int CharacterType, HOGUpgradableAttacksConfig AttacksData)
        {
            HOGDebug.Log("AttacksData=" + AttacksData);
            allAttackData = AttacksData;
            if (AttacksUI != null)
            {
                attacksUI = AttacksUI;
                attacksUI.Init(delegate {
                    characterType = CharacterType;
                    UpdateAttacksData();
                });
            }
            
        }
        public void ResetList()
        {
            ClearActions();
            AddAction(attackData[2].ActionId, attackData[2].ActionStrength);
            AddAction(attackData[1].ActionId, attackData[1].ActionStrength);
            AddAction(attackData[0].ActionId, attackData[0].ActionStrength);
            currentSlotNumber = 0;
            UpdateUI();
        }
        public void AddAction(HOGCharacterState.CharacterStates actionId, int actionStrength)
        {
            characterAttacks.Insert(0, new HOGCharacterActionBase(actionId, actionStrength));
            if(characterAttacks.Count > requiredListLength)
            {
                characterAttacks.RemoveAt(characterAttacks.Count-1);
            }
            UpdateUI();
        }

        public void ReplaceAction(HOGCharacterState.CharacterStates actionId, int actionStrength, bool isTemp, int slotNumber=-1)
        {
            if (isTemp)
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
            else
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
            
            UpdateUI();
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
            if(attacksUI != null)
            {
                attacksUI.ShowActiveSlot(currentSlotNumber + 1);
            }
            
            return characterAttacks[currentSlotNumber++];
        }

        public void RemoveTempAction()
        {
            HOGDebug.Log($"removing number {currentSlotNumber - 1}");
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
            UpdateUI();
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
                    attackData = element.CharacterActions;
                    break;
                }
            }
            ResetList();
        }

        private void UpdateUI(int slotNumber = -1)
        {
            var startNum = 0;
            var endNum = characterAttacks.Count;
            if(slotNumber != -1)
            {
                startNum = slotNumber - 1;
                endNum = slotNumber;
            }
            for (int i = startNum; i < endNum; i++) 
            {
                var firstChar = characterAttacks[i].ActionId.ToString()[0];
                var strength = characterAttacks[i].ActionStrength.ToString();
                if(attacksUI != null)
                {
                    attacksUI.UpdateAttackText(i + 1, firstChar, strength);
                    attacksUI.UpdateCharacterTypeText(characterType+1);
                }
            }
        }
    }
}
