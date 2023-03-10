using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

namespace Dhs5.AdvancedUI
{
    #region Dropdown Content
    [System.Serializable]
    public struct DropdownContent
    {
        public DropdownContent(List<string> options, string title = "", float tHeight = 200, float iHeight = 40)
        {
            dropdownTitle = title;
            dropdownOptions = options;
            templateHeight = tHeight;
            itemHeight = iHeight;
        }

        // ### Properties ###
        public string dropdownTitle;
        public List<string> dropdownOptions;
        [Space]
        [SerializeField] private float templateHeight;
        public float TemplateHeight { get { return templateHeight > 0 ? templateHeight : 200; } set { templateHeight = value; } }
        [SerializeField] private float itemHeight;
        public float ItemHeight { get { return itemHeight > 0 ? itemHeight : 40; } set { itemHeight = value; } }
    }
    #endregion

    public class AdvancedDropdown : AdvancedComponent
    {
        [Header("Dropdown Type")]
        [SerializeField] private AdvancedDropdownType dropdownType;
        public AdvancedDropdownType Type { get { return dropdownType; } set { dropdownType = value; SetUpConfig(); } }

        [Header("Dropdown Content")]
        [SerializeField] private DropdownContent dropdownContent;
        public DropdownContent Content { get { return dropdownContent; } set { dropdownContent = value; SetUpConfig(); } }

        public override bool Interactable { get => dropdown.interactable; set => dropdown.interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent<int> onValueChanged;
        [SerializeField] private UnityEvent onClick;

        public event Action<int> OnValueChanged;
        public event Action OnClick { add { dropdown.OnClick += value; } remove { dropdown.OnClick -= value; } }



        [Header("Custom Style Sheet")]
        [SerializeField] private DropdownStyleSheet customStyleSheet;


        [Header("Style Sheet Container")]
        [SerializeField] private StyleSheetContainer styleSheetContainer;
        private DropdownStyleSheet CurrentStyleSheet { get { return dropdownType == AdvancedDropdownType.CUSTOM ? customStyleSheet : 
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.dropdownStyleSheets.GetStyleSheet(dropdownType) : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenDropdown dropdown;
        [SerializeField] private Image dropdownBackground;
        [Space]
        [SerializeField] private TextMeshProUGUI titleText;
        [Space]
        [SerializeField] private Image arrowImage;
        [SerializeField] private TextMeshProUGUI dropdownText;
        [Space]
        [SerializeField] private AdvancedScrollView templateScrollView;
        [Space]
        [SerializeField] private DropdownItemToggle itemToggle;



        protected override void Awake()
        {
            dropdown.GetGraphics(dropdownBackground, CurrentStyleSheet.backgroundStyleSheet,
                titleText, CurrentStyleSheet.titleStyleSheet,
                arrowImage, CurrentStyleSheet.arrowStyleSheet,
                dropdownText, CurrentStyleSheet.textStyleSheet);

            base.Awake();
        }

        #region Events
        protected override void LinkEvents()
        {
            dropdown.onValueChanged.AddListener(ValueChanged);
            OnClick += Click;
        }
        protected override void UnlinkEvents()
        {
            dropdown.onValueChanged?.RemoveListener(ValueChanged);
            OnClick -= Click;
        }

        private void ValueChanged(int index)
        {
            onValueChanged?.Invoke(index);
            OnValueChanged?.Invoke(index);
        }
        private void Click()
        {
            onClick?.Invoke();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (CurrentStyleSheet == null) return;

            // Dropdown
            if (dropdown)
            {
                if (arrowImage) dropdown.ArrowRect = arrowImage.rectTransform;
                dropdown.ClearOptions();
                dropdown.AddOptions(Content.dropdownOptions);
            }
            
            // Background
            if (dropdownBackground)
            {
                dropdownBackground.enabled = CurrentStyleSheet.backgroundActive;
                dropdownBackground.SetUpImage(CurrentStyleSheet.backgroundStyleSheet);
            }

            // Title
            if (titleText)
            {
                titleText.enabled = CurrentStyleSheet.titleActive;
                titleText.SetUpText(CurrentStyleSheet.titleStyleSheet);
                titleText.text = Content.dropdownTitle;
                titleText.rectTransform.SetSizeWithCurrentAnchors
                    (RectTransform.Axis.Vertical, (gameObject.transform as RectTransform).rect.height);
            }

            // Arrow
            if (arrowImage)
            {
                arrowImage.enabled = CurrentStyleSheet.arrowActive;
                arrowImage.SetUpImage(CurrentStyleSheet.arrowStyleSheet);
            }

            // Title
            if (dropdownText)
            {
                dropdownText.SetUpText(CurrentStyleSheet.textStyleSheet);
            }

            // ScrollView
            if (templateScrollView)
            {
                templateScrollView.Content = new ScrollViewContent(ScrollViewContent.ScrollViewDirection.VERTICAL, Content.ItemHeight);
                templateScrollView.Type = CurrentStyleSheet.templateScrollviewType;
                (templateScrollView.transform as RectTransform).
                    SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Content.TemplateHeight);
            }

            // Item Toggle
            if (itemToggle)
            {
                itemToggle.Type = CurrentStyleSheet.itemToggleType;
            }
        }

        #endregion
    }
}
