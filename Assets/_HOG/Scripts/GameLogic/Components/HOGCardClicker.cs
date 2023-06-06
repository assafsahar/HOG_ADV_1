using HOG.Core;
using HOG.GameLogic;
using System;

namespace HOG.Components
{
    public class HOGCardClicker : HOGMonoBehaviour
    {
        private HOGDeckManager deckManager;

        // the following methods are called from the UI buttons (editor)

        private void Awake()
        {
            deckManager = FindObjectOfType<HOGDeckManager>();
            if(deckManager == null)
            {
                HOGDebug.LogException("HOGDeckManager not found. Make sure it exists in the scene.");
            }
        }

        public void OnChangeAttackButtonClicked()
        {
            int cardCost = deckManager.configurableCards[0].CardCost; // Set the desired card cost
            int cardValue = deckManager.configurableCards[0].CardValue;
            ChangeAttack(cardValue, cardCost);
        }

        public void OnCharacterNumberButtonClicked()
        {
            int cardCost = deckManager.configurableCards[1].CardCost; // Set the desired card cost
            int cardValue = deckManager.configurableCards[1].CardValue;
            CharacterNumber(cardValue, cardCost);
        }

        public void ChangeAttack(int cardValue, int cardCost)
        {

            if (deckManager != null && deckManager.CurrentEnergy >= cardCost)
            {
                deckManager.UpdateEnergy(-cardCost);
                var amount = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangePower).CurrentLevel;
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
            }
        }

        public void CharacterNumber(int cardValue, int cardCost)
        {
            if (deckManager != null && deckManager.CurrentEnergy >= cardCost)
            {
                deckManager.UpdateEnergy(-cardCost);
                var num = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangeCharacter).CurrentLevel;
                var character = HOGGameLogicManager.Instance.UpgradeManager.GetCharacterByIDAndLevel(UpgradeablesTypeID.ChangeCharacter, num - 1);
                InvokeEvent(HOGEventNames.OnCharacterChange, character);
            }
        }

        public void OnUpgradePress(int upgradeId)
        {
            UpgradeablesTypeID upgradable = (UpgradeablesTypeID)upgradeId;
            HOGGameLogicManager.Instance.UpgradeManager.UpgradeItemByID(upgradable);
        }
    }
}
