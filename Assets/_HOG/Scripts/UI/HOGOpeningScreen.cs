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

        public override void Init()
        {
            ScreenName = HOGScreenNames.OpeningScreen;
        }
        private void Awake()
        {
            
            if (startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
            }
        }

        private void OnStartGameClicked()
        {
            Manager.EventsManager.InvokeEvent(Core.HOGEventNames.OnGameStart, null);
        }
    }

}
