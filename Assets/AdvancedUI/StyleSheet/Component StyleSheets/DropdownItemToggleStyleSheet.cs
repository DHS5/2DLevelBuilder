using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class DropdownItemToggleStyleSheet : BaseStyleSheet
    {
        public bool backgroundActive = true;
        public StylePicker backgroundStylePicker;
        [Space, Space]
        public StylePicker checkmarkStylePicker;
        [Space, Space]
        public StylePicker textStylePicker;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet CheckmarkStyleSheet => checkmarkStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet TextStyleSheet => textStylePicker.StyleSheet as TextStyleSheet;

        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            checkmarkStylePicker.SetUp(container, StyleSheetType.ICON_IMAGE, "Checkmark");
            textStylePicker.SetUp(container, StyleSheetType.TEXT, "Text type");
        }
    }
}
