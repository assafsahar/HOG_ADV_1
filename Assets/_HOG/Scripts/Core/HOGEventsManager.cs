using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Core
{
    public class HOGEventsManager
    {
        private Dictionary<HOGEventNames, List<Action<object>>> activeListeners = new();

        public void AddListener(HOGEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                if (listOfEvents.Contains(onGameStart))
                {
                    return;
                }
                listOfEvents.Add(onGameStart);
                return;
            }

            activeListeners.Add(eventName, new List<Action<object>> { onGameStart });
        }

        public void RemoveListener(HOGEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Remove(onGameStart);

                if (listOfEvents.Count <= 0)
                {
                    activeListeners.Remove(eventName);
                }
            }
        }

        public void InvokeEvent(HOGEventNames eventName, object obj)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                //TODO: Do For Loop
                foreach (var action in listOfEvents)
                {
                    action.Invoke(obj);
                }
            }
        }
    }

    public enum HOGEventNames
    {
        OnScoreSet,
        OnGameStart,
        OnUpgraded,
        PlayerTaken,
        pressSpace,
        ReturnBullet
    }
}
