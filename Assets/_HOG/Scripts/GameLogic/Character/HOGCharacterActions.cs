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
            if (characterAttacks.Count == 0)
            {
                return null;
            }
            var tempAction = characterAttacks[characterAttacks.Count - 1];
            RemoveAction(characterAttacks.Count - 1);
            return tempAction;
        }

        public void RemoveAction(int actionIndex)
        {
            if (characterAttacks.Count == 0)
            {
                return;
            }
            characterAttacks.RemoveAt(actionIndex);
        }

        public void ClearActions()
        {
            characterAttacks.Clear();
        }

        public int GetAttacksCount()
        {
            return characterAttacks.Count;
        }

        public void ChangeAction(int actionIndex, HOGCharacterActionBase action)
        {

        }
    }
}
