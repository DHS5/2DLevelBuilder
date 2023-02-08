using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    public static class ExtensionMethods
    {
        private enum SelectionState
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled,
        }

        public static void TransitionImage(this Image image, int state, bool instant, ImageStyleSheet styleSheet)
        {
            if (styleSheet.transition.transitionType == Selectable.Transition.SpriteSwap)
            {
                var targetSprite =
            state == (int)SelectionState.Disabled ? styleSheet.transition.spriteState.disabledSprite :
            state == (int)SelectionState.Highlighted ? styleSheet.transition.spriteState.highlightedSprite :
            state == (int)SelectionState.Pressed ? styleSheet.transition.spriteState.pressedSprite :
            state == (int)SelectionState.Selected ? styleSheet.transition.spriteState.selectedSprite : styleSheet.baseSprite;

                image.sprite = targetSprite;
            }
            else if (styleSheet.transition.transitionType == Selectable.Transition.ColorTint)
            {
                // CrossFadeColor : current color * target color --> black * ... = black / white * ... = ...
                image.color = Color.white;

                var targetColor =
            state == (int)SelectionState.Disabled ? styleSheet.transition.colorBlock.disabledColor :
            state == (int)SelectionState.Highlighted ? styleSheet.transition.colorBlock.highlightedColor :
            state == (int)SelectionState.Pressed ? styleSheet.transition.colorBlock.pressedColor :
            state == (int)SelectionState.Selected ? styleSheet.transition.colorBlock.selectedColor : styleSheet.transition.colorBlock.normalColor;

                image.CrossFadeColor(targetColor, instant ? 0 : styleSheet.transition.colorBlock.fadeDuration, true, true);
            }
        }

        public static void TransitionText(this TextMeshProUGUI text, int state, bool instant, TextStyleSheet styleSheet)
        {
            // CrossFadeColor : current color * target color --> black * ... = black / white * ... = ...
            text.color = Color.white;

            var targetColor =
            state == (int)SelectionState.Disabled ? styleSheet.transition.disabledColor :
            state == (int)SelectionState.Highlighted ? styleSheet.transition.highlightedColor :
            state == (int)SelectionState.Pressed ? styleSheet.transition.pressedColor :
            state == (int)SelectionState.Selected ? styleSheet.transition.selectedColor : styleSheet.transition.normalColor;

            text.CrossFadeColor(targetColor, instant ? 0 : styleSheet.transition.fadeDuration, true, true);
        }

        public static void TransitionGraphic(this Graphic graphic, int state, bool instant, GraphicStyleSheet styleSheet)
        {
            if (graphic is Image) (graphic as Image).TransitionImage(state, instant, styleSheet.imageStyleSheet);
            if (graphic is TextMeshProUGUI) (graphic as TextMeshProUGUI).TransitionText(state, instant, styleSheet.textStyleSheet);
        }


        public static void SetUpImage(this Image image, ImageStyleSheet styleSheet)
        {
            if (image == null) throw new NullReferenceException();

            image.sprite = styleSheet.baseSprite;
            image.color = styleSheet.baseColor;
            image.material = styleSheet.baseMaterial;
            image.type = styleSheet.imageType;
            image.pixelsPerUnitMultiplier = styleSheet.pixelsPerUnit;
        }

        public static void SetUpText(this TextMeshProUGUI text, TextStyleSheet styleSheet)
        {
            text.color = styleSheet.transition.normalColor;
            text.font = styleSheet.font;
            text.fontStyle = styleSheet.fontStyle;
            text.alignment = styleSheet.alignment;
        }
    }
}
