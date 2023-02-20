using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGScreenManager:HOGMonoBehaviour
    {
        [SerializeField] List<HOGScreenBase> Screens = new();

        public HOGScreenManager()
        {

            
        }

        private void Start()
        {
            EnableScreen(HOGScreenNames.OpeningScreen);
        }

        private void OnEnable()
        {
            AddListener(HOGEventNames.OnGameStart, StartGame);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnGameStart, StartGame);
        }

        private void StartGame(object obj)
        {
            EnableScreen(HOGScreenNames.GameScreen);
        }

        
        private void DisableAll()
        {
            foreach (var screen in Screens)
            {
                if (screen != null)
                {
                    screen.DisableScreen();
                }
            }
        }
        private void EnableScreen(HOGScreenNames screenName)
        {
            DisableAll();
            foreach (var screen in Screens)
            {
                if(screen != null && screen.ScreenName == screenName)
                {
                    screen.EnableScreen();
                }
            }
        }
    }

    
}

