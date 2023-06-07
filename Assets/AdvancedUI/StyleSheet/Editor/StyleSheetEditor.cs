using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Tilemaps;

namespace Dhs5.AdvancedUI
{
    [CustomEditor(typeof(StyleSheet))]
    [CanEditMultipleObjects]
    public class StyleSheetEditor : Editor
    {
        StyleSheet styleSheet;
        StyleSheetContainer container;

        List<ReorderableList> reorderableLists = new();

        ReorderableList list;

        private void CreateReorderableList(string listPropertyName, List<StyleSheetPlaceholder> placeholderList, string displayName)
        {
            SerializedProperty textList = serializedObject.FindProperty(listPropertyName);
            list = new ReorderableList(serializedObject, textList, false, true, false, false)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, displayName);
                },

                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = textList.GetArrayElementAtIndex(index);

                    EditorGUI.indentLevel++;
                    EditorGUI.PropertyField(rect, element, new GUIContent(placeholderList[index].Name), true);
                    EditorGUI.indentLevel--;
                },

                elementHeightCallback = index =>
                {
                    var element = textList.GetArrayElementAtIndex(index);

                    return EditorGUI.GetPropertyHeight(element);
                }
            };

            reorderableLists.Add(list);
        }

        private void OnEnable()
        {
            styleSheet = (StyleSheet)serializedObject.targetObject;
            container = styleSheet.Container;

            styleSheet.ApplyTemplate();

            CreateReorderableList("TextStyleSheets", container.Texts, "Texts");
            CreateReorderableList("BackgroundImageStyleSheets", container.Backgrounds, "Backgrounds");
            CreateReorderableList("IconImageStyleSheets", container.Icons, "Icons");
            CreateReorderableList("ButtonStyleSheets", container.Buttons, "Buttons");
            CreateReorderableList("ToggleStyleSheets", container.Toggles, "Toggles");
            CreateReorderableList("DropdownItemToggleStyleSheets", container.DropdownItems, "Dropdown Item Toggles");
            CreateReorderableList("SwitchToggleStyleSheets", container.Switchs, "Switch Toggles");
            CreateReorderableList("SliderStyleSheets", container.Sliders, "Sliders");
            CreateReorderableList("DropdownStyleSheets", container.Dropdowns, "Dropdowns");
            CreateReorderableList("InputfieldStyleSheets", container.InputFields, "Input Fields");
            CreateReorderableList("ScrollbarStyleSheets", container.Scrollbars, "Scrollbars");
            CreateReorderableList("ScrollViewStyleSheets", container.ScrollViews, "Scroll Views");
            CreateReorderableList("ScrollListStyleSheets", container.ScrollLists, "Scroll Lists");
            CreateReorderableList("PopupStyleSheets", container.Popups, "Popups");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            //base.OnInspectorGUI();

            if (container == null) return;
            
            EditorGUILayout.Space(15);
            EditorGUILayout.BeginVertical();

            foreach (var list in reorderableLists)
                list.DoLayoutList();
            
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
            UnityEditor.EditorUtility.SetDirty(target);
        }
    }
}
