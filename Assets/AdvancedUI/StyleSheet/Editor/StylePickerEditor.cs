using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Dhs5.AdvancedUI
{
    [CustomPropertyDrawer(typeof(StylePicker))]
    public class StylePickerEditor : PropertyDrawer
    {
        SerializedProperty styleSheetContainerP;
        SerializedObject containerObj;
        StyleSheetContainer container;
        StyleSheet styleSheet;

        private SerializedProperty styleSheetTypeP;
        private SerializedProperty styleSheetUIDP;

        int index = 0;
        StyleSheetType type;

        GUIContent empty = new GUIContent("");

        public int GetIndexByUniqueID(List<StyleSheetPlaceholder> styleSheets, int UID)
        {
            if (UID == 0) return -1;
            return styleSheets.FindIndex(v => v.UID == UID);
        }
        public int GetUniqueIDByIndex(List<StyleSheetPlaceholder> styleSheets, int index)
        {
            if (index < 0 || index >= styleSheets.Count) return 0;
            return styleSheets[index].UID;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            styleSheetTypeP = property.FindPropertyRelative("type");
            styleSheetUIDP = property.FindPropertyRelative("styleSheetUID");

            styleSheetContainerP = property.FindPropertyRelative("container");
            if (styleSheetContainerP.objectReferenceValue == null)
            {
                EditorGUI.LabelField(position, "StyleSheetContainer is missing !");
                EditorGUI.EndProperty();
                return;
            }
            // Get the StyleSheetContainer
            containerObj = new SerializedObject(styleSheetContainerP.objectReferenceValue);
            container = containerObj.targetObject as StyleSheetContainer;
            if (container == null)
            {
                EditorGUI.LabelField(position, "StyleSheetContainer is null !");
                EditorGUI.EndProperty();
                return;
            }

            // Get the project style sheet
            styleSheet = container.projectStyleSheet;
            if (styleSheet == null)
            {
                EditorGUI.LabelField(position, "Project StyleSheet is null !");
                EditorGUI.EndProperty();
                return;
            }

            // Style Sheet Type
            type = (StyleSheetType)styleSheetTypeP.enumValueIndex;

            // Style Sheet List
            List<StyleSheetPlaceholder> styleSheetList = container.GetStyleSheetByType(type);

            // Get index
            int indexSave = GetIndexByUniqueID(styleSheetList, styleSheetUIDP.intValue);
            if (indexSave == -1) indexSave = 0;

            // StyleSheet choice popup
            Rect popupPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            index = EditorGUI.Popup(popupPosition, property.FindPropertyRelative("name").stringValue, indexSave, container.StyleSheetNames(styleSheetList).ToArray());
            if (GetUniqueIDByIndex(styleSheetList, index) == 0) index = indexSave;
            styleSheetUIDP.intValue = GetUniqueIDByIndex(styleSheetList, index);
            
            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
