using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dhs5.AdvancedUI
{
    public class AdvancedScrollbar : AdvancedComponent
    {
        [Header("Scrollbar Type")]
        [SerializeField] private StylePicker scrollbarStylePicker;
        public StylePicker Style { get => scrollbarStylePicker; set { scrollbarStylePicker.ForceSet(value); SetUpConfig(); } }

        public override bool Interactable { get => scrollbar.interactable; set => scrollbar.interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent<float> onValueChanged;
        [SerializeField] private UnityEvent onButtonDown;
        [SerializeField] private UnityEvent onButtonUp;

        public event Action<float> OnValueChanged;
        public event Action OnButtonDown { add { scrollbar.OnScrollbarDown += value; } remove { scrollbar.OnScrollbarDown -= value; } }
        public event Action OnButtonUp { add { scrollbar.OnScrollbarUp += value; } remove { scrollbar.OnScrollbarUp -= value; } }

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ScrollbarStyleSheet customStyleSheet;

        private ScrollbarStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? Style.StyleSheet as ScrollbarStyleSheet : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenScrollbar scrollbar;
        [Space]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image handle;


        #region Events

        protected override void LinkEvents()
        {
            scrollbar.onValueChanged.AddListener(ValueChanged);
            OnButtonDown += ButtonDown;
            OnButtonUp += ButtonUp;
        }
        protected override void UnlinkEvents()
        {
            scrollbar.onValueChanged?.RemoveListener(ValueChanged);
            OnButtonDown -= ButtonDown;
            OnButtonUp -= ButtonUp;
        }

        private void ValueChanged(float value)
        {
            onValueChanged?.Invoke(value);
            OnValueChanged?.Invoke(value);
        }
        private void ButtonDown()
        {
            onButtonDown?.Invoke();
        }
        private void ButtonUp()
        {
            onButtonUp?.Invoke();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            scrollbarStylePicker.SetUp(styleSheetContainer, StyleSheetType.SCROLLBAR, "Scrollbar Type");

            if (CurrentStyleSheet == null) return;

            // Background
            if (backgroundImage)
            {
                backgroundImage.enabled = CurrentStyleSheet.backgroundActive;
                backgroundImage.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }

            // Handle
            if (handle)
            {
                handle.SetUpImage(CurrentStyleSheet.HandleStyleSheet);
            }
        }

        protected override void SetUpGraphics()
        {
            scrollbar.GetGraphics(backgroundImage, CurrentStyleSheet.BackgroundStyleSheet,
                handle, CurrentStyleSheet.HandleStyleSheet);
        }

        #endregion
    }
}
