using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dhs5.AdvancedUI
{
    /// <summary>
    /// StyleSheet Scriptable Object containing the style options for every AdvancedUI components
    /// </summary>
    [CreateAssetMenu(fileName = "StyleSheet Container", menuName = "AdvancedUI/StyleSheet/Container", order = 0)]
    public class StyleSheetContainer : ScriptableObject
    {
        public StyleSheet projectStyleSheet;
    }
}
