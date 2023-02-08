using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedToggleType
    {
        CUSTOM = -1,
        BASIC = 0,
        BASIC_W_TEXT = 1,
        ONLY_CHECK = 2,
        ONLY_CHECK_W_TEXT = 3,
        CHECK_TEXTS = 4,
        CHECK_TEXTS_NO_BACK = 5,
        ONLY_CHECK_TEXT = 6,
        FULL_TEXTS = 7,
        FULL_TEXTS_NO_BACK = 8,
        CHECKS_NO_BACK = 9,
        CHECKS_NO_BACK_W_TEXT = 10,
    }

    [System.Serializable]
    public class ToggleStyleSheetList
    {
        public ToggleStyleSheet basic;
        public ToggleStyleSheet basicWithText;
        public ToggleStyleSheet checkOnly;
        public ToggleStyleSheet checkOnlyWithText;
        public ToggleStyleSheet checkTexts;
        public ToggleStyleSheet checkTextsNoBackground;
        public ToggleStyleSheet checkTextOnly;
        public ToggleStyleSheet fullTexts;
        public ToggleStyleSheet fullTextsNoBackground;
        public ToggleStyleSheet checksNoBackground;
        public ToggleStyleSheet checksNoBackgroundWithText;


        public ToggleStyleSheet GetStyleSheet(AdvancedToggleType type)
        {
            return type switch
            {
                AdvancedToggleType.BASIC => basic,
                AdvancedToggleType.BASIC_W_TEXT => basicWithText,
                AdvancedToggleType.ONLY_CHECK => checkOnly,
                AdvancedToggleType.ONLY_CHECK_W_TEXT => checkOnlyWithText,
                AdvancedToggleType.CHECK_TEXTS => checkTexts,
                AdvancedToggleType.CHECK_TEXTS_NO_BACK => checkTextsNoBackground,
                AdvancedToggleType.ONLY_CHECK_TEXT => checkTextOnly,
                AdvancedToggleType.FULL_TEXTS => fullTexts,
                AdvancedToggleType.FULL_TEXTS_NO_BACK => fullTextsNoBackground,
                AdvancedToggleType.CHECKS_NO_BACK => checksNoBackground,
                AdvancedToggleType.CHECKS_NO_BACK_W_TEXT => checksNoBackgroundWithText,
                _ => null,
            };
        }
    }
}
