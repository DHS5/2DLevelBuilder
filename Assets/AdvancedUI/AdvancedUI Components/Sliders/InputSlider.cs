using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.AdvancedUI
{
    public class InputSlider : AdvancedComponent
    {
        [Header("Slider")]
        [SerializeField] private StylePicker sliderStylePicker;
        public StylePicker SliderStyle { get =>  sliderStylePicker; set { sliderStylePicker.ForceSet(value); SetUpConfig(); } }

        [SerializeField] private AdvancedSlider.SliderContent sliderContent;
        public AdvancedSlider.SliderContent SliderContent { get => sliderContent; set { sliderContent = value; SetUpConfig(); } }
        
        [Header("InputField")]
        [SerializeField] private StylePicker inputfieldStylePicker;
        public StylePicker InputStyle { get => inputfieldStylePicker; set { inputfieldStylePicker.ForceSet(value); SetUpConfig(); } }

        public override bool Interactable 
        {
            get { return slider.Interactable; }
            set
            {
                slider.Interactable = value;
                inputField.Interactable = value;
            }
        }

        [Header("Events")]
        [SerializeField] private UnityEvent<float> onValueChanged;
        [SerializeField] private UnityEvent onHandleDown;
        [SerializeField] private UnityEvent onHandleUp;
        [SerializeField] private UnityEvent<float> onSubmit;
        [SerializeField] private UnityEvent<float> onEndEdit;

        public event Action<float> OnValueChanged;
        public event Action OnHandleDown { add { slider.OnButtonDown += value; } remove { slider.OnButtonDown -= value; } }
        public event Action OnHandleUp { add { slider.OnButtonUp += value; } remove { slider.OnButtonUp -= value; } }
        public event Action<float> OnSubmit;
        public event Action<float> OnEndEdit;


        [Header("UI components")]
        [SerializeField] private AdvancedSlider slider;
        [SerializeField] private AdvancedInputField inputField;


        protected override void Awake()
        {
            base.Awake();

            inputField.Text = slider.SliderValue.ToString();
        }


        #region Events
        protected override void LinkEvents()
        {
            slider.OnValueChanged += ValueChanged;
            slider.OnButtonDown += HandleDown;
            slider.OnButtonUp += HandleUp;
            inputField.OnValueChanged += ValueChanged;
            inputField.OnSubmit += Submit;
            inputField.OnEndEdit += EndEdit;
        }
        protected override void UnlinkEvents()
        {
            slider.OnValueChanged -= ValueChanged;
            slider.OnButtonDown -= HandleDown;
            slider.OnButtonUp -= HandleUp;
            inputField.OnValueChanged -= ValueChanged;
            inputField.OnSubmit -= Submit;
            inputField.OnEndEdit -= EndEdit;
        }

        private void ValueChanged(float value)
        {
            OnValueChanged?.Invoke(value);

            inputField.Text = value.ToString("0.00");
        }
        private void ValueChanged(string str)
        {
            if (!float.TryParse(str, out float value)) value = slider.Content.minValue;
            value = Mathf.Clamp(value, slider.Content.minValue, slider.Content.maxValue);

            OnValueChanged?.Invoke(value);
            slider.SliderValue = value;
        }
        private void EndEdit(string str)
        {
            if (!float.TryParse(str, out float value)) value = slider.Content.minValue;
            value = Mathf.Clamp(value, slider.Content.minValue, slider.Content.maxValue);
            
            inputField.Text = value.ToString("0.00");
            OnEndEdit?.Invoke(value);
        }
        private void Submit(string str)
        {
            if (!float.TryParse(str, out float value)) value = slider.Content.minValue;
            value = Mathf.Clamp(value, slider.Content.minValue, slider.Content.maxValue);

            OnSubmit?.Invoke(value);
        }
        private void HandleDown() { onHandleDown?.Invoke(); }
        private void HandleUp() { onHandleUp?.Invoke(); }
        #endregion

        #region Configs

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            sliderStylePicker.SetUp(styleSheetContainer, StyleSheetType.SLIDER, "Slider type");
            inputfieldStylePicker.SetUp(styleSheetContainer, StyleSheetType.INPUT_FIELD, "InputField type");

            if (slider)
            {
                slider.SetContainer(styleSheetContainer);
                slider.Style = SliderStyle;
                slider.Content = SliderContent;
            }
            if (inputField)
            {
                inputField.SetContainer(styleSheetContainer);
                inputField.Style = InputStyle;
            }
        }

        protected override void SetUpGraphics() { }

        #endregion
    }
}
