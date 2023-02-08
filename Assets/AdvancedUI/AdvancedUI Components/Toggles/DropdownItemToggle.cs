using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    public class DropdownItemToggle : AdvancedComponent
    {
        [Header("Toggle Type")]
        [SerializeField] private DropdownItemToggleType toggleType;
        public DropdownItemToggleType Type { get { return toggleType; } set { toggleType = value; SetUpConfig(); } }

        [Header("Toggle Content")]
        [SerializeField] private bool isOn = true;
        [SerializeField] private bool overrideText = false;
        [SerializeField] private string text;

        public override bool Interactable { get => toggle.interactable; set => toggle.interactable = value; }


        [Header("Custom Style Sheet")]
        [SerializeField] private DropdownItemToggleStyleSheet customStyleSheet;


        [Header("Style Sheet Container")]
        [SerializeField] private StyleSheetContainer styleSheetContainer;
        private DropdownItemToggleStyleSheet CurrentStyleSheet { get { return toggleType == DropdownItemToggleType.CUSTOM ? customStyleSheet :
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.dropdownItemToggleStyleSheets.GetStyleSheet(toggleType) : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenToggle toggle;
        [SerializeField] private Image toggleBackground;
        [Space]
        [SerializeField] private Image checkmarkImage;
        [Space]
        [SerializeField] private TextMeshProUGUI toggleText;


        protected override void Awake()
        {
            toggle.GetGraphics(toggleBackground, CurrentStyleSheet.backgroundStyleSheet,
                checkmarkImage, CurrentStyleSheet.checkmarkStyleSheet,
                toggleText, CurrentStyleSheet.textStyleSheet);

            base.Awake();
        }

        #region Public Accessors & Methods

        public bool State { get { return toggle.isOn; } set { toggle.isOn = value; } }
        public string Text { get { return overrideText ? text : toggleText.text; } set { if (overrideText) toggleText.text = value; } }

        public void ActuState()
        {
            if (checkmarkImage) checkmarkImage.enabled = State;

            toggle.ForceInstantTransition();
        }

        #endregion

        #region Events

        public event Action<bool> OnValueChanged { add { toggle.OnValueChanged += value; } remove { toggle.OnValueChanged -= value; } }

        protected override void LinkEvents()
        {
            OnValueChanged += ValueChanged;
        }
        protected override void UnlinkEvents()
        {
            OnValueChanged -= ValueChanged;
        }

        private void ValueChanged(bool state)
        {
            isOn = state;
            ActuState();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            State = isOn;

            if (CurrentStyleSheet == null) return;

            // Background
            if (toggleBackground != null)
            {
                toggleBackground.enabled = CurrentStyleSheet.backgroundActive;
                toggleBackground.SetUpImage(CurrentStyleSheet.backgroundStyleSheet);
            }

            // Checkmark Icon
            if (checkmarkImage != null)
            {
                checkmarkImage.SetUpImage(CurrentStyleSheet.checkmarkStyleSheet);
            }

            // Text
            if (toggleText != null)
            {
                toggleText.text = Text;
                toggleText.SetUpText(CurrentStyleSheet.textStyleSheet);
            }

            ActuState();
        }

        private void OnTransformParentChanged()
        {
            if (checkmarkImage)
                checkmarkImage.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, (toggle.transform as RectTransform).rect.height);
        }

        #endregion
    }
}
