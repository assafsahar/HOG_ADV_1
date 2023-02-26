using System;

namespace HOG.Core
{
    public class HOGManager : IHOGBaseManager
    {
        public static HOGManager Instance;

        public HOGEventsManager EventsManager;
        public HOGFactoryManager FactoryManager;
        public HOGPoolManager PoolManager;
        public HOGSaveManager SaveManager;
        //public HOGConfigManager

        public HOGManager()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;

           
        }

        public void LoadManager(Action onComplete)
        {
            EventsManager = new HOGEventsManager();
            FactoryManager = new HOGFactoryManager();
            PoolManager = new HOGPoolManager();
            SaveManager = new HOGSaveManager();
            onComplete.Invoke();
        }
    }
}
