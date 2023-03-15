using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum TextType
    {
        CUSTOM = -1,
        BASIC_BLACK = 0,
        BASIC_WHITE = 1,
        BASIC_BUTTON = 2,
    }

    [System.Serializable]
    public class TextStyleSheetList
    {
        public TextStyleSheet basicBlack;
        public TextStyleSheet basicWhite;
        public TextStyleSheet basicButton;

        public TextStyleSheet GetStyleSheet(TextType type)
        {
            return type switch
            {
                TextType.BASIC_BLACK => basicBlack,
                TextType.BASIC_WHITE => basicWhite,
                TextType.BASIC_BUTTON => basicButton,
                _ => null,
            };
        }
    }
}
