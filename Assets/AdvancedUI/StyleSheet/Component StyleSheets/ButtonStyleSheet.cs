using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ButtonStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public bool iconActive = true;
        [ShowIf(nameof(iconActive))]
        [AllowNesting]
        public float iconScale = 1;
        [ShowIf(nameof(iconActive))]
        [AllowNesting]
        public ImageStyleSheet iconStyleSheet;
        [Space, Space]
        public bool textActive = false;
        [ShowIf(nameof(textActive))]
        [AllowNesting]
        public TextStyleSheet textStyleSheet;
    }
}
