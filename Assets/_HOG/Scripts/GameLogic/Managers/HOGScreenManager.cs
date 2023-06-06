using HOG.Core;
using HOG.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGScreenManager:HOGMonoBehaviour
    {
        [SerializeField] List<HOGScreenBase> Screens;

        private void OnEnable()
        {
            //AddListener(HOGEventNames.OnGameStart, StartGame);
            AddListener(HOGEventNames.OnPreFightReady, StartGame);
        }


        private void OnDisable()
        {
            //RemoveListener(HOGEventNames.OnGameStart, StartGame);
            RemoveListener(HOGEventNames.OnPreFightReady, StartGame);
        }

        private void Awake()
        {
            foreach (var screen in Screens)
            {
                screen.Init();
            }
        }

        private void Start()
        {
            StartCoroutine(EnableScreen(HOGScreenNames.OpeningScreen, 0.1f));
        }
        public IEnumerator EnableScreen(HOGScreenNames screenName, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            DisableAll();
            foreach (var screen in Screens)
            {
                if (screen != null && screen.ScreenName == screenName && !screen.IsActive())
                {
                    screen.EnableScreen();
                }
            }
        }

        private void StartGame(object obj)
        {

            StartCoroutine(EnableScreen(HOGScreenNames.GameScreen));
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
    }
}

