using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class DropdownStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public bool titleActive = true;
        [ShowIf(nameof(titleActive))]
        [AllowNesting]
        public TextType titleType;
        [Space, Space]
        public bool arrowActive = true;
        [ShowIf(nameof(arrowActive))]
        [AllowNesting]
        public ImageStyleSheet arrowStyleSheet;
        [Space, Space]
        public TextType textType;
        [Space, Space]
        public AdvancedScrollViewType templateScrollviewType;
        [Space, Space]
        public DropdownItemToggleType itemToggleType;
    }
}
