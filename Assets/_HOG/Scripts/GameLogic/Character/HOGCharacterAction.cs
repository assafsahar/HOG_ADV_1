using HOG.Core;
using System;

namespace HOG.Character
{
    [Serializable]
    public class HOGCharacterAction
    {
        public HOGCharacterState.CharacterStates ActionId;
        public int ActionStrength;
    }
}
