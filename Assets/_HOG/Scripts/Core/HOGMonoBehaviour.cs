using System;
using UnityEngine;

namespace HOG.Core
{
    public class HOGMonoBehaviour : MonoBehaviour
    {
        protected HOGManager Manager => HOGManager.Instance;

        protected void AddListener(string eventName, Action<object> listener) => Manager.EventsManager.AddListener(eventName, listener);
        protected void RemoveListener(string eventName, Action<object> listener) => Manager.EventsManager.RemoveListener(eventName, listener);
        protected void InvokeEvent(string eventName, object obj = null) => Manager.EventsManager.InvokeEvent(eventName, obj);
    }
    

}
