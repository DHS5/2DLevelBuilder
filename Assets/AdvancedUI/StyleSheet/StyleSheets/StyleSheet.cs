using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    /// <summary>
    /// StyleSheet Scriptable Object containing the style options for every AdvancedUI components
    /// </summary>
    [CreateAssetMenu(fileName = "StyleSheet", menuName = "AdvancedUI/StyleSheet/StyleSheet", order = 1)]
    public class StyleSheet : ScriptableObject
    {
        [Header("Texts")]
        public TextStyleSheetList textStyleSheets;
        [Space, Space]
        [Header("Buttons")]
        public ButtonStyleSheetList buttonStyleSheets;
        [Space, Space]
        [Header("Toggle")]
        public ToggleStyleSheetList toggleStyleSheets;
        public DropdownItemToggleStyleSheetList dropdownItemToggleStyleSheets;
        public SwitchToggleStyleSheetList switchToggleStyleSheets;
        [Space, Space]
        [Header("Slider")]
        public SliderStyleSheetList sliderStyleSheets;
        [Space, Space]
        [Header("Dropdown")]
        public DropdownStyleSheetList dropdownStyleSheets;
        [Space, Space]
        [Header("InputField")]
        public InputfieldStyleSheetList inputfieldStyleSheets;
        [Space, Space]
        [Header("ScrollBar")]
        public ScrollbarStyleSheetList scrollbarStyleSheets;
        [Space, Space]
        [Header("Popup")]
        public PopupStyleSheetList popupStyleSheets;
        [Space, Space]
        [Header("ScrollView")]
        public ScrollViewStyleSheetList scrollViewStyleSheets;
        [Space, Space]
        [Header("ScrollList")]
        public ScrollListStyleSheetList scrollListStyleSheets;
    }

    #region Composite Style Sheets

    [System.Serializable]
    public class TransitionStyleSheet
    {
        [Header("Transition Type")]
        public Selectable.Transition transitionType;

        [Header("Sprite Transition")]
        public SpriteState spriteState;

        [Header("Color Transition")]
        public ColorBlock colorBlock;

        [Header("Animation Transition")]
        public AnimationTriggers animationTriggers = new();
    }

    [System.Serializable]
    public class ImageStyleSheet
    {
        public Sprite baseSprite;
        public Color baseColor = Color.white;
        public Material baseMaterial;
        [Space]
        public Image.Type imageType;
        [Range(0, 10)] public float pixelsPerUnit = 1;
        [Header("Transition")]
        public bool isStatic = true;
        public TransitionStyleSheet transition;
    }

    [System.Serializable]
    public struct GradientTransition
    {
        public VertexGradient normalGradient;
        public VertexGradient highlightedGradient;
        public VertexGradient pressedGradient;
        public VertexGradient selectedGradient;
        public VertexGradient disabledGradient;
    }

    [System.Serializable]
    public class TextStyleSheet
    {
        public TMP_FontAsset font;
        public FontStyles fontStyle;
        public bool overrideAlignment;
        public TextAlignmentOptions alignment;

        [Header("Color")]
        public bool isGradient = false; bool IsNotGradient => !isGradient;
        public bool isStatic = true; bool IsNotStatic => !isStatic;

        [ShowIf(EConditionOperator.And, nameof(IsNotGradient), nameof(isStatic))][AllowNesting] 
        public Color color = Color.black;
        [ShowIf(EConditionOperator.And, nameof(isGradient), nameof(isStatic))][AllowNesting] 
        public VertexGradient colorGradient;

        [ShowIf(EConditionOperator.And, nameof(IsNotGradient), nameof(IsNotStatic))][AllowNesting] 
        public ColorBlock colorTransition;
        [ShowIf(EConditionOperator.And, nameof(isGradient), nameof(IsNotStatic))][AllowNesting]
        public GradientTransition gradientTransition;
    }

    [System.Serializable]
    public class GraphicStyleSheet
    {
        public bool isImage;
        public ImageStyleSheet imageStyleSheet;
        public TextStyleSheet textStyleSheet;
    }

    #endregion
}
