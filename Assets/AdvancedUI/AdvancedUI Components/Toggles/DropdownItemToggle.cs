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
        [SerializeField] private StylePicker toggleStylePicker;
        public StylePicker Style { get => toggleStylePicker; set { toggleStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("Toggle Content")]
        [SerializeField] private bool isOn = true;
        [SerializeField] private bool overrideText = false;
        [SerializeField] private string text;

        public override bool Interactable { get => toggle.interactable; set => toggle.interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent<bool> onValueChanged;
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action<bool> OnValueChanged { add { toggle.OnValueChanged += value; } remove { toggle.OnValueChanged -= value; } }
        public event Action OnClick { add { toggle.OnToggleClick += value; } remove { toggle.OnToggleClick -= value; } }
        public event Action OnMouseEnter { add { toggle.OnToggleEnter += value; } remove { toggle.OnToggleEnter -= value; } }
        public event Action OnMouseExit { add { toggle.OnToggleExit += value; } remove { toggle.OnToggleExit -= value; } }

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private DropdownItemToggleStyleSheet customStyleSheet;

        private DropdownItemToggleStyleSheet CurrentStyleSheet 
        { get { return custom ? customStyleSheet : styleSheetContainer ? toggleStylePicker.StyleSheet as DropdownItemToggleStyleSheet : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenToggle toggle;
        [SerializeField] private Image toggleBackground;
        [Space]
        [SerializeField] private Image checkmarkImage;
        [Space]
        [SerializeField] private TextMeshProUGUI toggleText;


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

        protected override void LinkEvents()
        {
            OnValueChanged += ValueChanged;
            OnClick += Click;
            OnMouseEnter += MouseEnter;
            OnMouseExit += MouseExit;
        }
        protected override void UnlinkEvents()
        {
            OnValueChanged -= ValueChanged;
            OnClick -= Click;
            OnMouseEnter -= MouseEnter;
            OnMouseExit -= MouseExit;
        }

        private void ValueChanged(bool state)
        {
            isOn = state;
            ActuState();
            onValueChanged?.Invoke(state);
        }
        private void Click()
        {
            onClick?.Invoke();
        }
        private void MouseEnter()
        {
            onMouseEnter?.Invoke();
        }
        private void MouseExit()
        {
            onMouseExit?.Invoke();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            State = isOn;

            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            toggleStylePicker.SetUp(styleSheetContainer, StyleSheetType.DROPDOWN_ITEM_TOGGLE, "Toggle Type");

            if (CurrentStyleSheet == null) return;

            // Background
            if (toggleBackground != null)
            {
                toggleBackground.enabled = CurrentStyleSheet.backgroundActive;
                toggleBackground.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }

            // Checkmark Icon
            if (checkmarkImage != null)
            {
                checkmarkImage.SetUpImage(CurrentStyleSheet.CheckmarkStyleSheet);
            }

            // Text
            if (toggleText != null)
            {
                toggleText.text = Text;
                toggleText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }

            ActuState();
        }

        protected override void SetUpGraphics()
        {
            toggle.GetGraphics(toggleBackground, CurrentStyleSheet.BackgroundStyleSheet,
                checkmarkImage, CurrentStyleSheet.CheckmarkStyleSheet,
                toggleText, CurrentStyleSheet.TextStyleSheet);
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
