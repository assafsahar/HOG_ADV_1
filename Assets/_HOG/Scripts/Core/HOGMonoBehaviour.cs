using System;
using System.Collections;
using UnityEngine;

namespace HOG.Core
{
    public class HOGMonoBehaviour : MonoBehaviour
    {
        protected HOGManager Manager => HOGManager.Instance;

        protected void AddListener(HOGEventNames eventName, Action<object> listener) => Manager.EventsManager.AddListener(eventName, listener);
        protected void RemoveListener(HOGEventNames eventName, Action<object> listener) => Manager.EventsManager.RemoveListener(eventName, listener);
        protected void InvokeEvent(HOGEventNames eventName, object obj = null) => Manager.EventsManager.InvokeEvent(eventName, obj);

        public Coroutine WaitForSeconds(float time, Action onComplete)
        {
            return StartCoroutine(WaitForSecondsCoroutine(time, onComplete));
        }

        private IEnumerator WaitForSecondsCoroutine(float time, Action onComplete)
        {
            yield return new WaitForSeconds(time);
            onComplete?.Invoke();
        }

        public Coroutine WaitForFrame(Action onComplete)
        {
            return StartCoroutine(WaitForFrameCoroutine(onComplete));
        }

        private IEnumerator WaitForFrameCoroutine(Action onComplete)
        {
            yield return null;
            onComplete?.Invoke();
        }
    }
    

}
