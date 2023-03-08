using HOG.Core;
using System.Collections;
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
        private HOGCharacterAttacksScriptable allAttackData;
        private List<HOGCharacterAction> attackData;

        public HOGCharacterActions(HOGAttacksUI AttacksUI, int CharacterType, HOGCharacterAttacksScriptable AttacksData)
        {
            allAttackData = AttacksData;
            if (AttacksUI != null)
            {
                attacksUI = AttacksUI;
                attacksUI.Init();
            }
            characterType = CharacterType;
            UpdateAttacksData();
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
            foreach (var element in allAttackData.AttacksConfig)
            {
                if (element.characterType == characterType)
                {
                    attackData = element.characterActions;
                    break;
                }
            }
            ResetList();
        }

        private void UpdateUI()
        {
            for(int i = 0; i<characterAttacks.Count; i++) 
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
