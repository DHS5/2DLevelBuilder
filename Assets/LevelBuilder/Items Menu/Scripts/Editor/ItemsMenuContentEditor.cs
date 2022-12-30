using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace LevelBuilder2D
{
    [CustomEditor(typeof(ItemsMenuContent))]
    [CanEditMultipleObjects]
    public class ItemsMenuContentEditor : Editor
    {
        ItemsMenuContent menuContent;
        ItemsMenuTemplate template;

        bool[] showFoldouts;
        Object tileObj = null;


        private void OnEnable()
        {
            menuContent = (ItemsMenuContent)serializedObject.targetObject;
            template = menuContent.template;

            showFoldouts = new bool[template.categories.Length];

            menuContent.BuildCategories();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            base.OnInspectorGUI();

            int catNum;
            int i;
            Object obj;
            
            if (menuContent.template != null)
            {
                EditorGUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                for (int c = 0; c < template.categories.Length; c++)
                {
                    ItemsMenuCategory category = template.categories[c];
                    catNum = category.categoryNumber;
                    
                    showFoldouts[c] = EditorGUILayout.Foldout(showFoldouts[c], category.categoryName, true);
                    if (showFoldouts[c])
                    {
                        foreach (ItemTemplate item in category.items)
                        {
                            i = item.number;
                            obj = menuContent.categories[catNum].tiles[i];
                            tileObj = EditorGUILayout.ObjectField(item.name, obj, typeof(TileBase), false);

                            TileBase tile = tileObj as TileBase;            
                            menuContent.categories[catNum].tiles[i] = tile;
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }

            EditorUtility.SetDirty(target);
        }
    }
}
