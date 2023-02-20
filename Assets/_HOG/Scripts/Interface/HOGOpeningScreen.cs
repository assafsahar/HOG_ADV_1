using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Screens
{
    public class HOGOpeningScreen : HOGScreenBase
    {
        [SerializeField] Button startGameButton;

        private void Awake()
        {
            ScreenName = HOGScreenNames.OpeningScreen;
            if (startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
            }
        }

        private void OnStartGameClicked()
        {
            Manager.EventsManager.InvokeEvent(Core.HOGEventNames.OnGameStart, null);
            //gameObject.SetActive(false);

        }
    }

}
