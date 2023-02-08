using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dhs5.AdvancedUI
{
    #region ScrollView Content

    [System.Serializable]
    public struct ScrollViewContent
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

    public class AdvancedScrollView : AdvancedComponent
    {
        [Header("ScrollView Type")]
        [SerializeField] private AdvancedScrollViewType scrollviewType;
        public AdvancedScrollViewType Type { get { return scrollviewType; } set { scrollviewType = value; SetUpConfig(); } }

        [Header("ScrollView Content")]
        [SerializeField] private ScrollViewContent scrollViewContent;
        public ScrollViewContent Content { get { return scrollViewContent; } set { scrollViewContent = value; SetUpConfig(); } }

        public override bool Interactable { get => scrollView.enabled; set => scrollView.enabled = value; }


        [Header("Custom Style Sheet")]
        [SerializeField] private ScrollViewStyleSheet customStyleSheet;

        [Header("Style Sheet Container")]
        [SerializeField] private StyleSheetContainer styleSheetContainer;
        private ScrollViewStyleSheet CurrentStyleSheet
        { get { return Type == AdvancedScrollViewType.CUSTOM ? customStyleSheet :
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.scrollViewStyleSheets.GetStyleSheet(Type) : null; } }


        [Header("UI Components")]
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private SelectableGraphic backgroundImage;
        [Space]
        [SerializeField] private RectTransform viewportRect;
        [SerializeField] private RectTransform contentRect;
        [Space]
        [SerializeField] private AdvancedScrollbar verticalScrollbar;
        [SerializeField] private AdvancedScrollbar horizontalScrollbar;


        #region Events
        protected override void LinkEvents() { }
        protected override void UnlinkEvents() { }
        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (CurrentStyleSheet == null) return;

            // Background
            if (backgroundImage)
            {
                backgroundImage.targetGraphic.enabled = CurrentStyleSheet.backgroundActive;
                backgroundImage.GraphicStyleSheet = CurrentStyleSheet.backgroundStyleSheet;
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
                verticalScrollbar.gameObject.SetActive(CurrentStyleSheet.verticalScrollbarActive);
                verticalScrollbar.Type = CurrentStyleSheet.verticalScrollbarType;
            }
            if (horizontalScrollbar)
            {
                horizontalScrollbar.gameObject.SetActive(CurrentStyleSheet.horizontalScrollbarActive);
                horizontalScrollbar.Type = CurrentStyleSheet.horizontalScrollbarType;
            }
        }

        #endregion
    }
}
