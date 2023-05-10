using HOG.Core;
using System;
using System.Collections.Generic;

namespace HOG.Character
{
    [Serializable]
    public class HOGCharacterAction
    {
        public HOGCharacterState.CharacterStates ActionId;
        public int ActionStrength;
    }
}
