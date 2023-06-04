using UnityEngine;
using UnityEngine.UI;

namespace HOG.Screens
{
    public class HOGOpeningScreen : HOGScreenBase
    {
        [SerializeField] Button startGameButton;

        private void Awake()
        {
            if (startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
            }
        }

        public override void Init()
        {
            ScreenName = HOGScreenNames.OpeningScreen;
        }

        private void OnStartGameClicked()
        {
            Manager.EventsManager.InvokeEvent(Core.HOGEventNames.OnGameStart, null);
        }
    }
}