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
        List<HOGCharacterActionBase> characterAttacks = new List<HOGCharacterActionBase>();
        private int currentSlotNumber = 0;
        private HOGAttacksUI attacksUI;

        public HOGCharacterActions(HOGAttacksUI AttacksUI)
        {
            if(AttacksUI != null)
            {
                attacksUI = AttacksUI;
                attacksUI.Init();
            }
            
        }
        public void ResetList()
        {
            ClearActions();
            AddAction(HOGCharacterState.CharacterStates.Attack, 1);
            AddAction(HOGCharacterState.CharacterStates.Attack, 1);
            AddAction(HOGCharacterState.CharacterStates.Attack, 1);
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
            return characterAttacks[currentSlotNumber++];
        }



        private void ClearActions()
        {
            characterAttacks.Clear();
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
                }
                
            }
        }
    }
}
