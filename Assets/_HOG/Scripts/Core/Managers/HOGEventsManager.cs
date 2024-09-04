using System;
using System.Collections.Generic;

namespace HOG.Core
{
    public class HOGEventsManager
    {
        private Dictionary<HOGEventNames, List<Action<object>>> activeListeners = new();
        private Dictionary<HOGEventNames, object> lastEventStates = new();

        public void AddListener(HOGEventNames eventName, Action<object> listener)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                if (!listOfEvents.Contains(listener))
                {
                    listOfEvents.Add(listener);
                }
            }
            else
            {
                activeListeners.Add(eventName, new List<Action<object>> { listener });
            }

            // Check if there's a stored state for this event and invoke it immediately
            if (lastEventStates.TryGetValue(eventName, out var lastState))
            {
                listener.Invoke(lastState);
            }
        }

        public void RemoveListener(HOGEventNames eventName, Action<object> listener)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Remove(listener);

                if (listOfEvents.Count <= 0)
                {
                    activeListeners.Remove(eventName);
                }
            }
        }

        public void InvokeEvent(HOGEventNames eventName, object obj)
        {
            // Store the current state for this event
            lastEventStates[eventName] = obj;

            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                foreach (var action in listOfEvents)
                {
                    action.Invoke(obj);
                }
            }
        }
    }

    public enum HOGEventNames
    {
        OnScoreSet = 0,
        OnGameStart = 1,
        OnUpgraded = 2,
        PlayerTaken = 3,
        ReturnBullet = 4,
        OnAttacksFinish = 5,
        OnAttack = 6,
        OnGetHit = 7,
        OnCharacterDied = 8,
        OnGameReset = 9,
        OnCharacterChange = 10,
        OnAbilityChange = 11,
        OnBackToIdle = 12,
        OnTurnChange = 13,
        OnPreFightReady = 14,
        OnFightReady = 15,
        OnEnergyUpdate = 17,
        OnAttackAnimationComplete = 18
    }
}
