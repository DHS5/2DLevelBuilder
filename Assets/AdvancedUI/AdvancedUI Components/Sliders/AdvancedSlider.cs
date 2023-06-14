using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class AdvancedSlider : AdvancedComponent
    {
        #region Slider Content
        [Serializable]
        public class SliderContent
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

        [Header("Slider Type")]
        [SerializeField] private StylePicker sliderStylePicker;
        public StylePicker Style { get => sliderStylePicker; set { sliderStylePicker.ForceSet(value); SetUpConfig(); } }

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
        [SerializeField] private bool custom;
        [SerializeField] private SliderStyleSheet customStyleSheet;

        private SliderStyleSheet CurrentStyleSheet
        { get { return custom ? customStyleSheet : styleSheetContainer ? Style.StyleSheet as SliderStyleSheet : null; } }


        [Header("UI Components")]
        [SerializeField] private OpenSlider slider;
        [Space]
        [SerializeField] private Image backgroundImage;
        [Space]
        [SerializeField] private Image handle;
        [SerializeField] private Image fill;
        [Space]
        [SerializeField] private TextMeshProUGUI sliderText;


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
            if (styleSheetContainer == null) return;

            customStyleSheet.SetUp(styleSheetContainer);
            sliderStylePicker.SetUp(styleSheetContainer, StyleSheetType.SLIDER, "Slider type");

            if (CurrentStyleSheet == null) return;

            SetSliderInfo();

            // Background
            if (backgroundImage)
            {
                backgroundImage.enabled = CurrentStyleSheet.backgroundActive;
                backgroundImage.SetUpImage(CurrentStyleSheet.BackgroundStyleSheet);
            }

            // Fill
            if (fill)
            {
                fill.enabled = CurrentStyleSheet.fillActive;
                fill.SetUpImage(CurrentStyleSheet.FillStyleSheet);
            }

            // Handle
            if (handle)
            {
                handle.enabled = CurrentStyleSheet.handleActive;
                handle.SetUpImage(CurrentStyleSheet.HandleStyleSheet);
            }

            // Text
            if (sliderText)
            {
                sliderText.enabled = CurrentStyleSheet.textActive;
                sliderText.text = Content.text;
                sliderText.SetUpText(CurrentStyleSheet.TextStyleSheet);
            }

            SetGradient();
        }

        protected override void SetUpGraphics()
        {
            slider.GetGraphics(backgroundImage, CurrentStyleSheet.BackgroundStyleSheet,
                handle, CurrentStyleSheet.HandleStyleSheet,
                fill, CurrentStyleSheet.FillStyleSheet,
                sliderText, CurrentStyleSheet.TextStyleSheet);
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
