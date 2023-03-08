using HOG.Character;
using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    [Serializable]
    public class HOGCharacterActionsConfig
    {
        public int characterType;
        public List<HOGCharacterAction> characterActions;
     
    }

    [Serializable]
    public class HOGCharacterAction
    {
        public HOGCharacterState.CharacterStates ActionId;
        public int ActionStrength;
    }
}
