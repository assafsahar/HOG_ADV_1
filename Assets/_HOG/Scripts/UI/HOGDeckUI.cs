using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HOGDeckUI : HOGMonoBehaviour
    {
        public Dictionary<int, Button> Cards = new Dictionary<int, Button>();

        public void ShowCard(int cardId, bool toShow, bool toEnable)
        {
            // Find the card with the specified ID
            Button card;
            if(!Cards.TryGetValue(cardId, out card))
            {
                throw new ArgumentException("Card with ID " + cardId + " not found.");
            }
            card.interactable = toEnable;
            card.enabled = toShow;
            // If the card was not found, throw an exception
            if (card == null)
            {
                throw new ArgumentException("Card with ID " + cardId + " not found.");
            }

        }

    }

}