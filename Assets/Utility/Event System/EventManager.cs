using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.EventSystem
{
    public enum LevelBuilderEvent
    {
        CREATE_BUILDER = 0,
        BUILDER_CREATED = 1,
        OPEN_BUILDER = 2,
        QUIT_BUILDER = 3,
        SAVE_LEVEL = 4,
        BEFORE_SAVE = 5,
        DO_ACTION = 6,
        UNDO_ACTION = 7,
        OPEN_HELP = 8,
        QUIT_HELP = 9
    }

    public static class EventManager<T> where T :Enum
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
