using HOG.Core;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.GameLogic
{
    public class HOGDeckManager : HOGMonoBehaviour
    {
        [SerializeField]
        List<ConfigurableCard> configurableCards = new List<ConfigurableCard>();
        [SerializeField] HOGDeckUI deckUI;

        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, DisableAllCards);
            AddListener(HOGEventNames.OnCharacterChange, DisableAllCards);
        }

        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, DisableAllCards);
            RemoveListener(HOGEventNames.OnCharacterChange, DisableAllCards);
        }
        private void Awake()
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    if (card.CardButton != null)
                    {
                        deckUI.Cards.Add(card.CardId, card.CardButton);
                    }
                    if(card.UpgradeButton != null)
                    {
                        deckUI.UpgradeButtons.Add(card.CardId, card.UpgradeButton);
                    }
                }
            }
            int changePowerLevel = 1;
            var changePowerData = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangePower);
            if (changePowerData != null)
            {
                changePowerLevel = changePowerData.CurrentLevel;
            }
            int changeCharacterLevel = 1;
            var changeCharacterData = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangeCharacter);
            if (changeCharacterData != null)
            {
                changeCharacterLevel = changeCharacterData.CurrentLevel;
            }

            UpdateCardLevel(2, changePowerLevel);
            UpdateCardLevel(1, changeCharacterLevel);
        }
        void Start()
        {
            ShowCard(2, true, true);
        }

        public void DisableAllCards(object obj=null)
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    ShowCard(card.CardId, true, false);
                }
            }
        }
        public void EnableAllCards()
        {
            foreach(var card in configurableCards)
            {
                if(card != null)
                {
                    ShowCard(card.CardId, true, true);
                }
            }
        }

        public void ShowCard(int cardId, bool toShow, bool toEnable)
        {
            if(configurableCards[0] == null)
            {
                HOGDebug.LogException("Card with ID " + cardId + " not found.");
            }
            deckUI.ShowCard(cardId, toShow, toEnable);
            configurableCards[0].CardVisible = toShow;
            configurableCards[0].CardEnabled = toEnable;
        }

        public void UpdateCardLevel(int cardId, int level)
        {
            deckUI.ChangeCardLevelValue(cardId, level.ToString());
        }
        public void ScoreChanged()
        {
            if (HOGGameLogicManager.Instance.UpgradeManager.CanMakeUpgrade(UpgradeablesTypeID.ChangePower))
            {
                deckUI.ShowUpgradeButton(2, true, true);
            }
            else
            {
                deckUI.ShowUpgradeButton(2, true, false);
            }
            if (HOGGameLogicManager.Instance.UpgradeManager.CanMakeUpgrade(UpgradeablesTypeID.ChangeCharacter))
            {
                deckUI.ShowUpgradeButton(1, true, true);
            }
            else
            {
                deckUI.ShowUpgradeButton(1, true, false);
            }
        }
    }
}

[Serializable]
public class ConfigurableCard
{
    public Button CardButton;
    public string CardName = "";
    public int CardId = 1;
    public bool CardEnabled = true;
    public bool CardVisible = true;
    public CardTypes CardType = CardTypes.Ability;
    public Button UpgradeButton;
}

public enum CardTypes
{
    Ability = 0,
    Character = 1
}
