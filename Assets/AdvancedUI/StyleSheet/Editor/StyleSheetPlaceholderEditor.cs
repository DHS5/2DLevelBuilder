using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.AdvancedUI
{
    [CustomPropertyDrawer(typeof(StyleSheetPlaceholder))]
    public class StyleSheetPlaceholderEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width * 0.88f, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("name"), new GUIContent(""));
            EditorGUI.LabelField(new Rect(position.x + position.width * 0.9f, position.y, position.width * 0.10f, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("UID").intValue.ToString());

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.2f;
        }
    }
}
