using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Core
{
    public class HOGEventsManager
    {
        private Dictionary<string, List<Action<object>>> listOfListeners = new();

        public void AddListener(string eventName, Action<object> listener)
        {
            if (listOfListeners.TryGetValue(eventName, out List<Action<object>> listenersList))
            {
                listenersList.Add(listener);
                return;
            }
            listOfListeners.Add(eventName, new List<Action<object>> { listener });
        }

        public void RemoveListener(string eventName, Action<object> listener)
        {
            if (listOfListeners.TryGetValue(eventName, out List<Action<object>> listenersList))
            {
                listenersList.Remove(listener);

                if (listenersList.Count <= 0)
                {
                    listOfListeners.Remove(eventName);
                }
            }
        }
        public void InvokeEvent(string eventName, object obj = null)
        {
            if (listOfListeners.TryGetValue(eventName, out List<Action<object>> listenersList))
            {
                foreach (var listener in listenersList)
                {
                    listener.Invoke(obj);
                }
            }
        }
    }
}
