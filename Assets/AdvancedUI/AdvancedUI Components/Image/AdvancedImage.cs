using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Dhs5.AdvancedUI
{
    public class AdvancedImage : AdvancedComponent
    {
        [Serializable]
        public enum ImageType
        {
            BACKGROUND,
            ICON,
        }

        [Header("Image Type")]
        [SerializeField] private ImageType type;
        [SerializeField] private StylePicker imageStylePicker;
        public StylePicker Style { get => imageStylePicker; set { imageStylePicker.ForceSet(value); SetUpConfig(); } }
        [SerializeField] private bool selectable;

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ImageStyleSheet customStyleSheet;

        private ImageStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? imageStylePicker.StyleSheet as ImageStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private SelectableGraphic imageGraphic;

        public override bool Interactable { get => imageGraphic.interactable; set { imageGraphic.interactable = value; SetUpConfig(); } }

        public event Action OnMouseEnter { add { imageGraphic.OnMouseEnter += value; } remove { imageGraphic.OnMouseEnter -= value; } }
        public event Action OnMouseExit { add { imageGraphic.OnMouseExit += value; } remove { imageGraphic.OnMouseExit -= value; } }


        #region Events
        protected override void LinkEvents() { }
        protected override void UnlinkEvents() { }
        #endregion

        #region Configs
        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            imageStylePicker.SetUp(styleSheetContainer, GetStyleSheetType(type));
            
            if (CurrentStyleSheet == null) return;

            if (imageGraphic && imageGraphic.targetGraphic is Image image)
            {
                image.SetUpImage(CurrentStyleSheet);
                imageGraphic.selectable = selectable;
            }
        }

        protected override void SetUpGraphics()
        {
            imageGraphic.SetStyleSheet(CurrentStyleSheet);
        }
        #endregion

        #region Private functions
        private StyleSheetType GetStyleSheetType(ImageType type)
        {
            return type switch
            {
                ImageType.BACKGROUND => StyleSheetType.BACKGROUND_IMAGE,
                ImageType.ICON => StyleSheetType.ICON_IMAGE,
                _ => StyleSheetType.BACKGROUND_IMAGE,
            };
        }
        #endregion
    }
}
