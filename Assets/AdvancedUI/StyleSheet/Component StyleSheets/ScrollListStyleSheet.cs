using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class ScrollListStyleSheet
    {
        public bool frameActive = false;
        [ShowIf(nameof(frameActive))][AllowNesting] public StaticImageStyleSheet frameStyleSheet;
        public StaticImageStyleSheet frameMaskStyleSheet;
        public StaticImageStyleSheet backgroundStyleSheet;
        [Space]
        public StaticImageStyleSheet horizontalMaskStyleSheet;
        public StaticImageStyleSheet verticalMaskStyleSheet;
        [Space]
        public StaticTextStyleSheet textStyleSheet;
        [Space]
        public AdvancedButtonType leftButtonType;
        public AdvancedButtonType rightButtonType;
    }
}
