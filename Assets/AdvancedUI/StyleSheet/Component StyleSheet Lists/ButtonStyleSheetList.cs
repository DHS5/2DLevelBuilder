using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedButtonType
    {
        CUSTOM = -1,

        TEXT = 0,
        ONLY_TEXT = 1,
        ONLY_ICON = 2,
        ONLY_BACKGROUND = 3,
        BACK_AND_ICON = 4,

        QUIT = 5,
        BACK = 6,
        INFO = 7,
        QUESTION = 8,
        WARNING = 9,
        IMPORTANT_ANSWER = 10,
    }

    [System.Serializable]
    public class ButtonStyleSheetList
    {
        public ButtonStyleSheet text;
        public ButtonStyleSheet textOnly;
        public ButtonStyleSheet iconOnly;
        public ButtonStyleSheet backgroundOnly;
        public ButtonStyleSheet backAndIcon;
        [Space]
        public ButtonStyleSheet quit;
        public ButtonStyleSheet back;
        public ButtonStyleSheet info;
        public ButtonStyleSheet question;
        public ButtonStyleSheet warning;
        public ButtonStyleSheet importantAnswer;


        public ButtonStyleSheet GetStyleSheet(AdvancedButtonType type)
        {
            return type switch
            {
                AdvancedButtonType.TEXT => text,
                AdvancedButtonType.ONLY_TEXT => textOnly,
                AdvancedButtonType.ONLY_ICON => iconOnly,
                AdvancedButtonType.ONLY_BACKGROUND => backgroundOnly,
                AdvancedButtonType.BACK_AND_ICON => backAndIcon,
                AdvancedButtonType.QUIT => quit,
                AdvancedButtonType.BACK => back,
                AdvancedButtonType.INFO => info,
                AdvancedButtonType.QUESTION => question,
                AdvancedButtonType.WARNING => warning,
                AdvancedButtonType.IMPORTANT_ANSWER => importantAnswer,
                _ => null,
            };
        }
    }
}
