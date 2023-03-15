using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class PopupStyleSheet
    {
        public ImageStyleSheet popupStyleSheet;
        [Space]
        public bool filterActive = true;
        [ShowIf(nameof(filterActive))]
        [AllowNesting]
        public Color filterColor;
        [Space]
        public bool textActive = true;
        [ShowIf(nameof(textActive))]
        [AllowNesting]
        public TextType textType;
        [Space]
        public bool confirmButtonActive = false;
        [ShowIf(nameof(confirmButtonActive))]
        [AllowNesting]
        public AdvancedButtonType confirmButtonType;
        public bool cancelButtonActive = false;
        [ShowIf(nameof(cancelButtonActive))]
        [AllowNesting]
        public AdvancedButtonType cancelButtonType;
        public bool quitButtonActive = true;
        [ShowIf(nameof(quitButtonActive))]
        [AllowNesting]
        public AdvancedButtonType quitButtonType;
    }
}
