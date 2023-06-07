using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class DropdownStyleSheet : BaseStyleSheet
    {
        public bool backgroundActive = true;
        public StylePicker backgroundStylePicker;
        [Space, Space]
        public bool titleActive = true;
        public StylePicker titleTextStylePicker;
        [Space, Space]
        public bool arrowActive = true;
        public StylePicker arrowStylePicker;
        [Space, Space]
        public StylePicker textStylePicker;
        [Space, Space]
        [SerializeField] private StylePicker templateScrollviewStylePicker;
        [Space, Space]
        [SerializeField] private StylePicker itemToggleStylePicker;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet TitleStyleSheet => titleTextStylePicker.StyleSheet as TextStyleSheet;
        public ImageStyleSheet ArrowStyleSheet => arrowStylePicker.StyleSheet as ImageStyleSheet;
        public TextStyleSheet TextStyleSheet => textStylePicker.StyleSheet as TextStyleSheet;
        public StylePicker TemplateScrollviewStyle => templateScrollviewStylePicker;
        public StylePicker ItemToggleStyle => itemToggleStylePicker;

        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            titleTextStylePicker.SetUp(container, StyleSheetType.TEXT, "Title Text type");
            arrowStylePicker.SetUp(container, StyleSheetType.ICON_IMAGE, "Arrow Icon");
            textStylePicker.SetUp(container, StyleSheetType.TEXT, "Text type");
            templateScrollviewStylePicker.SetUp(container, StyleSheetType.SCROLL_VIEW, "Scrollview");
            itemToggleStylePicker.SetUp(container, StyleSheetType.DROPDOWN_ITEM_TOGGLE, "Item Toggle");
        }
    }
}
