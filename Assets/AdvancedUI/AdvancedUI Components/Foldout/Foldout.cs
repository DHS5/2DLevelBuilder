using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Dhs5.AdvancedUI
{
    public class Foldout : AdvancedComponent
    {
        #region Foldout Content
        [Serializable]
        public class FoldoutContent
        {
            public FoldoutContent(int bHeight)
            {
                buttonHeight = bHeight;

                padding = new(5, 5, 5, 5);
                spacing = 5;
                gridSpacing = new Vector2(5, 5);
                cellSize = new Vector2(100, 100);
                childAlignment = TextAnchor.UpperCenter;
                constraint = GridLayoutGroup.Constraint.Flexible;
                constraintCount = 2;
            }

            // ### Properties ###
            [Header("Button")]
            public int buttonHeight;

            [Header("Layout")]
            public RectOffset padding;
            public TextAnchor childAlignment;

            [Header("Vertical/Horizontal")]
            public float spacing;

            [Header("Grid")]
            public Vector2 cellSize;
            public Vector2 gridSpacing;
            public GridLayoutGroup.Constraint constraint;
            public int constraintCount;
        }
        #endregion

        [Header("Foldout")]
        [SerializeField] private FoldoutContent foldoutContent;
        public FoldoutContent Content { get =>  foldoutContent; set { foldoutContent = value; SetUpConfig(); } }

        [SerializeField] private bool open;
        public bool IsOpen { get => open; set { open = value; SetFoldoutState(value); } }

        [Header("Button")]
        [SerializeField] private StylePicker buttonStylePicker;
        [SerializeField] private AdvancedButton.ButtonContent buttonContent;
        public AdvancedButton.ButtonContent ButtonContent { get { return buttonContent; } set { buttonContent = value; } }

        [Header("Background")]
        [SerializeField] private StylePicker backgroundStylePicker;

        public override bool Interactable { get => button.Interactable; set => button.Interactable = value; }

        [Header("Events")]
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private UnityEvent onClose;

        public event Action OnClick { add { button.OnClick += value; } remove { button.OnClick -= value; } }
        public event Action OnOpen;
        public event Action OnClose;

        [Header("UI Components")]
        [SerializeField] private AdvancedButton button;
        [SerializeField] private RectTransform buttonRect;
        [Space]
        [SerializeField] private AdvancedImage background;
        [SerializeField] private UIMask mask;
        [Space]
        [SerializeField] private LayoutGroup contentLayout;


        #region Events

        protected override void LinkEvents()
        {
            button.OnClick += Click;
        }
        protected override void UnlinkEvents()
        {
            button.OnClick -= Click;
        }

        private void Click() 
        { 
            onClick?.Invoke();
            ChangeFoldoutState();
        }
        private void Open()
        {
            OnOpen?.Invoke();
            onOpen?.Invoke();
        }
        private void Close()
        {
            OnClose?.Invoke();
            onClose?.Invoke();
        }


        private void ChangeFoldoutState() 
        { 
            IsOpen = !IsOpen;
            SetFoldoutState(IsOpen);
        }
        private void SetFoldoutState(bool state) 
        { 
            background.gameObject.SetActive(state);

            if (state) Open();
            else Close();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            buttonStylePicker.SetUp(styleSheetContainer, StyleSheetType.BUTTON, "Button Style");
            backgroundStylePicker.SetUp(styleSheetContainer, StyleSheetType.BACKGROUND_IMAGE, "Background Style");

            if (button)
            {
                button.SetContainer(styleSheetContainer);
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.buttonHeight);
                button.Style = buttonStylePicker;
                button.Content = buttonContent;
            }
            if (background)
            {
                background.SetContainer(styleSheetContainer);
                background.gameObject.SetActive(IsOpen);
                background.Style = backgroundStylePicker;
            }
            if (mask)
            {
                mask.SetContainer(styleSheetContainer);
                mask.Style = backgroundStylePicker;
            }
            
            SetUpLayout();
        }

        protected override void SetUpGraphics() { }

        private void SetUpLayout()
        {
            if (!contentLayout) return;

            contentLayout.padding = Content.padding;
            contentLayout.childAlignment = Content.childAlignment;
            
            if (contentLayout is HorizontalOrVerticalLayoutGroup simple)
            {
                simple.spacing = Content.spacing;
            }
            else if (contentLayout is GridLayoutGroup grid)
            {
                grid.spacing = Content.gridSpacing;
                grid.cellSize = Content.cellSize;
                grid.constraint = Content.constraint;
                grid.constraintCount = Content.constraintCount;
            }
        }

        #endregion
    }
}
