using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HOG.Core
{
    public class HOGConfigManager 
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
