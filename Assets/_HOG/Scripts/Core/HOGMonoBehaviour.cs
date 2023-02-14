using System;
using UnityEngine;

namespace HOG.Core
{
    public class HOGMonoBehaviour : MonoBehaviour
    {
        protected HOGManager Manager => HOGManager.Instance;

        protected void AddListener(HOGEventNames eventName, Action<object> listener) => Manager.EventsManager.AddListener(eventName, listener);
        protected void RemoveListener(HOGEventNames eventName, Action<object> listener) => Manager.EventsManager.RemoveListener(eventName, listener);
        protected void InvokeEvent(HOGEventNames eventName, object obj = null) => Manager.EventsManager.InvokeEvent(eventName, obj);
    }
    

}
