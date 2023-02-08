using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using TMPro;

namespace Dhs5.AdvancedUI
{
    #region Switch Toggle Content
    [Serializable]
    public struct SwitchToggleContent
    {

        // ### Properties ###
        public bool onClickMode;
        [Space]
        public string leftText;
        public string rightText;
    }
    #endregion

    public class SwitchToggle : AdvancedComponent
    {
        [Header("Switch Toggle Type")]
        [SerializeField] private SwitchToggleType switchType;
        public SwitchToggleType Type { get { return switchType; } set { switchType = value; SetUpConfig(); } }

        [Header("Dropdown Content")]
        [SerializeField] private SwitchToggleContent switchContent;
        public SwitchToggleContent Content { get { return switchContent; } set { switchContent = value; SetUpConfig(); } }


        public bool Value { get { return slider.value != 0; } set { ForceValue(value); } }

        public override bool Interactable { get => slider.interactable; set => slider.interactable = value; }

        [Header("Events")]
        [SerializeField] private UnityEvent<bool> onValueChanged;

        public event Action<bool> OnValueChanged;


        [Header("Custom Style Sheet")]
        [SerializeField] private SwitchToggleStyleSheet customStyleSheet;


        [Header("Style Sheet Container")]
        [SerializeField] private StyleSheetContainer styleSheetContainer;
        private SwitchToggleStyleSheet CurrentStyleSheet { get { return switchType == SwitchToggleType.CUSTOM ? customStyleSheet : 
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.switchToggleStyleSheets.GetStyleSheet(switchType) : null; } }

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


        protected override void Awake()
        {
            slider.GetGraphics(background, CurrentStyleSheet.backgroundStyleSheet,
                foreground, CurrentStyleSheet.foregroundStyleSheet,
                handle, CurrentStyleSheet.handleStyleSheet,
                leftText, CurrentStyleSheet.leftTextStyleSheet,
                rightText, CurrentStyleSheet.rightTextStyleSheet);

            base.Awake();
        }


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
            if (CurrentStyleSheet == null) return;

            // Slider
            if (slider)
            {
                slider.interactable = !Content.onClickMode;
            }

            // Background
            if (background)
            {
                background.SetUpImage(CurrentStyleSheet.backgroundStyleSheet);
            }
            // Foreground
            if (foreground)
            {
                foreground.SetUpImage(CurrentStyleSheet.foregroundStyleSheet);
            }

            // Handle
            if (handle)
            {
                handle.SetUpImage(CurrentStyleSheet.handleStyleSheet);
            }

            // Left Text
            if (leftText)
            {
                leftText.enabled = CurrentStyleSheet.leftTextActive;
                leftText.SetUpText(CurrentStyleSheet.leftTextStyleSheet);
                leftText.text = Content.leftText;
            }
            // Right Text
            if (rightText)
            {
                rightText.enabled = CurrentStyleSheet.rightTextActive;
                rightText.SetUpText(CurrentStyleSheet.rightTextStyleSheet);
                rightText.text = Content.rightText;
            }
        }
        #endregion
    }
}
