using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class AdvancedText : AdvancedComponent
    {
        [Header("Text Type")]
        [SerializeField] private TextType textType;
        public TextType Type { get { return textType; } set { textType = value; } }

        [Header("Style Sheet")]
        [SerializeField] private TextStyleSheet customStyleSheet;

        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI text;

        public override bool Interactable { get => true; set => SetUpConfig(); }

        protected override void LinkEvents() { }
        protected override void UnlinkEvents() { }

        protected override void SetUpConfig()
        {
            if (styleSheetContainer == null) return;

            if (text)
                text.SetUpText(Type == TextType.CUSTOM ? customStyleSheet : GetTextStyleSheet(Type));
        }
    }
}
