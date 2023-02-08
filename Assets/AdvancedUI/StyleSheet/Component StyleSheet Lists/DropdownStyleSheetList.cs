using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedDropdownType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class DropdownStyleSheetList
    {
        public DropdownStyleSheet basic;


        public DropdownStyleSheet GetStyleSheet(AdvancedDropdownType type)
        {
            return type switch
            {
                AdvancedDropdownType.BASIC => basic,
                _ => null,
            };
        }
    }
}
