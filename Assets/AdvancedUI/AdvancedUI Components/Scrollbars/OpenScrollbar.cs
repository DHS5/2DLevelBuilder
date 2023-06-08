using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Dhs5.AdvancedUI
{
    public class OpenScrollbar : Scrollbar
    {
        #region Events

        // Events
        public event Action OnScrollbarDown;
        public event Action OnScrollbarUp;


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable)
                OnScrollbarDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (interactable)
                OnScrollbarUp?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private Image handleImage;
        private ImageStyleSheet handleStyleSheet;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image handle, ImageStyleSheet _handleStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            handleImage = handle;
            handleStyleSheet = _handleStyleSheet;

            ForceInstantTransition();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (backgroundImage && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (handleImage && handleImage.enabled) handleImage.TransitionImage((int)state, instant, handleStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
