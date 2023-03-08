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
        public bool IsTemp { get; set; }
        public HOGCharacterActionBase(HOGCharacterState.CharacterStates actionId, int actionStrength, bool isTemp=false)
        {
            ActionId = actionId;
            ActionStrength = actionStrength;
            IsTemp = isTemp;
        }

    }
}
