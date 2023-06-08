using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ToggleStyleSheet : BaseStyleSheet
    {
        public bool backgroundActive = true;
        public StylePicker backgroundStylePicker;
        [Space, Space]
        public bool checkmarkActive = true;
        public bool checkmarkIsImage = true;
        public float checkmarkScale = 1;
        public StylePicker checkmarkImageStylePicker;
        public StylePicker checkmarkTextStylePicker;
        [Space, Space]
        public bool uncheckmarkActive = true;
        public bool uncheckmarkIsImage = true;
        public float uncheckmarkScale = 1;
        public StylePicker uncheckmarkImageStylePicker;
        public StylePicker uncheckmarkTextStylePicker;
        [Space, Space]
        public bool textActive = true;
        public StylePicker textStylePicker;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet CheckmarkImageStyleSheet => checkmarkImageStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet UncheckmarkImageStyleSheet => uncheckmarkImageStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet CheckmarkTextStyleSheet => checkmarkTextStylePicker.StyleSheet as TextStyleSheet;
        public TextStyleSheet UncheckmarkTextStyleSheet => uncheckmarkTextStylePicker.StyleSheet as TextStyleSheet;
        public TextStyleSheet TextStyleSheet => textStylePicker.StyleSheet as TextStyleSheet;

        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            checkmarkImageStylePicker?.SetUp(container, StyleSheetType.ICON_IMAGE, "Checkmark Image");
            uncheckmarkImageStylePicker?.SetUp(container, StyleSheetType.ICON_IMAGE, "Uncheckmark Image");
            checkmarkTextStylePicker?.SetUp(container, StyleSheetType.TEXT, "Checkmark Text type");
            uncheckmarkTextStylePicker?.SetUp(container, StyleSheetType.TEXT, "Uncheckmark Text type");
            textStylePicker?.SetUp(container, StyleSheetType.TEXT, "Text type");
        }
    }
}
