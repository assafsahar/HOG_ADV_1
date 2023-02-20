using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacterActions
    {
        
        List<HOGCharacterActionBase> characterAttacks = new();

        public void AddAction(HOGCharacterActionBase action)
        {
            characterAttacks.Add(action);
        }
        public HOGCharacterActionBase GetAction()
        {
            return characterAttacks[characterAttacks.Count - 1];
        }

        public void RemoveAction(int actionIndex)
        {
            if (characterAttacks.Count == 0)
            {
                return;
            }
        }

        public void ClearActions()
        {
            characterAttacks.Clear();
        }

        public void ChangeAction(int actionIndex, HOGCharacterActionBase action)
        {

        }
    }
}
