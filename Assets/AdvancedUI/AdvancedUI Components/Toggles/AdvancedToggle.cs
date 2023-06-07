using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    #region Toggle Content
    [Serializable]
    public struct ToggleContent
    {
        // ### Constructor ###
        public ToggleContent(Sprite background, Sprite checkmark, float checkScale = 1, string checkText = "Active", 
            Sprite uncheckmark = null, float uncheckScale = 1, string uncheckText = "Inactive", string text = "", int size = 25)
        {
            toggleBackground = background;
            checkmarkIcon = checkmark;
            checkmarkScale = checkScale;
            checkmarkText = checkText;
            uncheckmarkIcon = uncheckmark;
            uncheckmarkScale = uncheckScale;
            uncheckmarkText = uncheckText;
            toggleText = text;
            fontSize = size;
        }

        // ### Properties ###
        [Header("Background")]
        public Sprite toggleBackground;
        [Header("Checkmark")]
        public Sprite checkmarkIcon;
        [SerializeField] private float checkmarkScale; public float CheckmarkScale
        { get { return checkmarkScale >  0 ? checkmarkScale : 1; } set { checkmarkScale = value; } }
        [SerializeField] private string checkmarkText; public string CheckmarkText 
        { get { return !string.IsNullOrWhiteSpace(checkmarkText) ? checkmarkText : "Active"; } set { checkmarkText = value; } }
        [Header("Uncheckmark")]
        public Sprite uncheckmarkIcon;
        [SerializeField] private float uncheckmarkScale; public float UncheckmarkScale
        { get { return uncheckmarkScale > 0 ? uncheckmarkScale : 1; } set { uncheckmarkScale = value; } }
        [SerializeField] private string uncheckmarkText; public string UncheckmarkText
        { get { return !string.IsNullOrWhiteSpace(uncheckmarkText) ? uncheckmarkText : "Inactive"; } set { uncheckmarkText = value; } }
        [Header("Text")]
        public string toggleText;
        [SerializeField] private int fontSize; public int FontSize 
        { get { return fontSize > 0 ? fontSize : 25; } set { fontSize = value; } }
    }
    #endregion

    public class AdvancedToggle : AdvancedComponent
    {
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
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action<bool> OnValueChanged { add { toggle.OnValueChanged += value; } remove { toggle.OnValueChanged -= value; } }
        public event Action OnClick { add { toggle.OnToggleClick += value; } remove { toggle.OnToggleClick -= value; } }
        public event Action OnMouseEnter { add { toggle.OnToggleEnter += value; } remove { toggle.OnToggleEnter -= value; } }
        public event Action OnMouseExit { add { toggle.OnToggleExit += value; } remove { toggle.OnToggleExit -= value; } }


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
            toggleStylePicker.SetUp(styleSheetContainer, StyleSheetType.TOGGLE, "Toggle type");

            if (CurrentStyleSheet == null) return;

            // Background
            if (toggleBackground != null)
            {
                toggleBackground.enabled = CurrentStyleSheet.backgroundActive;
                toggleBackground.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
                toggleBackground.sprite = Content.toggleBackground != null ? 
                    Content.toggleBackground : CurrentStyleSheet.BackgroundStyleSheet.baseSprite;
            }

            // Checkmark Icon
            if (checkmarkImage != null)
            {
                if (Content.checkmarkIcon == null) toggleContent.CheckmarkScale = CurrentStyleSheet.checkmarkScale;
                checkmarkImage.enabled = CurrentStyleSheet.checkmarkActive && CurrentStyleSheet.checkmarkIsImage;
                checkmarkImage.transform.localScale = new Vector2(Content.CheckmarkScale, Content.CheckmarkScale);
                checkmarkImage.SetUpImage(CurrentStyleSheet.CheckmarkImageStyleSheet);
                checkmarkImage.sprite = Content.checkmarkIcon != null ? 
                    Content.checkmarkIcon : CurrentStyleSheet.CheckmarkImageStyleSheet.baseSprite;
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
                if (Content.uncheckmarkIcon == null) toggleContent.UncheckmarkScale = CurrentStyleSheet.uncheckmarkScale;
                uncheckmarkImage.enabled = CurrentStyleSheet.uncheckmarkActive && CurrentStyleSheet.uncheckmarkIsImage;
                uncheckmarkImage.transform.localScale = new Vector2(Content.UncheckmarkScale, Content.UncheckmarkScale);
                uncheckmarkImage.SetUpImage(CurrentStyleSheet.UncheckmarkImageStyleSheet);
                uncheckmarkImage.sprite = Content.uncheckmarkIcon != null ? 
                    Content.uncheckmarkIcon : CurrentStyleSheet.UncheckmarkImageStyleSheet.baseSprite;
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
                CurrentStyleSheet.checkmarkIsImage ? checkmarkImage : null, CurrentStyleSheet.CheckmarkImageStyleSheet,
                CurrentStyleSheet.checkmarkIsImage ? null : checkmarkText, CurrentStyleSheet.CheckmarkTextStyleSheet,
                CurrentStyleSheet.uncheckmarkIsImage ? uncheckmarkImage : null, CurrentStyleSheet.UncheckmarkImageStyleSheet,
                CurrentStyleSheet.uncheckmarkIsImage ? null : uncheckmarkText, CurrentStyleSheet.UncheckmarkTextStyleSheet,
                toggleText, CurrentStyleSheet.TextStyleSheet);
        }

        #endregion
    }
}
