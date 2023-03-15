using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    #region Button Content

    [System.Serializable]
    public struct ButtonContent
    {
        public ButtonContent(string _text = "", Sprite backround = null, Sprite icon = null, float scale = 1)
        {
            text = _text;
            backgroundSprite = backround;
            iconSprite = icon;
            iconScale = scale;
        }

        // ### Properties ###
        public Sprite backgroundSprite;
        public Sprite iconSprite;
        [SerializeField] private float iconScale; public float IconScale { get { return iconScale > 0 ? iconScale : 1; } set { iconScale = value; } }
        [Space]
        public string text;
    }

    #endregion

    public class AdvancedButton : AdvancedComponent
    {
        [Header("Button Type")]
        [SerializeField] private AdvancedButtonType buttonType;
        public AdvancedButtonType Type { get { return buttonType; } set { buttonType = value; SetUpConfig(); } }

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
        [SerializeField] private ButtonStyleSheet customStyleSheet;

        private ButtonStyleSheet CurrentStyleSheet { get { return buttonType == AdvancedButtonType.CUSTOM ? customStyleSheet : 
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.buttonStyleSheets.GetStyleSheet(buttonType) : null; } }

        [Header("UI Components")]
        [SerializeField] private OpenButton button;
        [SerializeField] private Image buttonBackground;
        [SerializeField] private Image buttonIcon;
        [SerializeField] private TextMeshProUGUI buttonText;

        protected override void Awake()
        {
            button.GetGraphics(buttonBackground, CurrentStyleSheet.backgroundStyleSheet,
                buttonIcon, CurrentStyleSheet.iconStyleSheet,
                buttonText, GetTextStyleSheet(CurrentStyleSheet.textType));

            base.Awake();
        }

        #region Events
        // ### Events ###

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
        // ### Configs ###

        protected override void SetUpConfig()
        {
            if (CurrentStyleSheet == null) return;

            // Background
            if (buttonBackground != null)
            {
                buttonBackground.enabled = CurrentStyleSheet.backgroundActive;
                buttonBackground.sprite = Content.backgroundSprite != null ? Content.backgroundSprite : CurrentStyleSheet.backgroundStyleSheet.baseSprite;
                buttonBackground.color = CurrentStyleSheet.backgroundStyleSheet.baseColor;
                buttonBackground.material = CurrentStyleSheet.backgroundStyleSheet.baseMaterial;
                buttonBackground.type = CurrentStyleSheet.backgroundStyleSheet.imageType;
                buttonBackground.pixelsPerUnitMultiplier = CurrentStyleSheet.backgroundStyleSheet.pixelsPerUnit;
            }

            // Icon
            if (buttonIcon != null)
            {
                if (Content.iconSprite == null) buttonContent.IconScale = CurrentStyleSheet.iconScale;
                buttonIcon.enabled = CurrentStyleSheet.iconActive;
                buttonIcon.transform.localScale = new Vector2(Content.IconScale, Content.IconScale);
                buttonIcon.sprite = Content.iconSprite != null ? Content.iconSprite : CurrentStyleSheet.iconStyleSheet.baseSprite;
                buttonIcon.color = CurrentStyleSheet.iconStyleSheet.baseColor;
                buttonIcon.material = CurrentStyleSheet.iconStyleSheet.baseMaterial;
                buttonIcon.type = CurrentStyleSheet.iconStyleSheet.imageType;
                buttonIcon.pixelsPerUnitMultiplier = CurrentStyleSheet.iconStyleSheet.pixelsPerUnit;
            }

            // Text
            if (buttonText != null)
            {
                buttonText.enabled = CurrentStyleSheet.textActive;
                buttonText.text = Content.text;
                buttonText.SetUpText(GetTextStyleSheet(CurrentStyleSheet.textType));
            }
        }
        #endregion
    }
}
