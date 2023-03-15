using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    #region InputField Content
    [System.Serializable]
    public struct InputFieldContent
    {
        // ### Properties ###
        public string hintText;
    }
    #endregion

    public class AdvancedInputField : AdvancedComponent
    {
        [Header("InputField Type")]
        [SerializeField] private AdvancedInputfieldType sliderType;
        public AdvancedInputfieldType Type { get { return sliderType; } set { sliderType = value; SetUpConfig(); } }

        [Header("InputField Content")]
        [SerializeField] private InputFieldContent inputfieldContent;
        public InputFieldContent Content { get { return inputfieldContent; } set { inputfieldContent = value; SetUpConfig(); } }
        public string Text { get { return inputField.text; } set { inputField.text = value; } }

        public override bool Interactable { get => inputField.interactable; set => inputField.interactable = value; }


        [Header("Custom Style Sheet")]
        [SerializeField] private InputfieldStyleSheet customStyleSheet;

        private InputfieldStyleSheet CurrentStyleSheet
        { get { return Type == AdvancedInputfieldType.CUSTOM ? customStyleSheet :
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.inputfieldStyleSheets.GetStyleSheet(Type) : null; } }


        public event Action<string> OnValueChanged;
        public event Action<string> OnSubmit;
        public event Action<string> OnEndEdit;


        [Header("UI Components")]
        [SerializeField] private OpenInputField inputField;
        [SerializeField] private Image backgroundImage;
        [Space]
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private TextMeshProUGUI inputText;
        [Space]
        [SerializeField] private AdvancedScrollbar scrollbar;

        override protected void Awake()
        {
            inputField.GetGraphics(backgroundImage, CurrentStyleSheet.backgroundStyleSheet,
                hintText, GetTextStyleSheet(CurrentStyleSheet.hintTextType),
                inputText, GetTextStyleSheet(CurrentStyleSheet.inputTextType));

            base.Awake();
        }

        #region Events
        protected override void LinkEvents() 
        {
            inputField.onValueChanged.AddListener(ValueChanged);
            inputField.onSubmit.AddListener(Submit);
            inputField.onEndEdit.AddListener(EndEdit);
        }
        protected override void UnlinkEvents() 
        {
            inputField.onValueChanged?.RemoveListener(ValueChanged);
            inputField.onSubmit?.RemoveListener(Submit);
            inputField.onEndEdit?.RemoveListener(EndEdit);
        }

        private void ValueChanged(string s)
        {
            OnValueChanged?.Invoke(s);
        }
        private void Submit(string s)
        {
            OnSubmit?.Invoke(s);
        }
        private void EndEdit(string s)
        {
            OnEndEdit?.Invoke(s);
        }
        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (CurrentStyleSheet == null) return;

            // Input Text
            if (inputText)
            {
                inputText.SetUpText(GetTextStyleSheet(CurrentStyleSheet.inputTextType));
            }

            // Input Field
            if (inputField)
            {
                inputField.selectionColor = CurrentStyleSheet.selectionColor;
                inputField.caretColor = inputText != null ? inputText.color : Color.black;
            }

            // Background
            if (backgroundImage)
            {
                backgroundImage.enabled = CurrentStyleSheet.backgroundActive;
                backgroundImage.SetUpImage(CurrentStyleSheet.backgroundStyleSheet);
            }

            // Hint Text
            if (hintText)
            {
                hintText.enabled = CurrentStyleSheet.hintTextActive;
                hintText.text = Content.hintText;
                hintText.SetUpText(GetTextStyleSheet(CurrentStyleSheet.hintTextType));
            }
        }

        #endregion
    }
}
