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

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            Debug.Log(eventData.position);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            Debug.Log(eventData.position);
        }
    }
}
