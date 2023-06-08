using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ScrollViewStyleSheet : BaseStyleSheet
    {
        public bool backgroundActive = true;
        public StylePicker backgroundStylePicker;
        [Space]
        public StylePicker viewportMaskStylePicker;
        [Space]
        public bool verticalScrollbarActive = true;
        public StylePicker verticalScrollbarStylePicker;
        [Space]
        public bool horizontalScrollbarActive = false;
        public StylePicker horizontalScrollbarStylePicker;

        public ImageStyleSheet BackgroundStyleSheet => backgroundStylePicker.StyleSheet as ImageStyleSheet;
        public ImageStyleSheet ViewportMaskStyleSheet => viewportMaskStylePicker.StyleSheet as ImageStyleSheet;
        public StylePicker VerticalScrollbarStyle => verticalScrollbarStylePicker;
        public StylePicker HorizontalScrollbarStyle => horizontalScrollbarStylePicker;


        public override void SetUp(StyleSheetContainer _container)
        {
            base.SetUp(_container);

            backgroundStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Background");
            viewportMaskStylePicker?.SetUp(container, StyleSheetType.BACKGROUND_IMAGE, "Viewport Mask");
            verticalScrollbarStylePicker?.SetUp(container, StyleSheetType.SCROLLBAR, "Vertical Scrollbar");
            horizontalScrollbarStylePicker?.SetUp(container, StyleSheetType.SCROLLBAR, "Horizontal Scrollbar");
        }
    }
}
