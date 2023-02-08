using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ScrollbarStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public ImageStyleSheet handleStyleSheet;
    }
}
