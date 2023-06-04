using HOG.Core;

namespace HOG.Screens
{
    public class HOGScreenBase : HOGMonoBehaviour
    {
        private HOGScreenNames screenName;
        public HOGScreenNames ScreenName {
            get { return screenName; }
            set { screenName = value; }
        }

        // ready for override
        public virtual void Init()
        {

        }

        public virtual void EnableScreen()
        {
            gameObject.SetActive(true);
        }
        public virtual void DisableScreen()
        {
            gameObject.SetActive(false);
        }
        public bool IsActive()
        {
            return gameObject.activeInHierarchy;
        }
    }
    public enum HOGScreenNames
    {
        OpeningScreen = 0,
        GameScreen = 1,
        SummaryScreen = 2
    }
}
