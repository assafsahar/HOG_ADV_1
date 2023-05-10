using HOG.Core;
using System;

namespace HOG.GameLogic
{
    public class HOGGameLogicManager: IHOGBaseManager
    {
        public static HOGGameLogicManager Instance;
        public HOGScoreManager ScoreManager;
        public HOGUpgradeManager UpgradeManager;

        public HOGGameLogicManager()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void LoadManager(Action onComplete)
        {
            ScoreManager = new HOGScoreManager();
            UpgradeManager = new HOGUpgradeManager(
                /*() =>
                {
                    StoreManager = new HOGStoreManager();
                }*/
                );
            onComplete.Invoke();
        }
    }
}
