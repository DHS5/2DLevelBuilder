using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class SwitchToggle : AdvancedComponent
    {
        #region Switch Toggle Content
        [Serializable]
        public class SwitchToggleContent
        {

            // ### Properties ###
            public bool onClickMode;
            [Space]
            public string leftText;
            public string rightText;
        }
        #endregion

        [Header("Switch Toggle Type")]
        [SerializeField] private StylePicker switchStylePicker;
        public StylePicker Style { get => switchStylePicker; set { switchStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("Dropdown Content")]
        [SerializeField] private SwitchToggleContent switchContent;
        public SwitchToggleContent Content { get { return switchContent; } set { switchContent = value; SetUpConfig(); } }


        public bool Value { get { return slider.value != 0; } set { ForceValue(value); } }

        public override bool Interactable { get => slider.interactable; set => slider.interactable = value; }

        [Header("Events")]
        [SerializeField] private UnityEvent<bool> onValueChanged;
        [SerializeField] private UnityEvent onTrue;
        [SerializeField] private UnityEvent onFalse;

        public event Action<bool> OnValueChanged;
        public event Action OnTrue;
        public event Action OnFalse;


        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private SwitchToggleStyleSheet customStyleSheet;

        private SwitchToggleStyleSheet CurrentStyleSheet 
        { get { return custom ? customStyleSheet : styleSheetContainer ? switchStylePicker.StyleSheet as SwitchToggleStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private OpenSlider slider;
        [Space]
        [SerializeField] private Image background;
        [SerializeField] private Image foreground;
        [Space]
        [SerializeField] private Image handle;
        [Space]
        [SerializeField] private TextMeshProUGUI leftText;
        [SerializeField] private TextMeshProUGUI rightText;

        #region Events
        protected override void LinkEvents()
        {
            slider.onValueChanged.AddListener(ValueChanged);
            slider.OnSliderDown += ForceValueChange;
        }
        protected override void UnlinkEvents()
        {
            slider.onValueChanged?.RemoveListener(ValueChanged);
            slider.OnSliderDown -= ForceValueChange;
        }

        private void ValueChanged(float value)
        {
            bool boolValue = value != 0;

            onValueChanged?.Invoke(boolValue);
            OnValueChanged?.Invoke(boolValue);

            background.enabled = !boolValue;
            foreground.enabled = boolValue;

            True();
            False();
        }
        private void True()
        {
            if (!Value) return;

            onTrue?.Invoke();
            OnTrue?.Invoke();
        }
        private void False()
        {
            if (Value) return;

            onFalse?.Invoke();
            OnFalse?.Invoke();
        }
        private void ForceValueChange()
        {
            if (Content.onClickMode)
                slider.value = slider.value != 0 ? 0 : 1;
        }
        private void ForceValue(bool state)
        {
            slider.value = state ? 1 : 0;
        }
        #endregion

        #region Configs
        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            switchStylePicker.SetUp(styleSheetContainer, StyleSheetType.SWITCH_TOGGLE, "Switch Type");

            if (CurrentStyleSheet == null) return;

            // Slider
            if (slider)
            {
                slider.interactable = !Content.onClickMode;
            }

            // Background
            if (background)
            {
                background.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }
            // Foreground
            if (foreground)
            {
                foreground.SetUpImage(CurrentStyleSheet.ForegroundStyleSheet);
            }

            // Handle
            if (handle)
            {
                handle.SetUpImage(CurrentStyleSheet.HandleStyleSheet);
            }

            // Left Text
            if (leftText)
            {
                leftText.enabled = CurrentStyleSheet.leftTextActive;
                leftText.SetUpText(CurrentStyleSheet.LeftTextStyleSheet);
                leftText.text = Content.leftText;
            }
            // Right Text
            if (rightText)
            {
                rightText.enabled = CurrentStyleSheet.rightTextActive;
                rightText.SetUpText(CurrentStyleSheet.RightTextStyleSheet);
                rightText.text = Content.rightText;
            }
        }

        protected override void SetUpGraphics()
        {
            slider.GetGraphics(background, CurrentStyleSheet.BackgroundStyleSheet,
                foreground, CurrentStyleSheet.ForegroundStyleSheet,
                handle, CurrentStyleSheet.HandleStyleSheet,
                leftText, CurrentStyleSheet.LeftTextStyleSheet,
                rightText, CurrentStyleSheet.RightTextStyleSheet);
        }
        #endregion
    }
}
