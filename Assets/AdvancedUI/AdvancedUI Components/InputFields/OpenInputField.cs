using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class OpenInputField : TMP_InputField
    {
        #region Transitions

        private Image backgroundImage;
        private ImageStyleSheet backgroundStyleSheet;
        private TextMeshProUGUI hintText;
        private TextStyleSheet hintStyleSheet;
        private TextMeshProUGUI inputText;
        private TextStyleSheet inputStyleSheet;

        public void GetGraphics(Image background, ImageStyleSheet _backgroundStyleSheet,
            TextMeshProUGUI _hintText, TextStyleSheet _hintStyleSheet, TextMeshProUGUI _inputText, TextStyleSheet _inputStyleSheet)
        {
            backgroundImage = background;
            backgroundStyleSheet = _backgroundStyleSheet;
            hintText = _hintText;
            hintStyleSheet = _hintStyleSheet;
            inputText = _inputText;
            inputStyleSheet = _inputStyleSheet;

            ForceInstantTransition();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (backgroundImage && backgroundImage.enabled) backgroundImage.TransitionImage((int)state, instant, backgroundStyleSheet);
            if (hintText && hintText.enabled) hintText.TransitionText((int)state, instant, hintStyleSheet);
            if (inputText && inputText.enabled) inputText.TransitionText((int)state, instant, inputStyleSheet);
        }

        public void ForceInstantTransition()
        {
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion
    }
}
