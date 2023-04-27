using Firebase.Extensions;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace HOG.Core
{
    public class HOGConfigManager
    {
        private Action onInit;
        public HOGConfigManager(Action onComplete)
        {
            onInit = onComplete;
            var defaults = new Dictionary<string, object>();

            defaults.Add("UpgradableConfig", "{}");

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(OnDefaultValuesSet);

         }

        private void OnDefaultValuesSet(Task task)
        {
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(OnFetchComplete);
        }

        private void OnFetchComplete(Task obj)  
        {
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => OnActivateComplete(task));
        }

        private void OnActivateComplete(Task obj)
        {
            HOGDebug.Log("OnActivateComplete");
            onInit.Invoke();
        }

        public void GetConfigAsync<T>(string configID, Action<T> onComplete)
        {
            HOGDebug.Log("GetConfigAsync");
            var saveJson = FirebaseRemoteConfig.DefaultInstance.GetValue(configID).StringValue;
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
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
