using HOG.Core;
using System;
using System.Collections;
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
                }
            }
        }
        void Start()
        {
            ShowCard(0, true, true);
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
                throw new ArgumentException("Card with ID " + cardId + " not found.");
            }
            deckUI.ShowCard(cardId, toShow, toEnable);
            configurableCards[0].CardVisible = toShow;
            configurableCards[0].CardEnabled = toEnable;
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
}

public enum CardTypes
{
    Ability = 0,
    Character = 1
}
