using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum SwitchToggleType
    {
        CUSTOM = -1,
        BASIC = 0,
    }

    [System.Serializable]
    public class SwitchToggleStyleSheetList
    {
        public SwitchToggleStyleSheet basic;


        public SwitchToggleStyleSheet GetStyleSheet(SwitchToggleType type)
        {
            return type switch
            {
                SwitchToggleType.BASIC => basic,
                _ => null,
            };
        }
    }
}
