using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.GameLogic
{
    public class HOGDeckManager : MonoBehaviour
    {
        [SerializeField]
        List<ConfigurableCard> configurableCards = new List<ConfigurableCard>();

        [SerializeField] HOGDeckUI deckUI;
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

        private void ShowCard(int cardId, bool toShow, bool toEnable)
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
}

public enum CardTypes
{
    Ability,
    Character
}
