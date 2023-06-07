using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class SwitchToggleStyleSheet : BaseStyleSheet
    {
        public StylePicker backgroundStylePicker;
        public StylePicker foregroundStylePicker;
        [Space, Space]
        public StylePicker handleStylePicker;
        [Space, Space]
        public bool leftTextActive;
        public StylePicker leftTextStylePicker;
        public bool rightTextActive;
        public StylePicker rightTextStylePicker;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet ForegroundStyleSheet => foregroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet HandleStyleSheet => handleStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet LeftTextStyleSheet => leftTextStylePicker.StyleSheet as TextStyleSheet;
        public TextStyleSheet RightTextStyleSheet => rightTextStylePicker.StyleSheet as TextStyleSheet;

        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            foregroundStylePicker.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Foreground");
            handleStylePicker.SetUp(container, StyleSheetType.ICON_IMAGE, "Handle");
            leftTextStylePicker.SetUp(container, StyleSheetType.TEXT, "Left Text type");
            rightTextStylePicker.SetUp(container, StyleSheetType.TEXT, "Right Text type");
        }
    }
}
