using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Dhs5.AdvancedUI
{
    public class SelectableGraphic : Selectable
    {
        public GraphicStyleSheet GraphicStyleSheet { get; set; }


        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (GraphicStyleSheet != null)
                targetGraphic.TransitionGraphic((int)state, instant, GraphicStyleSheet);
            else
                base.DoStateTransition(state, instant);
        }


        #region Events

        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action OnMouseEnter;
        public event Action OnMouseExit;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnMouseEnter?.Invoke();
            onMouseEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnMouseExit?.Invoke();
            onMouseExit?.Invoke();
        }

        #endregion
    }
}
