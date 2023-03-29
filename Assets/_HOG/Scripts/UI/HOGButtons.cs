using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HOGButtons : HOGMonoBehaviour
    {
        [SerializeField]
        List<ConfigurableButton> configurableButtons = new List<ConfigurableButton>();

        private void Start()
        {
            ShowButton(0, true, false);
        }
        public void ShowButton(int buttonId, bool toShow, bool toEnable)
        {
            // Find the button with the specified ID
            ConfigurableButton button = null;
            foreach (ConfigurableButton btn in configurableButtons)
            {
                if (btn.BtnId == buttonId)
                {
                    button = btn;
                    break;
                }
            }

            // If the button was not found, throw an exception
            if (button == null)
            {
                throw new ArgumentException("Button with ID " + buttonId + " not found.");
            }

            // Set the visibility and enabled state of the button
            button.BtnVisible = toShow;
            button.BtnEnabled = toEnable;
            button.Button.interactable = toEnable;
            button.Button.gameObject.SetActive(toShow);
        }

    }

    [Serializable]
    public class ConfigurableButton
    {
        public Button Button;
        public string BtnName = "";
        public int BtnId = 1;
        public bool BtnEnabled = true;
        public bool BtnVisible = true;
    }
}