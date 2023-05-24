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
                return;
            }

            Instance = this;
        }

        public void LoadManager(Action onComplete)
        {
            onInitAction = onComplete;
            InitFirebase(delegate { InitManagers(); });
        }

        private void InitManagers()
        {
            //HOGDebug.Log("InitManagers");
            CrashManager = new HOGCrashManager();
            EventsManager = new HOGEventsManager();
            FactoryManager = new HOGFactoryManager();
            PoolManager = new HOGPoolManager();
            SaveManager = new HOGSaveManager();
            ConfigManager = new HOGConfigManager(delegate
            {
                onInitAction.Invoke();
            });
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
                    HOGDebug.Log($"Firebase initialized");
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
