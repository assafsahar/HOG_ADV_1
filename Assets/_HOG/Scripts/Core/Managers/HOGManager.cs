using Firebase.Extensions;
using System;

namespace HOG.Core
{
    public class HOGManager : IHOGBaseManager
    {
        public static HOGManager Instance;

        public HOGCrashManager CrashManager;
        public HOGEventsManager EventsManager;
        public HOGFactoryManager FactoryManager;
        public HOGPoolManager PoolManager;
        public HOGSaveManager SaveManager;
        public HOGConfigManager ConfigManager;

        public Action onInitAction;

        public HOGManager()
        {
            if (Instance != null)
            {
                //HOGDebug.Log("HOGManager Instance already set.");
                return;
            }

            Instance = this;
            //HOGDebug.Log("HOGManager Instance set.");
        }

        public void LoadManager(Action onComplete)
        {
            onInitAction = onComplete;
            InitFirebase(delegate { InitManagers(); });
        }

        private void InitManagers()
        {
            //HOGDebug.Log("HOGManager InitManagers called.");

            try
            {
                CrashManager = new HOGCrashManager();
                //HOGDebug.Log("HOGCrashManager initialized.");

                EventsManager = new HOGEventsManager();
                //HOGDebug.Log("HOGEventsManager initialized.");

                FactoryManager = new HOGFactoryManager();
                //HOGDebug.Log("HOGFactoryManager initialized.");

                PoolManager = new HOGPoolManager();
                //HOGDebug.Log("HOGPoolManager initialized.");

                SaveManager = new HOGSaveManager();
                //HOGDebug.Log("HOGSaveManager initialized.");

                ConfigManager = new HOGConfigManager(delegate
                {
                    //HOGDebug.Log("HOGConfigManager initialized.");
                    onInitAction?.Invoke();
                });
            }
            catch (Exception ex)
            {
                HOGDebug.LogException($"Exception during InitManagers: {ex.Message}");
            }
        }

        private void InitFirebase(Action onComplete)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    var app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    //HOGDebug.Log($"Firebase initialized");
                    onComplete.Invoke();
                }
                else
                {
                    HOGDebug.LogException($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
    }
}
