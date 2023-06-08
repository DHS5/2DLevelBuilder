using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace Dhs5.AdvancedUI
{
    public class OpenSlider : Slider
    {
        public float FillHeight { set { fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value); } }
        

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            ForceResizeHandle();
        }
        public void ForceResizeHandle()
        {
            if (direction == Direction.LeftToRight || direction == Direction.RightToLeft)
                handleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (transform as RectTransform).rect.height);
            else
                handleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (transform as RectTransform).rect.width);
        }

        #region Events

        // Events
        public event Action OnSliderDown;
        public event Action OnSliderUp;


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable)
                OnSliderDown?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (interactable)
                OnSliderUp?.Invoke();
        }

        #endregion

        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private Image foregroundImage;
        private ImageStyleSheet foregroundStyleSheet;
        private Image fillImage;
        private ImageStyleSheet fillStyleSheet;
        private Image handleImage;
        private ImageStyleSheet handleStyleSheet;
        private TextMeshProUGUI sliderText;
        private TextStyleSheet textStyleSheet;
        private TextMeshProUGUI leftText;
        private TextStyleSheet leftTextStyleSheet;
        private TextMeshProUGUI rightText;
        private TextStyleSheet rightTextStyleSheet;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image fill, ImageStyleSheet _fillStyleSheet, Image handle, ImageStyleSheet _handleStyleSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            fillImage = fill;
            fillStyleSheet = _fillStyleSheet;
            handleImage = handle;
            handleStyleSheet = _handleStyleSheet;
            sliderText = text;
            textStyleSheet = _textStyleSheet;

            ForceInstantTransition();
        }
        
        public void GetGraphics(Image handle, ImageStyleSheet _handleStyleSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            handleImage = handle;
            handleStyleSheet = _handleStyleSheet;
            sliderText = text;
            textStyleSheet = _textStyleSheet;

            ForceInstantTransition();
        }

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            Image foreground, ImageStyleSheet _foregroundStyleSheet,
            Image handle, ImageStyleSheet _handleStyleSheet,
            TextMeshProUGUI _leftText, TextStyleSheet _leftTextStyleSheet, 
            TextMeshProUGUI _rightText, TextStyleSheet _rightTextStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            foregroundImage = foreground;
            foregroundStyleSheet = _foregroundStyleSheet;
            handleImage = handle;
            handleStyleSheet = _handleStyleSheet;
            leftText = _leftText;
            leftTextStyleSheet = _leftTextStyleSheet;
            rightText = _rightText;
            rightTextStyleSheet = _rightTextStyleSheet;

            ForceInstantTransition();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (backgroundImage && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (foregroundImage && foregroundImage.enabled) foregroundImage.TransitionImage((int)state, instant, foregroundStyleSheet);
            if (fillImage && fillImage.enabled) fillImage.TransitionImage((int)state, instant, fillStyleSheet);
            if (handleImage && handleImage.enabled) handleImage.TransitionImage((int)state, instant, handleStyleSheet);
            if (sliderText && sliderText.enabled) sliderText.TransitionText((int)state, instant, textStyleSheet);
            if (leftText && leftText.enabled) leftText.TransitionText((int)state, instant, leftTextStyleSheet);
            if (rightText && rightText.enabled) rightText.TransitionText((int)state, instant, rightTextStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
