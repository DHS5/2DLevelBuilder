using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedScrollViewType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class ScrollViewStyleSheetList
    {
        public ScrollViewStyleSheet basic;


        public ScrollViewStyleSheet GetStyleSheet(AdvancedScrollViewType type)
        {
            return type switch
            {
                AdvancedScrollViewType.BASIC => basic,
                _ => null,
            };
        }
    }
}
