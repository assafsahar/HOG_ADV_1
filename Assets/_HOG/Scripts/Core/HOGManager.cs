namespace HOG.Core
{
    public class HOGManager
    {
        public static HOGManager Instance;

        public HOGEventsManager EventsManager;
        
        public HOGManager()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;

            EventsManager = new HOGEventsManager();
        }

    }
}
