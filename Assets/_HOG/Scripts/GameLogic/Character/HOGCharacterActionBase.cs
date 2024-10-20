using HOG.Core;

namespace HOG.Character
{
    public class HOGCharacterActionBase
    {
        private HOGCharacterState.CharacterStates actionId;
        private int actionStrength;
        private bool isTemp;

        public HOGCharacterState.CharacterStates ActionId {
            get { return actionId; } 
            private set { actionId = value; }
        }
        public int ActionStrength {
            get { return actionStrength; }
            private set { actionStrength = value; }
        }
        public bool IsTemp {
            get { return isTemp; }
            set { isTemp = value; }
        }

        public HOGCharacterActionBase(HOGCharacterState.CharacterStates actionId, int actionStrength, bool isTemp=false)
        {
            ActionId = actionId;
            ActionStrength = actionStrength;
            IsTemp = isTemp;
            //HOGDebug.Log($"HOGCharacterActionBase created with ActionId: {actionId}, ActionStrength: {actionStrength}, IsTemp: {IsTemp}");
        }
    }
}
