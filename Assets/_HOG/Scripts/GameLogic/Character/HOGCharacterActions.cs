using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterActions
    {
        [SerializeField] int requiredListLength = 3;
        List<HOGCharacterActionBase> characterAttacks = new List<HOGCharacterActionBase>();
        private int currentSlotNumber = 0;

        public void ResetList()
        {
            ClearActions();
            AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Move, 1));
            AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Attack, 1));
            AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Move, 1));
            currentSlotNumber = 0;
        }
        public void AddAction(HOGCharacterActionBase action)
        {
            characterAttacks.Add(action);
            if(characterAttacks.Count > requiredListLength)
            {
                characterAttacks.RemoveAt(characterAttacks.Count - 1);
            }
            
        }
        public HOGCharacterActionBase GetAction()
        {
            return characterAttacks[currentSlotNumber++];
        }

        public void ClearActions()
        {
            characterAttacks.Clear();
        }

        public bool CanContinue()
        {
            return currentSlotNumber < characterAttacks.Count;
        }

    }
}
