using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

namespace Dhs5.AdvancedUI
{
    #region Slider Content
    [Serializable]
    public struct SliderContent
    {

        // ### Properties ###
        [Header("Slider base properties")]
        public int minValue;
        public int maxValue;
        public bool wholeNumbers;

        [Header("Text")]
        public string text;
    }
    #endregion

    public class AdvancedSlider : AdvancedComponent
    {
        [Header("Slider Type")]
        [SerializeField] private AdvancedSliderType sliderType;
        public AdvancedSliderType Type { get { return sliderType; } set { sliderType = value; SetUpConfig(); } }

        [Header("Slider Content")]
        [SerializeField] private SliderContent sliderContent;
        public SliderContent Content { get { return sliderContent; } set { sliderContent = value; SetUpConfig(); } }
        public float SliderValue { get { return slider.value; } set { slider.value = value; } }

        public override bool Interactable { get => slider.interactable; set => slider.interactable = value; }


        [Header("Events")]
        [SerializeField] private UnityEvent<float> onValueChanged;
        [SerializeField] private UnityEvent onButtonDown;
        [SerializeField] private UnityEvent onButtonUp;

        public event Action<float> OnValueChanged;
        public event Action OnButtonDown { add { slider.OnSliderDown += value; } remove { slider.OnSliderDown -= value; } }
        public event Action OnButtonUp { add { slider.OnSliderUp += value; } remove { slider.OnSliderUp -= value; } }



        [Header("Custom Style Sheet")]
        [SerializeField] private SliderStyleSheet customStyleSheet;

        [Header("Style Sheet Container")]
        [SerializeField] private StyleSheetContainer styleSheetContainer;
        private SliderStyleSheet CurrentStyleSheet
        { get { return Type == AdvancedSliderType.CUSTOM ? customStyleSheet :
                    styleSheetContainer ? styleSheetContainer.projectStyleSheet.sliderStyleSheets.GetStyleSheet(Type) : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenSlider slider;
        [Space]
        [SerializeField] private Image backgroundImage;
        [Space]
        [SerializeField] private Image handle;
        [SerializeField] private Image fill;
        [Space]
        [SerializeField] private TextMeshProUGUI sliderText;


        protected override void Awake()
        {
            slider.GetGraphics(backgroundImage, CurrentStyleSheet.backgroundStyleSheet,
                handle, CurrentStyleSheet.handleStyleSheet,
                fill, CurrentStyleSheet.fillStyleSheet,
                sliderText, CurrentStyleSheet.textStyleSheet);

            base.Awake();
        }

        #region Private Functions
        private void SetGradient()
        {
            if (!CurrentStyleSheet.isGradient) return;

            if (fill && fill.enabled)
            {
                fill.color = CurrentStyleSheet.sliderGradient.Evaluate(slider.normalizedValue);
            }
            else if (backgroundImage && backgroundImage.enabled)
            {
                backgroundImage.color = CurrentStyleSheet.sliderGradient.Evaluate(slider.normalizedValue);
            }
        }
        #endregion

        #region Events

        protected override void LinkEvents()
        {
            slider.onValueChanged.AddListener(ValueChanged);
            OnButtonDown += ButtonDown;
            OnButtonUp += ButtonUp;
        }
        protected override void UnlinkEvents()
        {
            slider.onValueChanged?.RemoveListener(ValueChanged);
            OnButtonDown -= ButtonDown;
            OnButtonUp -= ButtonUp;
        }

        private void ValueChanged(float value)
        {
            onValueChanged?.Invoke(value);
            OnValueChanged?.Invoke(value);

            SetGradient();
        }
        private void ButtonDown()
        {
            onButtonDown?.Invoke();
        }
        private void ButtonUp()
        {
            onButtonUp?.Invoke();
        }

        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (CurrentStyleSheet == null) return;

            SetSliderInfo();

            // Background
            if (backgroundImage)
            {
                backgroundImage.enabled = CurrentStyleSheet.backgroundActive;
                backgroundImage.SetUpImage(CurrentStyleSheet.backgroundStyleSheet);
            }

            // Fill
            if (fill)
            {
                fill.enabled = CurrentStyleSheet.fillActive;
                fill.SetUpImage(CurrentStyleSheet.fillStyleSheet);
            }

            // Handle
            if (handle)
            {
                handle.enabled = CurrentStyleSheet.handleActive;
                handle.SetUpImage(CurrentStyleSheet.handleStyleSheet);
            }

            // Text
            if (sliderText)
            {
                sliderText.enabled = CurrentStyleSheet.textActive;
                sliderText.text = Content.text;
                sliderText.SetUpText(CurrentStyleSheet.textStyleSheet);
            }

            SetGradient();
        }

        private void SetSliderInfo()
        {
            if (slider)
            {
                slider.minValue = Content.minValue;
                slider.maxValue = Content.maxValue;
                slider.wholeNumbers = Content.wholeNumbers;
            }
        }

        #endregion
    }
}
