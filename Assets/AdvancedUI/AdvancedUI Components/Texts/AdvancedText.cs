using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class AdvancedText : AdvancedComponent
    {
        [Header("Text Type")]
        public StylePicker textStylePicker;
        public bool selectable;

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private TextStyleSheet customStyleSheet;

        private TextStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? textStylePicker.StyleSheet as TextStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private SelectableGraphic textGraphic;

        public override bool Interactable { get => true; set => SetUpConfig(); }

        #region Events
        protected override void LinkEvents() { }
        protected override void UnlinkEvents() { }
        #endregion

        #region Configs
        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            textStylePicker.SetUp(styleSheetContainer, StyleSheetType.TEXT);

            if (CurrentStyleSheet == null) return;

            if (textGraphic && textGraphic.targetGraphic is TextMeshProUGUI text)
            {
                text.SetUpText(CurrentStyleSheet);
                textGraphic.selectable = selectable;
            }
        }

        protected override void SetUpGraphics()
        {
            textGraphic.SetStyleSheet(CurrentStyleSheet);
        }
        #endregion
    }
}
