using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dhs5.Utility.SaveSystem;

namespace Dhs5.AdvancedUI
{
    public static class EditorUtility
    {
        private static Object SSContainer
        {
            get 
            {
                Object container = AssetDatabase.LoadAssetAtPath("Assets/AdvancedUI/StyleSheet/StyleSheet Container.asset",
                typeof(StyleSheetContainer));
                if (container == null)
                {
                    container = ScriptableObject.CreateInstance<StyleSheetContainer>();
                    AssetDatabase.CreateAsset(container, "Assets/AdvancedUI/StyleSheet/StyleSheet Container.asset");
                }
                return container;
            }
        }

        [MenuItem("Window/Advanced UI/Style Sheet Container", priority = 0)]
        private static void GetStyleSheetContainer()
        {
            Selection.activeObject = SSContainer;
        }
        
        [MenuItem("Window/Advanced UI/Current Style Sheet %&S", priority = 1)]
        private static void GetCurrentStyleSheet()
        {
            StyleSheetContainer container = SSContainer as StyleSheetContainer;
            if (container.projectStyleSheet != null)
                Selection.activeObject = container.projectStyleSheet;
        }
    }
}
