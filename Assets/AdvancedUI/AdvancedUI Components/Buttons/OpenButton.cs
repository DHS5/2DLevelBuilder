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

            OnButtonDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            OnButtonUp?.Invoke();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnButtonClick?.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnButtonEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnButtonExit?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private Image iconImage;
        private ImageStyleSheet iconStyleSheet;
        private TextMeshProUGUI textGraphic;
        private TextStyleSheet textStyleSheet;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image icon, ImageStyleSheet _iconStyleSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            iconImage = icon;
            iconStyleSheet = _iconStyleSheet;
            textGraphic = text;
            textStyleSheet = _textStyleSheet;

            DoStateTransition(SelectionState.Normal, true);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (backgroundImage != null && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (iconImage != null && iconImage.enabled) iconImage.TransitionImage((int)state, instant, iconStyleSheet);
            if (textGraphic != null && textGraphic.enabled) textGraphic.TransitionText((int)state, instant, textStyleSheet);
        }

        #endregion
    }
}
