using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class SwitchToggleStyleSheet
    {
        public ImageStyleSheet backgroundStyleSheet;
        public ImageStyleSheet foregroundStyleSheet;
        [Space, Space]
        public ImageStyleSheet handleStyleSheet;
        [Space, Space]
        public bool leftTextActive;
        [ShowIf(nameof(leftTextActive))]
        [AllowNesting]
        public TextStyleSheet leftTextStyleSheet;
        public bool rightTextActive;
        [ShowIf(nameof(rightTextActive))]
        [AllowNesting]
        public TextStyleSheet rightTextStyleSheet;
    }
}
