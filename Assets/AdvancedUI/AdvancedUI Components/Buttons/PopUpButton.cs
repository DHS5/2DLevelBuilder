using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dhs5.AdvancedUI
{
    public class PopUpButton : AdvancedComponent
    {
        [Header("Button")]
        [SerializeField] private AdvancedButtonType buttonType;
        [SerializeField] private ButtonContent buttonContent;
        public AdvancedButtonType ButtonType { get { return buttonType; } set { buttonType = value; } }
        public ButtonContent ButtonContent { get { return buttonContent; } set { buttonContent = value; } }

        [Header("Popup")]
        [SerializeField] private PopupType popupType;
        [SerializeField] private PopupContent popupContent;
        public PopupType PopupType { get { return popupType; } set { popupType = value; } }
        public PopupContent PopupContent { get { return popupContent; } set { popupContent = value; } }

        public override bool Interactable { get => button.Interactable; set => button.Interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent onConfirm;
        [SerializeField] private UnityEvent onCancel;
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onButtonDown;
        [SerializeField] private UnityEvent onButtonUp;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;

        public event Action OnConfirm { add { popup.OnConfirm += value; } remove { popup.OnConfirm -= value; } }
        public event Action OnCancel { add { popup.OnCancel += value; } remove { popup.OnCancel -= value; } }
        public event Action OnClick { add { button.OnClick += value; } remove { button.OnClick -= value; } }
        public event Action OnButtonDown { add { button.OnButtonDown += value; } remove { button.OnButtonDown -= value; } }
        public event Action OnButtonUp { add { button.OnButtonUp += value; } remove { button.OnButtonUp -= value; } }
        public event Action OnMouseEnter { add { button.OnMouseEnter += value; } remove { button.OnMouseEnter -= value; } }
        public event Action OnMouseExit { add { button.OnMouseExit += value; } remove { button.OnMouseExit -= value; } }

        [Header("UI Components")]
        [SerializeField] private AdvancedButton button;
        [SerializeField] private AdvancedPopup popup;

        #region Events

        protected override void LinkEvents()
        {
            popup.OnConfirm += Confirm;
            popup.OnCancel += Cancel;
            button.OnClick += Click;
            button.OnButtonDown += ButtonDown;
            button.OnButtonUp += ButtonUp;
            button.OnMouseEnter += MouseEnter;
            button.OnMouseExit += MouseExit;
        }
        protected override void UnlinkEvents()
        {
            popup.OnConfirm -= Confirm;
            popup.OnCancel -= Cancel;
            button.OnClick -= Click;
            button.OnButtonDown -= ButtonDown;
            button.OnButtonUp -= ButtonUp;
            button.OnMouseEnter -= MouseEnter;
            button.OnMouseExit -= MouseExit;
        }

        private void Confirm() { onConfirm?.Invoke(); }
        private void Cancel() { onCancel?.Invoke(); }
        private void Click() { onClick?.Invoke(); OpenPopup(); }
        private void ButtonDown() { onButtonDown?.Invoke(); }
        private void ButtonUp() { onButtonUp?.Invoke(); }
        private void MouseEnter() { onMouseEnter?.Invoke(); }
        private void MouseExit() { onMouseExit?.Invoke(); }


        private void OpenPopup() { popup.gameObject.SetActive(true); }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (button)
            {
                button.Type = buttonType;
                button.Content = buttonContent;
            }
            if (popup)
            {
                popup.Type = popupType;
                popup.Content = popupContent;
            }
        }

        #endregion
    }
}
