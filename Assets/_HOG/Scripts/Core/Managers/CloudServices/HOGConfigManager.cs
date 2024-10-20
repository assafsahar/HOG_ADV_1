using Firebase.Extensions;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HOG.Core
{
    public class HOGConfigManager
    {
        private Action onInit;
        public HOGConfigManager(Action onComplete)
        {
            onInit = onComplete;
            //HOGDebug.Log("HOGConfigManager constructor called.");
            var defaults = new Dictionary<string, object>();

            defaults.Add("UpgradableConfig", "{}");
            //HOGDebug.Log("HOGConfigManager");
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(OnDefaultValuesSet);
         }

        public void GetConfigAsync<T>(string configID, Action<T> onComplete)
        {
            //HOGDebug.Log("GetConfigAsync");
            var saveJson = FirebaseRemoteConfig.DefaultInstance.GetValue(configID).StringValue;
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }
        private void OnDefaultValuesSet(Task task)
        {
            //HOGDebug.Log("OnDefaultValuesSet");
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(OnFetchComplete);
        }

        private void OnFetchComplete(Task obj)  
        {
            //HOGDebug.Log("OnFetchComplete");
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => OnActivateComplete(task));
        }

        private void OnActivateComplete(Task obj)
        {
            //HOGDebug.Log("OnActivateComplete");
            onInit.Invoke();
        }
    }
    public class HOGConfigOfflineManager
    {
        public void GetConfigAsync<T>(string configID, Action<T> onComplete)
        {
            var path = $"Assets/_HOG/Configs/{configID}.json";
            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }
    }
}
