using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.AdvancedUI
{
    public static class EditorUtility
    {
        private static Object SSContainer
        {
            get 
            { 
                return AssetDatabase.LoadAssetAtPath("Assets/AdvancedUI/StyleSheet/StyleSheet Container.asset",
                typeof(StyleSheetContainer));
            }
        }

        [MenuItem("Window/Advanced UI/Style Sheet Container", priority = 0)]
        private static void GetStyleSheetContainer()
        {
            Selection.activeObject = SSContainer;
        }
        
        [MenuItem("Window/Advanced UI/Current Style Sheet", priority = 1)]
        private static void GetCurrentStyleSheet()
        {
            StyleSheetContainer container = SSContainer as StyleSheetContainer;
            Selection.activeObject = container.projectStyleSheet;
        }
    }
}
