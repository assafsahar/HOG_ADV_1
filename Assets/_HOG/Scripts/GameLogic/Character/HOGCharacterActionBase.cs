using HOG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOG.Character
{
    public class HOGCharacterActionBase
    {
        public HOGCharacterState.CharacterStates ActionId { get; private set; }
        public int ActionStrength { get; private set; }
        public HOGCharacterActionBase(HOGCharacterState.CharacterStates actionId, int actionStrength)
        {
            ActionId = actionId;
            ActionStrength = actionStrength;

        }

    }
}
