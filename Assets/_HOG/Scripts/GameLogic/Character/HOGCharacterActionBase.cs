using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOG.Character
{
    public class HOGCharacterActionBase
    {
        public HOGCharacterState ActionId { get; private set; }
        public HOGCharacterActionBase(HOGCharacterState.CharacterStates actionId)
        {
            ActionId = actionId;
        }

    }
}
