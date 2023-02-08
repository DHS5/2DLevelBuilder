using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class SliderStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public bool fillActive = true;
        [ShowIf(nameof(fillActive))]
        [AllowNesting]
        public ImageStyleSheet fillStyleSheet;
        [Space, Space]
        public bool handleActive = true;
        [ShowIf(nameof(handleActive))]
        [AllowNesting]
        public ImageStyleSheet handleStyleSheet;
        [Space, Space]
        public bool textActive = false;
        [ShowIf(nameof(textActive))]
        [AllowNesting]
        public TextStyleSheet textStyleSheet;
        [Space, Space]
        public bool isGradient = false;
        [ShowIf(nameof(isGradient))]
        [AllowNesting]
        public Gradient sliderGradient;
    }
}
