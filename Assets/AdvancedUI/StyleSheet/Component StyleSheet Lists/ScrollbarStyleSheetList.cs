using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedScrollbarType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class ScrollbarStyleSheetList
    {
        public ScrollbarStyleSheet basic;


        public ScrollbarStyleSheet GetStyleSheet(AdvancedScrollbarType type)
        {
            return type switch
            {
                AdvancedScrollbarType.BASIC => basic,
                _ => null,
            };
        }
    }
}
