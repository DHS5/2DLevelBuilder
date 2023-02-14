using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    public class DragableUI : MonoBehaviour
        , IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public bool dragObject = true;
        [ShowIf(nameof(dragObject))][BoxGroup("Drag Options")] public bool dragAlongX = true;
        [ShowIf(nameof(dragObject))][BoxGroup("Drag Options")] public bool dragAlongY = true;


        private Vector2 startDragPos;
        private Vector2 endDragPos;


        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            startDragPos = eventData.position;

            BeginDrag?.Invoke(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData) 
        {
            Drag?.Invoke(eventData);

            if (dragObject)
            {
                Vector2 delta = new(dragAlongX ? eventData.delta.x : 0, dragAlongY ? eventData.delta.y : 0);
                eventData.pointerDrag.transform.Translate(delta);
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            endDragPos = eventData.position;

            EndDrag?.Invoke(eventData);
            DragDelta?.Invoke(endDragPos - startDragPos);
        }


        #region Events

        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> EndDrag;
        public event Action<Vector2> DragDelta;

        #endregion
    }
}
