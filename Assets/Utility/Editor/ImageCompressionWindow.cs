using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dhs5.Utility.EditorTools
{
    public class ImageCompressionWindow : EditorWindow
    {
        [System.Serializable]
        public enum MaxTextureSize { S32 = 32, S64 = 64, S128 = 128, S256 = 256, S512 = 512, S1024 = 1024, S2048 = 2048 }

        // ### Properties ###
        MaxTextureSize maxTextureSize = MaxTextureSize.S512;
        TextureImporterCompression compressionType = TextureImporterCompression.Compressed;
        bool crunchCompression = true;
        int compressionQuality = 50;

        Object folder = null;
        string folderPath = "";


        [MenuItem("Window/Dhs5 Utility/Image Compression")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ImageCompressionWindow));
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Compression Format", EditorStyles.boldLabel);
            EditorGUILayout.Space(15);

            maxTextureSize = (MaxTextureSize)EditorGUILayout.EnumPopup("Max Texture Size : ", maxTextureSize, GUILayout.MinWidth(300));
            compressionType = (TextureImporterCompression)EditorGUILayout.EnumPopup("Compression Type : ", compressionType, GUILayout.MinWidth(300));
            crunchCompression = EditorGUILayout.Toggle("Crunch Compression : ", crunchCompression);
            compressionQuality = EditorGUILayout.IntSlider("Compression Quality : ", compressionQuality, 0, 100);

            EditorGUILayout.Space(50);

            folder = EditorGUILayout.ObjectField("Select a folder : ", folder, typeof(Object), false);
            if (folder != null)
            {
                if (Directory.Exists(AssetDatabase.GetAssetPath(folder)))
                {
                    folderPath = AssetDatabase.GetAssetPath(folder);
                }
                else
                {
                    folder = null;
                    folderPath = "";
                }
            }
            if (folderPath != "" && GUILayout.Button("Compress Images in Folder"))
            {
                if (folderPath != "" && Directory.Exists(folderPath))
                {
                    CompressImageInFolder(folderPath);
                }
                else
                {
                    Debug.LogError("Folder path not valid, make sure to select a folder in the object field");
                }
            }

            EditorGUILayout.EndVertical();
        }


        private void CompressImageInFolder(string path)
        {
            foreach (var t in AssetDatabase.FindAssets("t:texture2D", new[] { path }))
            {
                Debug.Log(AssetDatabase.GUIDToAssetPath(t));
                TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(t));

                textureImporter.isReadable = true;
                if (textureImporter.textureType == TextureImporterType.Sprite)
                {
                    textureImporter.maxTextureSize = (int)maxTextureSize;
                    textureImporter.crunchedCompression = crunchCompression;
                    textureImporter.textureCompression = compressionType;
                    textureImporter.compressionQuality = compressionQuality;

                    EditorUtility.SetDirty(textureImporter);
                    textureImporter.SaveAndReimport();
                }
            }
        }
    }
}
