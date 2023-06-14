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
        #region Transition
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
            if (styleSheet.isStatic) return;

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
        public static void TransitionImage(this Image image, int state, bool instant, ImageStyleSheet styleSheet, ImageOverrideSheet overrideSheet)
        {
            if (overrideSheet.isStatic) return;

            if (overrideSheet.transition.transitionType == Selectable.Transition.SpriteSwap)
            {
                var targetSprite =
            state == (int)SelectionState.Disabled ? overrideSheet.transition.spriteState.disabledSprite :
            state == (int)SelectionState.Highlighted ? overrideSheet.transition.spriteState.highlightedSprite :
            state == (int)SelectionState.Pressed ? overrideSheet.transition.spriteState.pressedSprite :
            state == (int)SelectionState.Selected ? overrideSheet.transition.spriteState.selectedSprite :
            overrideSheet.overrideSprite ? overrideSheet.sprite : styleSheet.baseSprite;

                image.sprite = targetSprite;
            }
            else if (overrideSheet.transition.transitionType == Selectable.Transition.ColorTint)
            {
                image.color = Color.white;

                var targetColor =
            state == (int)SelectionState.Disabled ? overrideSheet.transition.colorBlock.disabledColor :
            state == (int)SelectionState.Highlighted ? overrideSheet.transition.colorBlock.highlightedColor :
            state == (int)SelectionState.Pressed ? overrideSheet.transition.colorBlock.pressedColor :
            state == (int)SelectionState.Selected ? overrideSheet.transition.colorBlock.selectedColor : overrideSheet.transition.colorBlock.normalColor;

                image.CrossFadeColor(targetColor, instant ? 0 : overrideSheet.transition.colorBlock.fadeDuration, true, true);
            }
        }

        public static void TransitionText(this TextMeshProUGUI text, int state, bool instant, TextStyleSheet styleSheet)
        {
            if (styleSheet.isStatic) return;

            if (!styleSheet.isGradient)
            {
                // CrossFadeColor : current color * target color --> black * ... = black / white * ... = ...
                text.color = Color.white;

                var targetColor =
                state == (int)SelectionState.Disabled ? styleSheet.colorTransition.disabledColor :
                state == (int)SelectionState.Highlighted ? styleSheet.colorTransition.highlightedColor :
                state == (int)SelectionState.Pressed ? styleSheet.colorTransition.pressedColor :
                state == (int)SelectionState.Selected ? styleSheet.colorTransition.selectedColor : styleSheet.colorTransition.normalColor;

                text.CrossFadeColor(targetColor, instant ? 0 : styleSheet.colorTransition.fadeDuration, true, true);
                return;
            }

            var targetGradient =
            state == (int)SelectionState.Disabled ? styleSheet.gradientTransition.disabledGradient :
            state == (int)SelectionState.Highlighted ? styleSheet.gradientTransition.highlightedGradient :
            state == (int)SelectionState.Pressed ? styleSheet.gradientTransition.pressedGradient :
            state == (int)SelectionState.Selected ? styleSheet.gradientTransition.selectedGradient : 
            styleSheet.gradientTransition.normalGradient;

            text.colorGradient = targetGradient;
        }
        public static void TransitionText(this TextMeshProUGUI text, int state, bool instant, TextOverrideSheet overrideSheet)
        {
            if (overrideSheet.isStatic) return;

            if (!overrideSheet.isGradient)
            {
                text.color = Color.white;

                var targetColor =
                state == (int)SelectionState.Disabled ? overrideSheet.colorTransition.disabledColor :
                state == (int)SelectionState.Highlighted ? overrideSheet.colorTransition.highlightedColor :
                state == (int)SelectionState.Pressed ? overrideSheet.colorTransition.pressedColor :
                state == (int)SelectionState.Selected ? overrideSheet.colorTransition.selectedColor : overrideSheet.colorTransition.normalColor;

                text.CrossFadeColor(targetColor, instant ? 0 : overrideSheet.colorTransition.fadeDuration, true, true);
                return;
            }

            var targetGradient =
            state == (int)SelectionState.Disabled ? overrideSheet.gradientTransition.disabledGradient :
            state == (int)SelectionState.Highlighted ? overrideSheet.gradientTransition.highlightedGradient :
            state == (int)SelectionState.Pressed ? overrideSheet.gradientTransition.pressedGradient :
            state == (int)SelectionState.Selected ? overrideSheet.gradientTransition.selectedGradient : 
            overrideSheet.gradientTransition.normalGradient;

            text.colorGradient = targetGradient;
        }
        #endregion

        #region SetUp Graphic
        public static void SetUpImage(this Image image, ImageStyleSheet styleSheet, AspectRatioFitter ratioFitter = null)
        {
            if (image == null) throw new NullReferenceException();

            image.sprite = styleSheet.baseSprite;
            image.color = styleSheet.baseColor;
            image.material = styleSheet.baseMaterial;
            image.type = styleSheet.imageType;
            image.pixelsPerUnitMultiplier = styleSheet.pixelsPerUnit;

            if (ratioFitter != null && image.enabled)
            {
                ratioFitter.aspectRatio = styleSheet.ratio;
            }
        }
        public static void SetUpImage(this Image image, ImageStyleSheet styleSheet, ImageOverrideSheet overrideSheet, AspectRatioFitter ratioFitter = null)
        {
            if (image == null) throw new NullReferenceException();

            image.sprite = overrideSheet.overrideSprite ? overrideSheet.sprite : styleSheet.baseSprite;
            image.color = overrideSheet.overrideColor ? overrideSheet.color : styleSheet.baseColor;
            image.material = overrideSheet.overrideMaterial ? overrideSheet.material : styleSheet.baseMaterial;
            image.type = overrideSheet.overrideImageType ? overrideSheet.imageType : styleSheet.imageType;
            image.pixelsPerUnitMultiplier = overrideSheet.overrideImageType ? overrideSheet.pixelsPerUnit : styleSheet.pixelsPerUnit;

            if (ratioFitter != null)
            {
                ratioFitter.aspectRatio = overrideSheet.overrideRatio ? overrideSheet.ratio : styleSheet.ratio;
            }
            if (overrideSheet.overrideScale)
            {
                image.transform.localScale = Vector2.one * overrideSheet.scale;
            }
        }

        public static void SetUpText(this TextMeshProUGUI text, TextStyleSheet styleSheet)
        {
            text.color = styleSheet.isGradient ? Color.white :
                styleSheet.isStatic ? styleSheet.color : styleSheet.colorTransition.normalColor;
            text.enableVertexGradient = styleSheet.isGradient;
            if (styleSheet.isGradient)
            {
                text.colorGradient = styleSheet.isStatic ?
                    styleSheet.colorGradient : styleSheet.gradientTransition.normalGradient;
            }

            text.font = styleSheet.font;
            text.fontStyle = styleSheet.fontStyle;
            if (styleSheet.overrideAlignment) text.alignment = styleSheet.alignment;
        }
        public static void SetUpText(this TextMeshProUGUI text, TextStyleSheet styleSheet, TextOverrideSheet overrideSheet)
        {
            if (overrideSheet.overrideColor)
            {
                text.color = overrideSheet.isGradient ? Color.white :
                    overrideSheet.isStatic ? overrideSheet.color : overrideSheet.colorTransition.normalColor;
                text.enableVertexGradient = overrideSheet.isGradient;
                if (overrideSheet.isGradient)
                {
                    text.colorGradient = overrideSheet.isStatic ?
                        overrideSheet.colorGradient : overrideSheet.gradientTransition.normalGradient;
                }
            }
            else
            {
                text.color = styleSheet.isGradient ? Color.white :
                    styleSheet.isStatic ? styleSheet.color : styleSheet.colorTransition.normalColor;
                text.enableVertexGradient = styleSheet.isGradient;
                if (styleSheet.isGradient)
                {
                    text.colorGradient = styleSheet.isStatic ?
                        styleSheet.colorGradient : styleSheet.gradientTransition.normalGradient;
                }
            }

            text.font = overrideSheet.overrideFont ? overrideSheet.font : styleSheet.font;
            text.fontStyle = overrideSheet.overrideFont ? overrideSheet.fontStyle : styleSheet.fontStyle;

            if (overrideSheet.overrideAlignment) text.alignment = overrideSheet.alignment;
            else if (styleSheet.overrideAlignment) text.alignment = styleSheet.alignment;
        }

        public static void SetUpMask(this Image image, ImageStyleSheet styleSheet)
        {
            if (image == null) throw new NullReferenceException();

            image.sprite = styleSheet.baseSprite;
            image.color = Color.white;
            image.material = null;
            image.type = styleSheet.imageType;
            image.pixelsPerUnitMultiplier = styleSheet.pixelsPerUnit;
        }
        public static void SetUpMask(this Image image, ImageStyleSheet styleSheet, ImageOverrideSheet overrideSheet)
        {
            if (image == null) throw new NullReferenceException();

            image.sprite = overrideSheet.overrideSprite ? overrideSheet.sprite : styleSheet.baseSprite;
            image.color = Color.white;
            image.material = null;
            image.type = overrideSheet.overrideImageType ? overrideSheet.imageType : styleSheet.imageType;
            image.pixelsPerUnitMultiplier = overrideSheet.overrideImageType ? overrideSheet.pixelsPerUnit : styleSheet.pixelsPerUnit;
        }
        #endregion

        #region SetUp BaseStyleSheets
        public static void SetUp<T>(this List<T> list, StyleSheetContainer container) where T : BaseStyleSheet
        {
            if (list == null || list.Count < 1) return;

            foreach (T item in list)
            {
                item.SetUp(container);
            }
        }
        #endregion
    }
}
