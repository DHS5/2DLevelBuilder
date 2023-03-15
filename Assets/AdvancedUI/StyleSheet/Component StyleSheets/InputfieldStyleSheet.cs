using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class InputfieldStyleSheet
    {
        public bool backgroundActive = true;
        [ShowIf(nameof(backgroundActive))]
        [AllowNesting]
        public ImageStyleSheet backgroundStyleSheet;
        [Space, Space]
        public bool hintTextActive = true;
        [ShowIf(nameof(hintTextActive))]
        [AllowNesting]
        public TextType hintTextType;
        [Space, Space]
        public TextType inputTextType;
        public Color selectionColor;
    }
}
