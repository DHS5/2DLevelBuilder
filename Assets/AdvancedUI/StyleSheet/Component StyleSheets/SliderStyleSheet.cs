using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class SliderStyleSheet : BaseStyleSheet
    {
        public bool backgroundActive = true;
        public StylePicker backgroundStylePicker;
        [Space, Space]
        public bool fillActive = true;
        public StylePicker fillStylePicker;
        [Space, Space]
        public bool handleActive = true;
        public StylePicker handleStylePicker;
        [Space, Space]
        public bool textActive = false;
        public StylePicker textStylePicker;
        [Space, Space]
        public bool isGradient = false;
        public Gradient sliderGradient;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet FillStyleSheet => fillStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet HandleStyleSheet => handleStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet TextStyleSheet => textStylePicker.StyleSheet as TextStyleSheet;


        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            fillStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Fill");
            handleStylePicker?.SetUp(container, StyleSheetType.ICON_IMAGE, "Handle");
            textStylePicker?.SetUp(container, StyleSheetType.TEXT, "Text type");
        }
    }
}
