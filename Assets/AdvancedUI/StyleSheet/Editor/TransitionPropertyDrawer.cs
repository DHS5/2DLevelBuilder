using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.AdvancedUI
{
    [CustomPropertyDrawer(typeof(TransitionStyleSheet))]
    public class TransitionPropertyDrawer : PropertyDrawer
    {
        int transitionType = 0;
        int[] typesRectHeight = { 1, 9, 6, 7 };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);
            int lines = 1;
            if (property.isExpanded)
            {
                Rect rectType = new Rect(position.min.x, position.min.y + lines++ * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
                SerializedProperty transitionTypeProperty = property.FindPropertyRelative("transitionType");
                EditorGUI.PropertyField(rectType, transitionTypeProperty, true);
                transitionType = transitionTypeProperty.enumValueIndex;
                switch (transitionType)
                {
                    // Color Tint
                    case 1:
                        Rect rectColors = new Rect(position.min.x, position.min.y + (lines + 2) * EditorGUIUtility.singleLineHeight, 
                            position.size.x, EditorGUIUtility.singleLineHeight * typesRectHeight[transitionType]);
                        EditorGUI.PropertyField(rectColors, property.FindPropertyRelative("colorBlock"));
                        break;
                    // Sprite Swap
                    case 2:
                        Rect rectSprites = new Rect(position.min.x, position.min.y + (lines + 2) * EditorGUIUtility.singleLineHeight,
                            position.size.x, EditorGUIUtility.singleLineHeight * typesRectHeight[transitionType]);
                        EditorGUI.PropertyField(rectSprites, property.FindPropertyRelative("spriteState"));
                        break;
                    // Animation
                    case 3:
                        Rect rectAnimations = new Rect(position.min.x, position.min.y + (lines + 2) * EditorGUIUtility.singleLineHeight,
                            position.size.x, EditorGUIUtility.singleLineHeight * typesRectHeight[transitionType]);
                        EditorGUI.PropertyField(rectAnimations, property.FindPropertyRelative("animationTriggers"));
                        break;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
            {
                totalLines += typesRectHeight[transitionType] + 2;
            }

            return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}