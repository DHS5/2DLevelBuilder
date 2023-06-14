using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    public class UIMask : AdvancedComponent
    {
        [Header("Mask type")]
        [SerializeField] private StylePicker maskStylePicker;
        public StylePicker Style { get => maskStylePicker; set { maskStylePicker.ForceSet(value); SetUpConfig(); } }

        public override bool Interactable { get => maskImage.enabled; set => maskImage.enabled = value; }

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ImageStyleSheet customStyleSheet;

        [Header("Overrides")]
        [SerializeField] private bool overrideMask;
        [SerializeField] private ImageOverrideSheet maskOverrideSheet;

        private ImageStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? maskStylePicker.StyleSheet as ImageStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private Image maskImage;
        [SerializeField] private SoftMask mask;

        #region Events
        protected override void LinkEvents() { }
        protected override void UnlinkEvents() { }
        #endregion

        #region Configs
        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            maskStylePicker.SetUp(styleSheetContainer, StyleSheetType.BACKGROUND_IMAGE, "Mask Style");

            if (CurrentStyleSheet == null) return;

            if (maskImage)
            {
                if (!overrideMask)
                {
                    maskImage.SetUpMask(CurrentStyleSheet);
                }
                else
                {
                    maskImage.SetUpMask(CurrentStyleSheet, maskOverrideSheet);
                }
            }

            if (mask) mask.FixChildren();
        }

        protected override void SetUpGraphics() { }
        #endregion
    }
}
