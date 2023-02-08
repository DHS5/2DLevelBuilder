using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum PopupType
    {
        CUSTOM = -1,

        INFO = 0,
        QUESTION = 1,
        WARNING = 2,
        CONFIRMATION = 3,
    }

    [System.Serializable]
    public class PopupStyleSheetList
    {
        public PopupStyleSheet info;
        public PopupStyleSheet question;
        public PopupStyleSheet warning;
        public PopupStyleSheet confirmation;


        public PopupStyleSheet GetStyleSheet(PopupType type)
        {
            return type switch
            {
                PopupType.INFO => info,
                PopupType.QUESTION => question,
                PopupType.WARNING => warning,
                PopupType.CONFIRMATION => confirmation,
                _ => null,
            };
        }
    }
}
