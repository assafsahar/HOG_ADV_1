using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Menus
{
    public class HOGOpenningScreen : HOGScreenBase
    {
        [SerializeField] Button startGameButton;

        private void Awake()
        {
            if(startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
            }
        }

        private void OnStartGameClicked()
        {
            Manager.EventsManager.InvokeEvent(Core.HOGEventNames.OnGameStart, null);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
