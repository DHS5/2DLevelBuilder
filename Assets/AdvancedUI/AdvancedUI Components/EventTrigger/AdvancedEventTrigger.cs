using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Dhs5.AdvancedUI
{
    public class AdvancedEventTrigger : EventTrigger
    {
        public event Action onPointerDown;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            onPointerDown?.Invoke();
        }
    }
}
