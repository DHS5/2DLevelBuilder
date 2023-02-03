using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.EventSystem
{
    public enum Dhs5Event
    {
        START = 0,
    }

    public static class EventManager<T> where T : Enum
    {
        private static Dictionary<T, Action> eventDico = new();


        public static void StartListening(T keyEvent, Action listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] += listener;
            }
            else
            {
                eventDico.Add(keyEvent, listener);
            }
        }

        public static void StopListening(T keyEvent, Action listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] -= listener;
            }
        }

        public static void TriggerEvent(T keyEvent)
        {
            if (eventDico.TryGetValue(keyEvent, out Action thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}
