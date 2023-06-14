using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    public class AdvancedToggle : AdvancedComponent
    {
        #region Toggle Content
        [Serializable]
        public class ToggleContent
        {
            // ### Constructor ###
            public ToggleContent(string checkText = "Active", string uncheckText = "Inactive", string text = "", int size = 25)
            {
                checkmarkText = checkText;
                uncheckmarkText = uncheckText;

                toggleText = text;
                fontSize = size;
            }

            // ### Properties ###
            [Header("Checkmark")]
            [SerializeField] private string checkmarkText;
            public string CheckmarkText
            { get { return !string.IsNullOrWhiteSpace(checkmarkText) ? checkmarkText : "Active"; } set { checkmarkText = value; } }

            [Header("Uncheckmark")]
            [SerializeField] private string uncheckmarkText;
            public string UncheckmarkText
            { get { return !string.IsNullOrWhiteSpace(uncheckmarkText) ? uncheckmarkText : "Inactive"; } set { uncheckmarkText = value; } }

            [Header("Text")]
            public string toggleText;
            [SerializeField] private int fontSize; public int FontSize
            { get { return fontSize > 0 ? fontSize : 25; } set { fontSize = value; } }
        }
        #endregion

        [Header("Toggle Type")]
        [SerializeField] private StylePicker toggleStylePicker;
        public StylePicker Style { get => toggleStylePicker; set { toggleStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("Toggle Content")]
        [SerializeField] private bool isOn = true;
        [SerializeField] private ToggleContent toggleContent;
        public ToggleContent Content { get { return toggleContent; } set { toggleContent = value; SetUpConfig(); } }

        public override bool Interactable { get => toggle.interactable; set => toggle.interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent<bool> onValueChanged;
        [SerializeField] private UnityEvent onTrue;
        [SerializeField] private UnityEvent onFalse;
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action<bool> OnValueChanged { add { toggle.OnValueChanged += value; } remove { toggle.OnValueChanged -= value; } }
        public event Action OnTrue;
        public event Action OnFalse;
        public event Action OnClick { add { toggle.OnToggleClick += value; } remove { toggle.OnToggleClick -= value; } }
        public event Action OnMouseEnter { add { toggle.OnToggleEnter += value; } remove { toggle.OnToggleEnter -= value; } }
        public event Action OnMouseExit { add { toggle.OnToggleExit += value; } remove { toggle.OnToggleExit -= value; } }

        [Header("Overrides")]
        [SerializeField] private bool overrideCheckmark;
        [SerializeField] private ImageOverrideSheet checkmarkOverrideSheet;
        [SerializeField] private bool overrideUncheckmark;
        [SerializeField] private ImageOverrideSheet uncheckmarkOverrideSheet;

        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ToggleStyleSheet customStyleSheet;

        private ToggleStyleSheet CurrentStyleSheet 
        { get { return custom ? customStyleSheet : styleSheetContainer ? Style.StyleSheet as ToggleStyleSheet : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenToggle toggle;
        [SerializeField] private Image toggleBackground;
        [Space]
        [SerializeField] private Image checkmarkImage;
        [SerializeField] private TextMeshProUGUI checkmarkText;
        [SerializeField] private AspectRatioFitter checkmarkRatioFitter;
        [Space]
        [SerializeField] private Image uncheckmarkImage;
        [SerializeField] private TextMeshProUGUI uncheckmarkText;
        [SerializeField] private AspectRatioFitter uncheckmarkRatioFitter;
        [Space]
        [SerializeField] private TextMeshProUGUI toggleText;


        private Graphic CurrentCheckmark 
        { get { return CurrentStyleSheet.checkmarkIsImage ? checkmarkImage : checkmarkText; } }
        private Graphic CurrentUncheckmark 
        { get { return CurrentStyleSheet.uncheckmarkIsImage ? uncheckmarkImage : uncheckmarkText; } }



        #region Public Accessors & Methods

        public bool State { get { return toggle.isOn; } set { toggle.isOn = value; } }
        public ToggleGroup Group { get => toggle.group; set => toggle.group = value; }

        public void ActuState()
        {
            if (CurrentCheckmark) CurrentCheckmark.enabled = State;
            if (CurrentUncheckmark) CurrentUncheckmark.enabled = !State;

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
            ActuBackground();
            onValueChanged?.Invoke(state);
            True();
            False();
        }
        private void True()
        {
            if (!State) return;

            onTrue?.Invoke();
            OnTrue?.Invoke();
        }
        private void False()
        {
            if (State) return;

            onFalse?.Invoke();
            OnFalse?.Invoke();
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
            toggleStylePicker.SetUp(styleSheetContainer, StyleSheetType.TOGGLE, "Toggle type");

            if (CurrentStyleSheet == null) return;

            // Background
            if (toggleBackground != null)
            {
                toggleBackground.enabled = CurrentStyleSheet.backgroundActive;

                if (!CurrentStyleSheet.trueBackground || !State)
                {
                    toggleBackground.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
                }
                else
                {
                    toggleBackground.SetUpImage(CurrentStyleSheet.TrueBackgroundStyleSheet);
                }
            }

            // Checkmark Icon
            if (checkmarkImage != null)
            {
                checkmarkImage.enabled = CurrentStyleSheet.checkmarkActive && CurrentStyleSheet.checkmarkIsImage;
                checkmarkImage.transform.localScale = Vector2.one * CurrentStyleSheet.checkmarkScale;

                if (!overrideCheckmark)
                {
                    checkmarkImage.SetUpImage(CurrentStyleSheet.CheckmarkImageStyleSheet, checkmarkRatioFitter);
                }
                else
                {
                    checkmarkImage.SetUpImage(CurrentStyleSheet.CheckmarkImageStyleSheet, checkmarkOverrideSheet, checkmarkRatioFitter);
                }
            }
            // Checkmark Text
            if (checkmarkText != null)
            {
                checkmarkText.enabled = CurrentStyleSheet.checkmarkActive && !CurrentStyleSheet.checkmarkIsImage;
                checkmarkText.text = Content.CheckmarkText;
                checkmarkText.SetUpText(CurrentStyleSheet.CheckmarkTextStyleSheet);
            }

            // Uncheckmark Icon
            if (uncheckmarkImage != null)
            {
                uncheckmarkImage.enabled = CurrentStyleSheet.uncheckmarkActive && CurrentStyleSheet.uncheckmarkIsImage;
                uncheckmarkImage.transform.localScale = Vector2.one * CurrentStyleSheet.uncheckmarkScale;

                if (!overrideCheckmark)
                {
                    uncheckmarkImage.SetUpImage(CurrentStyleSheet.UncheckmarkImageStyleSheet, uncheckmarkRatioFitter);
                }
                else
                {
                    uncheckmarkImage.SetUpImage(CurrentStyleSheet.UncheckmarkImageStyleSheet, uncheckmarkOverrideSheet, uncheckmarkRatioFitter);
                }
            }
            // Uncheckmark Text
            if (uncheckmarkText != null)
            {
                uncheckmarkText.enabled = CurrentStyleSheet.uncheckmarkActive && !CurrentStyleSheet.uncheckmarkIsImage;
                uncheckmarkText.text = Content.UncheckmarkText;
                uncheckmarkText.SetUpText(CurrentStyleSheet.UncheckmarkTextStyleSheet);
            }

            // Text
            if (toggleText != null)
            {
                toggleText.enabled = CurrentStyleSheet.textActive;
                toggleText.text = Content.toggleText;
                toggleText.fontSize = Content.FontSize;
                toggleText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }

            ActuState();
        }

        protected override void SetUpGraphics()
        {
            toggle.GetGraphics(toggleBackground, CurrentStyleSheet.BackgroundStyleSheet, 
                CurrentStyleSheet.trueBackground ? CurrentStyleSheet.TrueBackgroundStyleSheet : null,
                CurrentStyleSheet.checkmarkIsImage ? checkmarkImage : null, CurrentStyleSheet.CheckmarkImageStyleSheet,
                CurrentStyleSheet.checkmarkIsImage ? null : checkmarkText, CurrentStyleSheet.CheckmarkTextStyleSheet,
                CurrentStyleSheet.uncheckmarkIsImage ? uncheckmarkImage : null, CurrentStyleSheet.UncheckmarkImageStyleSheet,
                CurrentStyleSheet.uncheckmarkIsImage ? null : uncheckmarkText, CurrentStyleSheet.UncheckmarkTextStyleSheet,
                toggleText, CurrentStyleSheet.TextStyleSheet);
        }

        private void ActuBackground()
        {
            if (toggleBackground == null || !CurrentStyleSheet.trueBackground) return;

            if (!State)
            {
                toggleBackground.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }
            else
            {
                toggleBackground.SetUpImage(CurrentStyleSheet.TrueBackgroundStyleSheet);
            }
        }

        #endregion
    }
}
