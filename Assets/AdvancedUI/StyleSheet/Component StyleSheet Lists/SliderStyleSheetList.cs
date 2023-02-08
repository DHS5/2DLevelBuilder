using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public enum AdvancedSliderType
    {
        CUSTOM = -1,
        CLASSIC = 0,
        CLASSIC_W_TEXT = 1,
        NO_FILL = 2,
        NO_FILL_W_TEXT = 3,
        NO_BACK = 4,
        NO_BACK_W_TEXT = 5,

        BASIC_GRADIENT = 6,
    }

    [System.Serializable]
    public class SliderStyleSheetList
    {
        public SliderStyleSheet classic;
        public SliderStyleSheet classicWithText;
        public SliderStyleSheet noFill;
        public SliderStyleSheet noFillWithText;
        public SliderStyleSheet noBackground;
        public SliderStyleSheet noBackgroundWithText;
        [Space]
        public SliderStyleSheet basicGradient;


        public SliderStyleSheet GetStyleSheet(AdvancedSliderType type)
        {
            return type switch
            {
                AdvancedSliderType.CLASSIC => classic,
                AdvancedSliderType.CLASSIC_W_TEXT => classicWithText,
                AdvancedSliderType.NO_FILL => noFill,
                AdvancedSliderType.NO_FILL_W_TEXT => noFillWithText,
                AdvancedSliderType.NO_BACK => noBackground,
                AdvancedSliderType.NO_BACK_W_TEXT => noBackgroundWithText,

                AdvancedSliderType.BASIC_GRADIENT => basicGradient,
                _ => null,
            };
        }
    }
}
