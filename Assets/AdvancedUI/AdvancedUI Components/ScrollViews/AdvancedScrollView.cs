using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dhs5.AdvancedUI
{
    public class AdvancedScrollView : AdvancedComponent
    {
        #region ScrollView Content

        [System.Serializable]
        public class ScrollViewContent
        {
            [System.Serializable]
            public enum ScrollViewDirection { VERTICAL, HORIZONTAL, BOTH };

            public ScrollViewContent(ScrollViewDirection _direction, float _height = 0, float _width = 0)
            {
                direction = _direction;
                contentHeight = _height;
                contentWidth = _width;
            }

            // ### Properties ###
            public ScrollViewDirection direction;
            [Space]
            [SerializeField] private float contentWidth;
            public float ContentWidth { get { return contentWidth > 0 ? contentWidth : 100; } set { contentWidth = value; } }
            [SerializeField] private float contentHeight;
            public float ContentHeight { get { return contentHeight > 0 ? contentHeight : 100; } set { contentHeight = value; } }
        }

        #endregion

        [Header("ScrollView Type")]
        [SerializeField] private StylePicker scrollviewStylePicker;
        public StylePicker Style { get => scrollviewStylePicker; set { scrollviewStylePicker.ForceSet(value); SetUpConfig(); } }

        [Header("ScrollView Content")]
        [SerializeField] private ScrollViewContent scrollViewContent;
        public ScrollViewContent Content { get { return scrollViewContent; } set { scrollViewContent = value; SetUpConfig(); } }

        public override bool Interactable { get => scrollView.enabled; set => scrollView.enabled = value; }


        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ScrollViewStyleSheet customStyleSheet;

        private ScrollViewStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? scrollviewStylePicker.StyleSheet as ScrollViewStyleSheet : null; } }


        [Header("UI Components")]
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private SelectableGraphic backgroundImage;
        [Space]
        [SerializeField] private RectTransform viewportRect;
        [SerializeField] private Image viewportMask;
        [Space]
        [SerializeField] private RectTransform contentRect;
        [Space]
        [SerializeField] private AdvancedScrollbar verticalScrollbar;
        [SerializeField] private AdvancedScrollbar horizontalScrollbar;


        #region Events
        public event Action<Vector2> OnValueChanged;
        public event Action OnMouseEnter;
        public event Action OnMouseExit;

        protected override void LinkEvents()
        {
            scrollView.onValueChanged.AddListener(ValueChanged);
            backgroundImage.OnMouseEnter += MouseEnter;
            backgroundImage.OnMouseExit += MouseExit;
        }
        protected override void UnlinkEvents()
        {
            scrollView.onValueChanged.RemoveListener(ValueChanged);
            backgroundImage.OnMouseEnter -= MouseEnter;
            backgroundImage.OnMouseExit -= MouseExit;
        }

        private void ValueChanged(Vector2 newValue)
        {
            OnValueChanged?.Invoke(newValue);
        }
        private void MouseEnter()
        {
            OnMouseEnter?.Invoke();
        }
        private void MouseExit()
        {
            OnMouseExit?.Invoke();
        }
        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            scrollviewStylePicker.SetUp(styleSheetContainer, StyleSheetType.SCROLL_VIEW, "Scrollview Type");

            if (CurrentStyleSheet == null) return;

            // Background
            if (backgroundImage && backgroundImage.targetGraphic is Image image)
            {
                image.enabled = CurrentStyleSheet.backgroundActive;
                image.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }

            // Viewport Mask
            if (viewportMask)
            {
                viewportMask.SetUpImage(CurrentStyleSheet.ViewportMaskStyleSheet);
            }

            // Content
            if (contentRect)
            {
                if (Content.direction == ScrollViewContent.ScrollViewDirection.VERTICAL)
                {
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.ContentHeight);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewportRect.rect.width);
                    if (scrollView)
                    {
                        scrollView.vertical = true;
                        scrollView.horizontal = false;
                    }
                }
                else if (Content.direction == ScrollViewContent.ScrollViewDirection.HORIZONTAL)
                {
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Content.ContentWidth);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, viewportRect.rect.height);
                    if (scrollView)
                    {
                        scrollView.vertical = false;
                        scrollView.horizontal = true;
                    }
                }
                else
                {
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.ContentHeight);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Content.ContentWidth);
                    if (scrollView)
                    {
                        scrollView.vertical = true;
                        scrollView.horizontal = true;
                    }
                }
            }

            // Scrollbars
            if (verticalScrollbar)
            {
                verticalScrollbar.SetContainer(styleSheetContainer);
                verticalScrollbar.gameObject.SetActive(CurrentStyleSheet.verticalScrollbarActive);
                verticalScrollbar.Style = CurrentStyleSheet.VerticalScrollbarStyle;
            }
            if (horizontalScrollbar)
            {
                horizontalScrollbar.SetContainer(styleSheetContainer);
                horizontalScrollbar.gameObject.SetActive(CurrentStyleSheet.horizontalScrollbarActive);
                horizontalScrollbar.Style = CurrentStyleSheet.HorizontalScrollbarStyle;
            }
        }

        protected override void SetUpGraphics()
        {
            backgroundImage.SetStyleSheet(CurrentStyleSheet.BackgroundStyleSheet);
        }

        #endregion
    }
}
