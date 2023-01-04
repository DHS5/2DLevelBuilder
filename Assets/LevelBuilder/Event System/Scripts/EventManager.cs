using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LevelBuilder2D
{
    public static class EventManager
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

        private static Dictionary<LevelBuilderEvent, Action> eventDico = new();



        public static void StartListening(LevelBuilderEvent keyEvent, Action listener)
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

        public static void StopListening(LevelBuilderEvent keyEvent, Action listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] -= listener;
            }
        }

        public static void TriggerEvent(LevelBuilderEvent keyEvent)
        {
            if (eventDico.TryGetValue(keyEvent, out Action thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}
