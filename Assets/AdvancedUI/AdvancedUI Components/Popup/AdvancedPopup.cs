using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    

    public class AdvancedPopup : AdvancedComponent
    {
        #region Popup Content

        [System.Serializable]
        public class PopupContent
        {
            // ### Constructor ###
            public PopupContent(string text, int _fontSize = 25, int width = 200, string confirmation = "Yes", string cancel = "No", int _buttonsHeight = 50)
            {
                popupText = text;
                fontSize = _fontSize;
                popupWidth = width;
                confirmationText = confirmation;
                cancelText = cancel;
                buttonsHeight = _buttonsHeight;
            }


            // ### Properties ###
            [TextArea] public string popupText;
            [SerializeField] private int fontSize;
            public int FontSize { get { return fontSize > 0 ? fontSize : 25; } set => fontSize = value; }
            [SerializeField] private int popupWidth;
            public int PopupWidth { get { return popupWidth > 0 ? popupWidth : 200; } set => popupWidth = value; }
            [Space]
            [SerializeField] private string confirmationText;
            public string ConfirmationText { get { return !string.IsNullOrWhiteSpace(confirmationText) ? confirmationText : "Yes"; } set => confirmationText = value; }
            [SerializeField] private string cancelText;
            public string CancelText { get { return !string.IsNullOrWhiteSpace(cancelText) ? cancelText : "No"; } set => cancelText = value; }
            [SerializeField] private int buttonsHeight;
            public int ButtonsHeight { get { return buttonsHeight > 0 ? buttonsHeight : 50; } set => buttonsHeight = value; }
        }

        #endregion

        [Header("Popup Type")]
        [SerializeField] private StylePicker popupStylePicker;
        public StylePicker Style { get => popupStylePicker; set { popupStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("Content")]
        [SerializeField] private PopupContent popupContent;
        public PopupContent Content { get { return popupContent; } set { popupContent = value; SetUpConfig(); } }

        public override bool Interactable { get => gameObject.activeSelf; set => gameObject.SetActive(value); }


        [Header("Events")]
        [SerializeField] private UnityEvent onConfirm;
        [SerializeField] private UnityEvent onCancel;

        public event Action OnConfirm { add { confirmButton.OnClick += value; } remove { confirmButton.OnClick -= value; } }
        public event Action OnCancel { add { cancelButton.OnClick += value; } remove { cancelButton.OnClick -= value; } }


        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private PopupStyleSheet customStyleSheet;

        private PopupStyleSheet CurrentStyleSheet 
        { get { return custom ? customStyleSheet : styleSheetContainer ? popupStylePicker.StyleSheet as PopupStyleSheet : null; } }

        [Header("UI Components")]
        [SerializeField] private Image popupImage;
        [SerializeField] private Image filterImage;
        [Space]
        [SerializeField] private TextMeshProUGUI popupText;
        [Space]
        [SerializeField] private GameObject buttonsContainer;
        [SerializeField] private AdvancedButton confirmButton;
        [SerializeField] private AdvancedButton cancelButton;
        [SerializeField] private AdvancedButton quitButton;

        #region Events
        // ### Events ###

        protected override void LinkEvents()
        {
            if (onConfirm.GetPersistentEventCount() > 0)
                confirmButton.OnClick += Confirm;
            cancelButton.OnClick += Cancel;
            quitButton.OnClick += Cancel;
        }
        protected override void UnlinkEvents()
        {
            if (onConfirm.GetPersistentEventCount() > 0)
                confirmButton.OnClick -= Confirm;
            cancelButton.OnClick -= Cancel;
            quitButton.OnClick -= Cancel;
        }

        private void Confirm()
        {
            onConfirm?.Invoke();
        }
        private void Cancel()
        {
            onCancel?.Invoke();
            ClosePopup();
        }

        private void ClosePopup() { gameObject.SetActive(false); }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            popupStylePicker.SetUp(styleSheetContainer, StyleSheetType.POPUP, "Popup Style");

            if (CurrentStyleSheet == null) return;

            // Background
            if (popupImage != null)
            {
                popupImage.sprite = CurrentStyleSheet.PopupBackgroundStyleSheet.baseSprite;
                popupImage.color = CurrentStyleSheet.PopupBackgroundStyleSheet.baseColor;
                popupImage.material = CurrentStyleSheet.PopupBackgroundStyleSheet.baseMaterial;
                popupImage.type = CurrentStyleSheet.PopupBackgroundStyleSheet.imageType;
                popupImage.pixelsPerUnitMultiplier = CurrentStyleSheet.PopupBackgroundStyleSheet.pixelsPerUnit;
            }

            // Filter
            if (filterImage)
            {
                filterImage.enabled = CurrentStyleSheet.filterActive;
                filterImage.color = CurrentStyleSheet.filterColor;
            }

            // Text
            if (popupText != null)
            {
                popupText.enabled = CurrentStyleSheet.textActive;
                popupText.text = popupContent.popupText;
                popupText.fontSize = popupContent.FontSize;
                popupText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }

            // Buttons
            if (buttonsContainer)
            {
                buttonsContainer.SetActive(CurrentStyleSheet.confirmButtonActive && CurrentStyleSheet.cancelButtonActive);
                (buttonsContainer.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.ButtonsHeight);
            }
            if (confirmButton)
            {
                confirmButton.SetContainer(styleSheetContainer);
                confirmButton.gameObject.SetActive(CurrentStyleSheet.confirmButtonActive);
                confirmButton.Style = CurrentStyleSheet.ConfirmationButtonStyle;
                confirmButton.Content.text = Content.ConfirmationText;
            }
            if (cancelButton)
            {
                confirmButton.SetContainer(styleSheetContainer);
                cancelButton.gameObject.SetActive(CurrentStyleSheet.cancelButtonActive);
                cancelButton.Style = CurrentStyleSheet.CancelButtonStyle;
                cancelButton.Content.text = Content.CancelText;
            }
            if (quitButton)
            {
                confirmButton.SetContainer(styleSheetContainer);
                quitButton.gameObject.SetActive(CurrentStyleSheet.quitButtonActive);
                quitButton.Style = CurrentStyleSheet.QuitButtonStyle;
            }

            if (popupImage)
            {
                (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Content.PopupWidth);
                LayoutRebuilder.ForceRebuildLayoutImmediate(popupImage.rectTransform);
            }
        }

        protected override void SetUpGraphics() { }
        #endregion
    }
}
