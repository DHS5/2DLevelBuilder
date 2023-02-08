using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ScrollViewStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public GraphicStyleSheet backgroundStyleSheet;
        [Space]
        public bool verticalScrollbarActive = true;
        [ShowIf(nameof(verticalScrollbarActive))]
        [AllowNesting]
        public AdvancedScrollbarType verticalScrollbarType;
        [Space]
        public bool horizontalScrollbarActive = false;
        [ShowIf(nameof(horizontalScrollbarActive))]
        [AllowNesting]
        public AdvancedScrollbarType horizontalScrollbarType;
    }
}
