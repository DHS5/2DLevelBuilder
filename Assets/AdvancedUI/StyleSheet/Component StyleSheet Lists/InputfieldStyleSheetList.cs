using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedInputfieldType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class InputfieldStyleSheetList
    {
        public InputfieldStyleSheet basic;


        public InputfieldStyleSheet GetStyleSheet(AdvancedInputfieldType type)
        {
            return type switch
            {
                AdvancedInputfieldType.BASIC => basic,
                _ => null,
            };
        }
    }
}
