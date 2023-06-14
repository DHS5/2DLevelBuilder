using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    

    public class AdvancedButton : AdvancedComponent
    {
        #region Button Content

        [System.Serializable]
        public class ButtonContent
        {
            public ButtonContent(string _text = "")
            {
                text = _text;
            }

            // ### Properties ###
            public string text;
        }

        #endregion

        [Header("Button Type")]
        [SerializeField] private StylePicker buttonStylePicker;
        public StylePicker Style { get => buttonStylePicker; set { buttonStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("Content")]
        [SerializeField] private ButtonContent buttonContent;
        public ButtonContent Content { get { return buttonContent; } set { buttonContent = value; SetUpConfig(); } }

        public override bool Interactable { get => button.interactable; set => button.interactable = value; }

        [Header("Events")]
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onButtonDown;
        [SerializeField] private UnityEvent onButtonUp;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action OnClick { add { button.OnButtonClick += value; } remove { button.OnButtonClick -= value; } }
        public event Action OnButtonDown { add { button.OnButtonDown += value; } remove { button.OnButtonDown -= value; } }
        public event Action OnButtonUp { add { button.OnButtonUp += value; } remove { button.OnButtonUp -= value; } }
        public event Action OnMouseEnter { add { button.OnButtonEnter += value; } remove { button.OnButtonEnter -= value; } }
        public event Action OnMouseExit { add { button.OnButtonExit += value; } remove { button.OnButtonExit -= value; } }


        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ButtonStyleSheet customStyleSheet;

        [Header("Overrides")]
        [SerializeField] private bool overrideIcon;
        [SerializeField] private ImageOverrideSheet iconOverrideSheet;

        private ButtonStyleSheet CurrentStyleSheet 
        { get { return custom ? customStyleSheet : styleSheetContainer ? buttonStylePicker.StyleSheet as ButtonStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private OpenButton button;
        [SerializeField] private Image buttonBackground;
        [SerializeField] private Image buttonIcon;
        [SerializeField] private AspectRatioFitter iconRatioFitter;
        [SerializeField] private TextMeshProUGUI buttonText;


        #region Events

        protected override void LinkEvents()
        {
            OnButtonDown += ButtonDown;
            OnButtonUp += ButtonUp;
            OnClick += Click;
            OnMouseEnter += MouseEnter;
            OnMouseExit += MouseExit;
        }
        protected override void UnlinkEvents()
        {
            OnButtonDown -= ButtonDown;
            OnButtonUp -= ButtonUp;
            OnClick -= Click;
            OnMouseEnter -= MouseEnter;
            OnMouseExit -= MouseExit;
        }


        private void ButtonDown()
        {
            onButtonDown?.Invoke();
        }
        private void ButtonUp()
        {
            onButtonUp?.Invoke();
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
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            buttonStylePicker.SetUp(styleSheetContainer, StyleSheetType.BUTTON, "Button Style");

            if (CurrentStyleSheet == null) return;

            // Background
            if (buttonBackground != null)
            {
                buttonBackground.enabled = CurrentStyleSheet.backgroundActive;
                buttonBackground.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }

            // Icon
            if (buttonIcon != null)
            {
                buttonIcon.enabled = CurrentStyleSheet.iconActive;
                buttonIcon.transform.localScale = Vector2.one * CurrentStyleSheet.iconScale;
                if (!overrideIcon)
                {
                    buttonIcon.SetUpImage(CurrentStyleSheet.IconStyleSheet, iconRatioFitter);
                }
                else
                {
                    buttonIcon.SetUpImage(CurrentStyleSheet.IconStyleSheet, iconOverrideSheet, iconRatioFitter);
                }
            }

            // Text
            if (buttonText != null)
            {
                buttonText.enabled = CurrentStyleSheet.textActive;
                buttonText.text = Content.text;
                buttonText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }
        }

        protected override void SetUpGraphics()
        {
            button.GetGraphics(buttonBackground, CurrentStyleSheet.BackgroundStyleSheet,
                buttonIcon, CurrentStyleSheet.IconStyleSheet, overrideIcon ? iconOverrideSheet : null,
                buttonText, CurrentStyleSheet.TextStyleSheet);
        }
        #endregion
    }
}
