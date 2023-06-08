using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class SelectableGraphic : Selectable
    {
        public bool selectable = true;

        public ImageStyleSheet ImageStyleSheet { get; private set; }
        public TextStyleSheet TextStyleSheet { get; private set; }

        public void SetStyleSheet(BaseStyleSheet styleSheet)
        {
            ImageStyleSheet = (styleSheet is ImageStyleSheet imageSS) ? imageSS : null;
            TextStyleSheet = (styleSheet is TextStyleSheet textSS) ? textSS : null;

            DoStateTransition(SelectionState.Normal, true);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (ImageStyleSheet != null && targetGraphic is Image image)
                image.TransitionImage((int)state, instant, ImageStyleSheet);
            else if (TextStyleSheet != null && targetGraphic is TextMeshProUGUI text)
                text.TransitionText((int)state, instant, TextStyleSheet);
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

            if (interactable)
            {
                OnMouseEnter?.Invoke();
                onMouseEnter?.Invoke();
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (interactable)
            {
                OnMouseExit?.Invoke();
                onMouseExit?.Invoke();
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (selectable && interactable)
            {
                base.OnSelect(eventData);
            }
        }
        #endregion
    }
}
