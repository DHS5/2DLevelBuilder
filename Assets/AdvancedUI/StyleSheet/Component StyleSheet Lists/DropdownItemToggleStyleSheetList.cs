using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum DropdownItemToggleType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class DropdownItemToggleStyleSheetList
    {
        public DropdownItemToggleStyleSheet basic;


        public DropdownItemToggleStyleSheet GetStyleSheet(DropdownItemToggleType type)
        {
            return type switch
            {
                DropdownItemToggleType.BASIC => basic,
                _ => null,
            };
        }
    }
}
