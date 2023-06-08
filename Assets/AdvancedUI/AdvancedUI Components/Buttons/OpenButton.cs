using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Dhs5.AdvancedUI
{
    public class OpenButton : Button
    {
        #region Events

        // Button Events
        public event Action OnButtonDown;
        public event Action OnButtonUp;
        public event Action OnButtonClick;
        public event Action OnButtonEnter;
        public event Action OnButtonExit;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable)
                OnButtonDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (interactable)
                OnButtonUp?.Invoke();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (interactable)
                OnButtonClick?.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (interactable)
                OnButtonEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (interactable)
                OnButtonExit?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;

        private Image iconImage;
        private ImageStyleSheet iconStyleSheet;
        private ImageOverrideSheet iconOverrideSheet;

        private TextMeshProUGUI textGraphic;
        private TextStyleSheet textStyleSheet;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image icon, ImageStyleSheet _iconStyleSheet, ImageOverrideSheet _iconOverrideSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            iconImage = icon;
            iconStyleSheet = _iconStyleSheet;
            iconOverrideSheet = _iconOverrideSheet;
            textGraphic = text;
            textStyleSheet = _textStyleSheet;

            DoStateTransition(SelectionState.Normal, true);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (backgroundImage != null && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (iconImage != null && iconImage.enabled)
            {
                if (iconOverrideSheet != null && iconOverrideSheet.overrideTransition)
                    iconImage.TransitionImage((int)state, instant, iconStyleSheet, iconOverrideSheet);
                else
                    iconImage.TransitionImage((int)state, instant, iconStyleSheet);
            }
            if (textGraphic != null && textGraphic.enabled) textGraphic.TransitionText((int)state, instant, textStyleSheet);
        }

        #endregion
    }
}
