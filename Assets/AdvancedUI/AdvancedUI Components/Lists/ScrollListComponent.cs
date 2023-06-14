using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;
using TMPro;
using Dhs5.Utility;
using UnityEngine.Events;

namespace Dhs5.AdvancedUI
{
    public class ScrollListComponent : AdvancedComponent
    {
        #region Enums
        [System.Serializable]
        private enum ScrollDirection { Horizontal, Vertical }
        
        [System.Serializable]
        private enum ListFormat { Infinite, Simple }
        #endregion

        #region Scroll List Content
        [Serializable]
        public class ScrollListContent
        {
            public float socketSize = 100;
            public float spaceBetweenSockets = 10;
            [Space]
            public bool useScroll = true;
            [ShowIf(nameof(useScroll))][AllowNesting] public float scrollSensitivity = 100;
            [Space]
            public bool useDisplay = true;
            [ShowIf(nameof(useDisplay))][AllowNesting] public float displayHeight = 80;
            [Space]
            public bool useButtons = false;
            [ShowIf(nameof(useButtons))][AllowNesting] public float buttonsHeight = 80;
            [Space]
            public bool useAnim = false;
            [ShowIf(nameof(useAnim))][AllowNesting] public float animLerp = 0.5f;
            [ShowIf(nameof(useAnim))][AllowNesting] public float animDelay = 0.02f;
        }
        #endregion

        [Header("Parameters")]
        [OnValueChanged(nameof(InverseDimension))]
        [SerializeField] private ScrollDirection scrollDirection;
        private bool IsHorizontal => scrollDirection == ScrollDirection.Horizontal;
        private bool IsVertical => scrollDirection == ScrollDirection.Vertical;
        [Space]
        [SerializeField] private ListFormat format;
        private bool IsInfinite => format == ListFormat.Infinite;
        private bool IsSimple => format == ListFormat.Simple;
        [Space]
        [SerializeField] private StylePicker scrollListStylePicker;
        public StylePicker Style { get => scrollListStylePicker; set { scrollListStylePicker.ForceSet(value); SetUpConfig(); } }

        [Space]
        [SerializeField] private ScrollListContent scrollListContent;
        public ScrollListContent Content { get { return scrollListContent; } set { scrollListContent = value; SetUpConfig(); } }

        [Header("Events")]
        [SerializeField] private UnityEvent<int> OnSelectionChange;

        public event Action<int> onSelectionChange;


        [Header("Custom Style Sheet")]
        [SerializeField] private bool custom;
        [SerializeField] private ScrollListStyleSheet customStyleSheet;

        private ScrollListStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? scrollListStylePicker.StyleSheet as ScrollListStyleSheet : null; } }

        

        [Space, Space]
        #region UI Components
        [Header("UI Components")]
        [SerializeField] private DragableUI dragableObject;
        [SerializeField] private Image frame;
        [SerializeField] private Image frameMask;
        [SerializeField] private Image background;
        [SerializeField] private Image mask;
        [Space]
        // Infinite
        [ShowIf(EConditionOperator.And, nameof(IsHorizontal), nameof(IsInfinite))][SerializeField] 
        private RectTransform socketHorizontalContainer;
        [ShowIf(EConditionOperator.And, nameof(IsHorizontal), nameof(IsInfinite))][SerializeField] 
        private HorizontalLayoutGroup horizontalLayout;
        [ShowIf(EConditionOperator.And, nameof(IsVertical), nameof(IsInfinite))][SerializeField] 
        private RectTransform socketVerticalContainer;
        [ShowIf(EConditionOperator.And, nameof(IsVertical), nameof(IsInfinite))][SerializeField] 
        private VerticalLayoutGroup verticalLayout;
        
        // Simple
        [ShowIf(EConditionOperator.And, nameof(IsHorizontal), nameof(IsSimple))][SerializeField] 
        private RectTransform socketHorizontalSimpleContainer;
        [ShowIf(EConditionOperator.And, nameof(IsHorizontal), nameof(IsSimple))][SerializeField] 
        private HorizontalLayoutGroup horizontalSimpleLayout;
        [ShowIf(EConditionOperator.And, nameof(IsVertical), nameof(IsSimple))][SerializeField] 
        private RectTransform socketVerticalSimpleContainer;
        [ShowIf(EConditionOperator.And, nameof(IsVertical), nameof(IsSimple))][SerializeField] 
        private VerticalLayoutGroup verticalSimpleLayout;
        [ShowIf(EConditionOperator.And, nameof(IsHorizontal), nameof(IsSimple))][SerializeField] 
        private HorizontalLayoutGroup objectHorizontalLayout;
        [ShowIf(EConditionOperator.And, nameof(IsVertical), nameof(IsSimple))][SerializeField] 
        private VerticalLayoutGroup objectVerticalLayout;

        [SerializeField] private GameObject scrollListObjectPrefab;
        [Space]
        [SerializeField] private RectTransform displayContainer;
        [SerializeField] private TextMeshProUGUI displayText;
        [Space]
        [SerializeField] private RectTransform buttonsContainer;
        [SerializeField] private AdvancedButton lessButton;
        [SerializeField] private AdvancedButton plusButton;

        private RectTransform SocketContainer { get { return IsHorizontal ? socketHorizontalContainer : socketVerticalContainer; } }
        private HorizontalOrVerticalLayoutGroup Layout { get { return IsHorizontal ? horizontalLayout : verticalLayout; } }
        private HorizontalOrVerticalLayoutGroup SimpleLayout 
        { get { return IsHorizontal ? horizontalSimpleLayout : verticalSimpleLayout; } }
        private HorizontalOrVerticalLayoutGroup ObjectLayout 
        { get { return IsHorizontal ? objectHorizontalLayout : objectVerticalLayout; } }

        [Space, Space]
        [Header("Sockets")]
        [SerializeField] private ScrollListSocket mainSocket;
        [ShowNativeProperty] public int TotalObjectNumber => sockets.Count;

        [ReadOnly][SerializeField] List<ScrollListSocket> sockets = new();
        [ReadOnly][SerializeField] List<ScrollListSocket> socketsInOrder = new();
        #endregion


        private bool interactable = false;
        public override bool Interactable 
        {
            get { return interactable; }
            set { interactable = value; SetScrollListState(value); }
        }

        public int CurrentSelectionIndex
        {
            get
            {
                if (scrollList == null) return 0;
                return scrollList.CurrentSelectionIndex();
            }
        }

        #region Events
        protected override void OnEnable()
        {
            base.OnEnable();

            SetScrollListState(Interactable);

            if (Content.useButtons) EnableButtons();
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            SetScrollListState(false);

            if (Content.useButtons) DisableButtons();
        }

        protected override void LinkEvents()
        {
            if (scrollList == null) return;

            scrollList.OnSelectionChange += SelectionChange;
            scrollList.InvokeSelectionChange();
        }
        protected override void UnlinkEvents()
        {
            if (scrollList == null) return;

            scrollList.OnSelectionChange -= SelectionChange;
        }

        private void SelectionChange(int index, string display)
        {
            onSelectionChange?.Invoke(index);
            OnSelectionChange?.Invoke(index);

            SetDisplayText(display);
        }
        #endregion

        #region ScrollList Management

        private IScrollList scrollList;
        public void CreateList<T>(List<T> list)
        {
            if (list == null || list.Count < 1)
            {
                ZDebug.LogE("List is null or empty");
                return;
            }

            if (scrollList != null)
            {
                UnlinkEvents();
                scrollList.Destroy();
            }

            // Infinite List
            if (IsInfinite) CreateInfiniteList(list);
            // Simple List
            else if (IsSimple) CreateSimpleList(list);

            // Event listening
            LinkEvents();

            Interactable = true;
        }
        private void CreateInfiniteList<T>(List<T> list)
        {
            scrollList = new InfiniteScrollList<T>(this, list, sockets, socketsInOrder, dragableObject, scrollListObjectPrefab,
                IsHorizontal, Content.useScroll, Content.scrollSensitivity, Content.socketSize, Content.spaceBetweenSockets,
                Content.useAnim, Content.animLerp, Content.animDelay);
        }
        private void CreateSimpleList<T>(List<T> list)
        {
            CreateSocketsForSimpleList(list.Count);

            scrollList = new SimpleScrollList<T>(this, list, sockets, ObjectLayout.gameObject, dragableObject, scrollListObjectPrefab,
                IsHorizontal, Content.useScroll, Content.scrollSensitivity, Content.socketSize, Content.spaceBetweenSockets,
                Content.useAnim, Content.animLerp, Content.animDelay);
        }

        private void SetScrollListState(bool state)
        {
            if (scrollList == null) return;

            if (state && isActiveAndEnabled)
                scrollList.Enable();
            else
                scrollList.Disable();
        }
        #endregion

        #region Socket Management

        [Button("Add 2 sockets")]
        private void Add()
        {
            AddSocket(true, SocketContainer);
            AddSocket(false, SocketContainer);
        }
        private void AddSocket(bool first, RectTransform container)
        {
            ScrollListSocket socket = Instantiate(mainSocket.gameObject, container).GetComponent<ScrollListSocket>();
            socket.name = "socket " + TotalObjectNumber;
            socket.Width = Content.socketSize;
            if (first)
            {
                socket.transform.SetAsFirstSibling();
                socketsInOrder.Insert(0, socket);
            }
            else socketsInOrder.Add(socket);
            sockets.Add(socket);
        }
        [Button("Remove 2 sockets")]
        private void Remove()
        {
            if (TotalObjectNumber < 3) return;
            RemoveSocket();
            RemoveSocket();
        }
        private void RemoveSocket()
        {
            ScrollListSocket socket = sockets[TotalObjectNumber - 1];
            sockets.Remove(socket);
            socketsInOrder.Remove(socket);
            DestroyImmediate(socket.gameObject);
        }
        [Button("Reset sockets")]
        private void ResetSockets()
        {
            if (sockets != null)
            {
                foreach (var socket in sockets)
                    if (socket && socket != mainSocket)
                        DestroyImmediate(socket.gameObject);
            }
            sockets = new();
            socketsInOrder = new();
            sockets.Add(mainSocket);
            socketsInOrder.Add(mainSocket);
        }

        private void CreateSocketsForSimpleList(int listSize)
        {
            RectTransform container = IsHorizontal ? socketHorizontalSimpleContainer : socketVerticalSimpleContainer;
            ResetSockets();
            mainSocket.transform.SetParent(container, false);
            mainSocket.Index = 0;
            for (int i = 1; i < listSize; i++)
            {
                AddSocket(true, container);
                sockets[i].Index = i;
            }
            ObjectLayout.transform.SetParent(mainSocket.transform, false);
            RectTransform rectTransform = ObjectLayout.transform as RectTransform;
            rectTransform.SetAnchor(RectTransformAnchor.MIDDLE_CENTER, true);
            rectTransform.SetSize(mainSocket.Width, mainSocket.Height, !IsHorizontal, IsHorizontal);
        }
        #endregion

        #region Buttons & Display Management
        private void EnableButtons()
        {
            if (lessButton) lessButton.OnClick += Less;
            if (plusButton) plusButton.OnClick += Plus;
        }
        private void DisableButtons()
        {
            if (lessButton) lessButton.OnClick -= Less;
            if (plusButton) plusButton.OnClick -= Plus;
        }

        private void Less()
        {
            if (scrollList != null)
                scrollList.AutoScroll(-1);
        }
        private void Plus()
        {
            if (scrollList != null)
                scrollList.AutoScroll(1);
        }

        private void SetDisplayText(string text)
        {
            if (displayText)
                displayText.text = text;
        }
        #endregion

        #region Configs
        private void SetActiveObjects()
        {
            socketHorizontalContainer.gameObject.SetActive(IsHorizontal && IsInfinite);
            socketVerticalContainer.gameObject.SetActive(IsVertical && IsInfinite);
            socketHorizontalSimpleContainer.gameObject.SetActive(IsHorizontal && IsSimple);
            socketVerticalSimpleContainer.gameObject.SetActive(IsVertical && IsSimple);
        }
        private void ResizeSockets()
        {
            foreach (var socket in sockets)
            {
                if (IsHorizontal)
                    socket.Width = Content.socketSize;
                else
                    socket.Height = Content.socketSize;
            }
        }
        private void SetLayoutSpacing()
        {
            Layout.spacing = Content.spaceBetweenSockets;
            SimpleLayout.spacing = Content.spaceBetweenSockets;
            ObjectLayout.spacing = Content.spaceBetweenSockets;
        }
        private void InverseDimension()
        {
            RectTransform rectTransform = transform as RectTransform;
            Vector2 dimensions = rectTransform.rect.size;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dimensions.y);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dimensions.x);

            SetMask();
        }
        private void ChangeSocketsParent()
        {
            foreach (var socket in socketsInOrder)
            {
                if (socket != mainSocket)
                    socket.transform.SetParent(SocketContainer);
            }
        }

        private void SetMask()
        {
            if (CurrentStyleSheet == null) return;
            mask.SetUpImage(IsHorizontal ? CurrentStyleSheet.HorizontalMaskStyleSheet : CurrentStyleSheet.VerticalMaskStyleSheet);
        }
        private void SetDisplay()
        {
            if (displayContainer)
            {
                displayContainer.gameObject.SetActive(Content.useDisplay);
                displayContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.displayHeight);

                if (CurrentStyleSheet == null) return;

                displayText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }
        }
        private void SetButtons()
        {
            if (buttonsContainer)
            {
                buttonsContainer.gameObject.SetActive(Content.useButtons);
                buttonsContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.buttonsHeight);

                if (CurrentStyleSheet == null) return;

                if (lessButton)
                {
                    lessButton.Style = CurrentStyleSheet.LeftButtonStyle;
                }
                if (plusButton)
                {
                    plusButton.Style = CurrentStyleSheet.RightButtonStyle;
                }
            }
        }

        private void SetUpStyle()
        {
            if (CurrentStyleSheet == null) return;

            if (frame) frame.SetUpImage(CurrentStyleSheet.FrameStyleSheet);
            if (frameMask) frameMask.SetUpImage(CurrentStyleSheet.FrameMaskStyleSheet);
            if (background) background.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
        }
        
        protected override void SetUpConfig()
        {
            SetActiveObjects();

            ChangeSocketsParent();
            ResizeSockets();

            SetLayoutSpacing();

            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            scrollListStylePicker.SetUp(styleSheetContainer, StyleSheetType.SCROLL_LIST, "ScrollList Style");

            if (CurrentStyleSheet == null) return;

            SetButtons();
            SetDisplay();

            SetMask();

            SetUpStyle();
        }

        protected override void SetUpGraphics() { }
        #endregion
    }
}
