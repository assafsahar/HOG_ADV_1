using HOG.Core;
using HOG.GameLogic;
using System;

namespace HOG.Components
{
    public class HOGCardClicker : HOGMonoBehaviour
    {
    // the following methods are called from the UI buttons (editor)
    public void ChangeAttack()
        {
            var amount = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangePower).CurrentLevel;
            InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
        }

        public void changeCharacter()
        {
            var num = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangeCharacter).CurrentLevel;
            var character = HOGGameLogicManager.Instance.UpgradeManager.GetCharacterByIDAndLevel(UpgradeablesTypeID.ChangeCharacter, num-1);
            InvokeEvent(HOGEventNames.OnCharacterChange, character);
        }

        public void OnUpgradePress(int upgradeId)
        {
            UpgradeablesTypeID upgradable = (UpgradeablesTypeID)upgradeId;
            HOGGameLogicManager.Instance.UpgradeManager.UpgradeItemByID(upgradable);
        }
    }
}
