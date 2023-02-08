using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ToggleStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public bool checkmarkActive = true;
        [ShowIf(nameof(checkmarkActive))]
        [AllowNesting]
        public float checkmarkScale = 1;
        [ShowIf(nameof(checkmarkActive))]
        [AllowNesting]
        public GraphicStyleSheet checkmarkStyleSheet;
        [Space, Space]
        public bool uncheckmarkActive = true;
        [ShowIf(nameof(uncheckmarkActive))]
        [AllowNesting]
        public float uncheckmarkScale = 1;
        [ShowIf(nameof(uncheckmarkActive))]
        [AllowNesting]
        public GraphicStyleSheet uncheckmarkStyleSheet;
        [Space, Space]
        public bool textActive = true;
        [ShowIf(nameof(textActive))]
        [AllowNesting]
        public TextStyleSheet textStyleSheet;
    }
}
