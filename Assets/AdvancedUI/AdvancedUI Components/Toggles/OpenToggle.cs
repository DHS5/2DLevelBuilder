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

            if (interactable)
                OnToggleClick?.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (interactable)
                OnToggleEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (interactable)
                OnToggleExit?.Invoke();
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable)
                OnToggleDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (interactable)
                OnToggleUp?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private ImageStyleSheet trueBackgroundStyleSheet;

        private Image checkmarkImage;
        private ImageStyleSheet checkmarkIStyleSheet;
        private TextMeshProUGUI checkmarkText;
        private TextStyleSheet checkmarkTStyleSheet;

        private Image uncheckmarkImage;
        private ImageStyleSheet uncheckmarkIStyleSheet;
        private TextMeshProUGUI uncheckmarkText;
        private TextStyleSheet uncheckmarkTStyleSheet;

        private TextMeshProUGUI toggleText;
        private TextStyleSheet textStyleSheet;

        private bool gotGraphics = false;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet, ImageStyleSheet _trueBackgroundStyleSheet,
            Image checkmarkI, ImageStyleSheet _checkmarkIStyleSheet, TextMeshProUGUI checkmarkT, TextStyleSheet _checkmarkTStyleSheet,
            Image uncheckmarkI, ImageStyleSheet _uncheckmarkIStyleSheet, TextMeshProUGUI uncheckmarkT, TextStyleSheet _uncheckmarkTStyleSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            gotGraphics = true;

            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            trueBackgroundStyleSheet = _trueBackgroundStyleSheet;

            checkmarkImage = checkmarkI;
            checkmarkIStyleSheet = _checkmarkIStyleSheet;
            checkmarkText = checkmarkT;
            checkmarkTStyleSheet = _checkmarkTStyleSheet;

            uncheckmarkImage = uncheckmarkI;
            uncheckmarkIStyleSheet = _uncheckmarkIStyleSheet;
            uncheckmarkText = uncheckmarkT;
            uncheckmarkTStyleSheet = _uncheckmarkTStyleSheet;

            toggleText = text;
            textStyleSheet = _textStyleSheet;

            ForceInstantTransition();
        }

        // No uncheckmark
        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image checkmark, ImageStyleSheet _checkmarkStyleSheet, 
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            gotGraphics = true;

            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            trueBackgroundStyleSheet = null;

            checkmarkImage = checkmark;
            checkmarkIStyleSheet = _checkmarkStyleSheet;
            checkmarkText = null;
            checkmarkTStyleSheet = null;

            uncheckmarkImage = null;
            uncheckmarkIStyleSheet = null;
            uncheckmarkText = null;
            uncheckmarkTStyleSheet = null;

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

            if (backgroundImage && backgroundImage.enabled)
            {
                if (trueBackgroundStyleSheet != null && isOn)
                {
                    backgroundImage.TransitionImage((int)state, instant, trueBackgroundStyleSheet);
                }
                else
                {
                    backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
                }
            }

            if (checkmarkImage && checkmarkImage.enabled) checkmarkImage.TransitionImage((int)state, instant, checkmarkIStyleSheet);
            if (checkmarkText && checkmarkText.enabled) checkmarkText.TransitionText((int)state, instant, checkmarkTStyleSheet);
            
            if (uncheckmarkImage && uncheckmarkImage.enabled) uncheckmarkImage.TransitionImage((int)state, instant, uncheckmarkIStyleSheet);
            if (uncheckmarkText && uncheckmarkText.enabled) uncheckmarkText.TransitionText((int)state, instant, uncheckmarkTStyleSheet);

            if (toggleText && toggleText.enabled) toggleText.TransitionText((int)state, instant, textStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
