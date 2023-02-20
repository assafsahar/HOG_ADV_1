using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : MonoBehaviour
    {
        HOGCharacterActions Actions;
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;
        HOGCharacterHealth health;
   
        public void PlayActionSequence()
        {

        }

        public void CreateActionSequence()
        {
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Attack));
        }
    }
}
