using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum ScrollListType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class ScrollListStyleSheetList
    {
        public ScrollListStyleSheet basic;


        public ScrollListStyleSheet GetStyleSheet(ScrollListType type)
        {
            return type switch
            {
                ScrollListType.BASIC => basic,
                _ => null,
            };
        }
    }
}
