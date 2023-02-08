using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.AdvancedUI
{
    [CustomPropertyDrawer(typeof(GraphicStyleSheet))]
    public class GraphicStyleSheetPropertyDrawer : PropertyDrawer
    {
        bool graphicIsImage = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);
            int lines = 1;
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect rectType = new Rect(position.min.x, position.min.y + lines++ * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
                SerializedProperty graphicTypeProperty = property.FindPropertyRelative("isImage");
                EditorGUI.PropertyField(rectType, graphicTypeProperty);
                graphicIsImage = graphicTypeProperty.boolValue;

                if (graphicIsImage)
                {
                    Rect rectImage = new Rect(position.min.x, position.min.y + lines * EditorGUIUtility.singleLineHeight,
                            position.size.x, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(rectImage, property.FindPropertyRelative("imageStyleSheet"), true);
                }
                else
                {
                    Rect rectText = new Rect(position.min.x, position.min.y + lines * EditorGUIUtility.singleLineHeight,
                            position.size.x, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(rectText, property.FindPropertyRelative("textStyleSheet"), true);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;
            float height = 0;
            
            if (property.isExpanded)
            {
                totalLines++;
                if (graphicIsImage)
                {
                    SerializedProperty imageProperty = property.FindPropertyRelative("imageStyleSheet");
                    height = EditorGUI.GetPropertyHeight(imageProperty);
                }
                else
                {
                    SerializedProperty textProperty = property.FindPropertyRelative("textStyleSheet");
                    height = EditorGUI.GetPropertyHeight(textProperty);
                }
            }
            
            return EditorGUIUtility.singleLineHeight * totalLines + height;
        }
    }
}
