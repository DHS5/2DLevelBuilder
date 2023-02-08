using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    public class OpenToggle : Toggle
    {
        #region Events

        // Button Events
        public event Action<bool> OnValueChanged;
        public event Action OnToggleClick;
        public event Action OnToggleEnter;
        public event Action OnToggleExit;
        public event Action OnToggleDown;
        public event Action OnToggleUp;

        protected override void OnEnable()
        {
            base.OnEnable();

            onValueChanged.AddListener(ValueChanged);
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            onValueChanged.RemoveListener(ValueChanged);
        }

        private void ValueChanged(bool state)
        {
            OnValueChanged?.Invoke(state);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnToggleClick?.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnToggleEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnToggleExit?.Invoke();
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            OnToggleDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            OnToggleUp?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private Graphic checkmarkGraphic;
        private GraphicStyleSheet checkmarkGStyleSheet;
        private Image checkmarkImage;
        private ImageStyleSheet checkmarkIStyleSheet;
        private Graphic uncheckmarkGraphic;
        private GraphicStyleSheet uncheckmarkStyleSheet;
        private TextMeshProUGUI toggleText;
        private TextStyleSheet textStyleSheet;

        private bool gotGraphics = false;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Graphic checkmark, GraphicStyleSheet _checkmarkStyleSheet, Graphic uncheckmark, GraphicStyleSheet _uncheckmarkStyleSheet, 
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            gotGraphics = true;

            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            checkmarkGraphic = checkmark;
            checkmarkGStyleSheet = _checkmarkStyleSheet;
            uncheckmarkGraphic = uncheckmark;
            uncheckmarkStyleSheet = _uncheckmarkStyleSheet;
            toggleText = text;
            textStyleSheet = _textStyleSheet;

            ForceInstantTransition();
        }
        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image checkmark, ImageStyleSheet _checkmarkStyleSheet, 
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            gotGraphics = true;

            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            checkmarkImage = checkmark;
            checkmarkIStyleSheet = _checkmarkStyleSheet;
            toggleText = text;
            textStyleSheet = _textStyleSheet;

            ForceInstantTransition();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gotGraphics)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            if (backgroundImage && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (uncheckmarkGraphic && uncheckmarkGraphic.enabled) uncheckmarkGraphic.TransitionGraphic((int)state, instant, uncheckmarkStyleSheet);
            if (toggleText && toggleText.enabled) toggleText.TransitionText((int)state, instant, textStyleSheet);

            if (checkmarkGraphic && checkmarkGraphic.enabled) checkmarkGraphic.TransitionGraphic((int)state, instant, checkmarkGStyleSheet);
            else if (checkmarkImage) checkmarkImage.TransitionImage((int)state, instant, checkmarkIStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
