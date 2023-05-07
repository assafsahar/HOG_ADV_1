using HOG.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HOGDeckUI : HOGMonoBehaviour
    {
        public Dictionary<int, Button> Cards = new Dictionary<int, Button>();
        public Dictionary<int, Button> UpgradeButtons = new Dictionary<int, Button>();

        private void Start()
        {
            
        }

        public void ShowCard(int cardId, bool toShow, bool toEnable)
        {
            // Find the card with the specified ID
            Button card;
            if(!Cards.TryGetValue(cardId, out card))
            {
                HOGDebug.LogException("Card with ID " + cardId + " not found.");
            }
            card.interactable = toEnable;
            card.enabled = toShow;
            // If the card was not found, throw an exception
            if (card == null)
            {
                HOGDebug.LogException("Card with ID " + cardId + " not found.");
            }
        }

        public void ShowUpgradeButton(int buttonId,  bool toShow, bool toEnable)
        {
            Button button;
            if (!UpgradeButtons.TryGetValue(buttonId, out button))
            {
                HOGDebug.LogException("Button with ID " + buttonId + " not found.");
            }
            button.interactable = toEnable;
            button.enabled = toShow;
            // If the button was not found, throw an exception
            if (button == null)
            {
                HOGDebug.LogException("Button with ID " + buttonId + " not found.");
            }
        }

        public void ChangeCardLevelValue(int cardId, string newValue)
        {
            Button card;
            if (!Cards.TryGetValue(cardId, out card))
            {
                HOGDebug.LogException("Card with ID " + cardId + " not found.");
            }
            Transform cardTransform = card.transform;
            Transform levelTextTransform = cardTransform.Find("LevelText");
            if (levelTextTransform != null)
            {
                TextMeshProUGUI textField = levelTextTransform.GetComponentInChildren<TextMeshProUGUI>();
                if (textField == null)
                {
                    HOGDebug.LogException("There's no text field in card " + cardId);
                }
                textField.text = newValue;
            }
        }
    }
}