using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ScrollListStyleSheet : BaseStyleSheet
    {
        public bool frameActive = false;
        public StylePicker frameStylePicker;
        public StylePicker frameMaskStylePicker;
        public StylePicker backgroundStylePicker;
        [Space]
        public StylePicker horizontalMaskStylePicker;
        public StylePicker verticalMaskStylePicker;
        [Space]
        public StylePicker textStylePicker;
        [Space]
        [SerializeField] private StylePicker leftButtonStylePicker;
        [SerializeField] private StylePicker rightButtonStylePicker;


        public ImageStyleSheet FrameStyleSheet => frameStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet FrameMaskStyleSheet => frameMaskStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet HorizontalMaskStyleSheet => horizontalMaskStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet VerticalMaskStyleSheet => verticalMaskStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet TextStyleSheet => textStylePicker.StyleSheet as TextStyleSheet;
        public StylePicker LeftButtonStyle => leftButtonStylePicker;
        public StylePicker RightButtonStyle => rightButtonStylePicker;


        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            frameStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Frame");
            frameMaskStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Frame mask");
            backgroundStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            horizontalMaskStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Horizontal mask");
            verticalMaskStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Vertical mask");

            textStylePicker?.SetUp(container, StyleSheetType.TEXT, "Text Type");
            leftButtonStylePicker?.SetUp(container, StyleSheetType.BUTTON, "Left Button");
            rightButtonStylePicker?.SetUp(container, StyleSheetType.BUTTON, "Right Button");
        }
    }
}
