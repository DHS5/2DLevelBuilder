using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

namespace Dhs5.AdvancedUI
{
    public class OpenDropdown : TMP_Dropdown
    {
        #region Arrow Resize
        public RectTransform ArrowRect { get; set; }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            ForceResizeArrow();
        }

        private void ForceResizeArrow()
        {
            if (ArrowRect != null)
            {
                ArrowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ArrowRect.rect.height);
            }
        }
        #endregion

        #region Events

        // Events
        public event Action OnClick;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (interactable)
                OnClick?.Invoke();
        }

        #endregion

        #region Transition

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private TextMeshProUGUI titleText;
        private TextStyleSheet titleStyleSheet;
        private Image arrowImage;
        private ImageStyleSheet arrowStyleSheet;
        private TextMeshProUGUI dropdownText;
        private TextStyleSheet textStyleSheet;

        private bool gotGraphics = false;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            TextMeshProUGUI title, TextStyleSheet _titleStyleSheet,
            Image arrow, ImageStyleSheet _arrowStyleSheet,
            TextMeshProUGUI text, TextStyleSheet _textStyleSheet)
        {
            gotGraphics = true;

            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            titleText = title;
            titleStyleSheet = _titleStyleSheet;
            arrowImage = arrow;
            arrowStyleSheet = _arrowStyleSheet;
            dropdownText = text;
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
            if (titleText && titleText.enabled) titleText.TransitionText((int)state, instant, titleStyleSheet);
            if (arrowImage && arrowImage.enabled) arrowImage.TransitionImage((int)state, instant, arrowStyleSheet);
            if (dropdownText) dropdownText.TransitionText((int)state, instant, textStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
