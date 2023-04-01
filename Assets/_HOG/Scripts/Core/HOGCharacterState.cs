using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Core
{
    
    public class HOGCharacterState
    {
        public enum CharacterStates
        {
            Idle,
            Move,
            Attack,
            Defense,
            Die,
            Hurt,
            Win
        }
    }
}
