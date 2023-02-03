using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using Dhs5.Utility.SaveSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelBuilder2D
{
    /// <summary>
    /// Static class handling :
    /// - the conversion of tilemaps into saveable level class (and vice-versa)
    /// - saving of levels
    /// - loading of levels
    /// </summary>
    public static class LevelManager
    {
        public readonly static string assetSavePath = "Assets/LevelBuilder/Level/Level Saves/";
        public readonly static string assetListSavePath = assetSavePath + "Level List.asset";
        public readonly static string diskSavePath = Application.persistentDataPath + "/Levels/";
        public readonly static string listSavePath = diskSavePath + "LevelList.json";

        // ### Calculations ###

        public static Level TilemapsToLevel(Tilemap[] tilemaps, ItemsMenuContent menuContent, string levelName)
        {
            Vector3Int currentPos;
            int length = tilemaps.Length;
            BoundsInt bounds;
            // x, y = pos ; z, w = type (z = category / w = item number)
            List<Vector4> tiles = new();
            Vector2 tileType;
            LevelTilemap[] levelTilemaps = new LevelTilemap[length];
            Level level;
            Dictionary<TileBase, Vector2> dico = menuContent.BuildDictionnary();

            // Iterate through each tilemap
            for (int t = 0; t < length; t++)
            {
                tilemaps[t].CompressBounds();
                bounds = tilemaps[t].cellBounds;
                tiles.Clear();

                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        currentPos = new Vector3Int(x, y, 0);
                        if (tilemaps[t].HasTile(currentPos))
                        {
                            tileType = dico[tilemaps[t].GetTile(currentPos)];
                            tiles.Add(new Vector4(x, y, tileType.x, tileType.y));
                        }
                    }
                }

                levelTilemaps[t] = new(bounds, tiles.ToArray());
            }

            level = new(levelTilemaps, levelName, menuContent.styleIndex);

            return level;
        }


        public static void LevelToTilemaps(Level level, Tilemap[] tilemaps, ItemsMenuContent menuContent)
        {
            int length = level.levelTilemaps.Length;
            LevelTilemap levelTilemap;
            Vector4 tile;
            Vector3Int pos;

            for (int t = 0; t < length; t++)
            {
                tilemaps[t].ClearAllTiles();
                levelTilemap = level.levelTilemaps[t];
                if (levelTilemap != null)
                {
                    for (int i = 0; i < levelTilemap.tiles.Length; i++)
                    {
                        tile = levelTilemap.tiles[i];
                        pos = new Vector3Int((int)tile.x, (int)tile.y, 0);
                        tilemaps[t].SetTile(pos, menuContent.categories[(int)tile.z].tiles[(int)tile.w]);
                    }
                }
            }
        }


        #region Level Saving (Disk)

        private static SavesRepertory<Level, LevelInfo> levelSaves;

        private static SavesRepertory<Level, LevelInfo> LevelSaves
        {
            get
            {
                if (levelSaves == null) levelSaves = new("LevelSaves", "/Levels/", ".txt");
                return levelSaves;
            }
        }

        // ### Save ###

        public static void SaveLevelToDisk(Level level)
        {
            LevelSaves.Add(level.name, level);
        }

        // ### Load ###

        public static Level LoadLevelFromDisk(string filename)
        {
            return LevelSaves.GetSave(filename);
        }

        // ### Delete ###

        public static void DeleteLevelFromDisk(LevelInfo levelInfo)
        {
            LevelSaves.Remove(levelInfo);
        }

        public static List<LevelInfo> GetLevelList()
        {
            return LevelSaves.GetInfosList();
        }
        public static List<string> GetLevelNamesList()
        {
            return LevelSaves.GetInfosNameList();
        }

        #endregion

        #region Level Saving (Assets)

        // ### Assets (Editor Only) ###
#if UNITY_EDITOR
        public static LevelSO InstanciateLevelSO(Level level)
        {
            LevelSO levelSO = ScriptableObject.CreateInstance<LevelSO>();
            levelSO.level = level;

            string completePath = assetSavePath + level.name + ".asset";

            AssetDatabase.CreateAsset(levelSO, completePath);

            LevelListSO levelList = AssetDatabase.LoadAssetAtPath<LevelListSO>(assetListSavePath);
            levelList.levels.Add(levelSO);
            EditorUtility.SetDirty(levelList);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = levelSO;

            return levelSO;
        }

        public static void ModifyLevelSO(LevelSO levelToModify, Level newLevel)
        {
            levelToModify.level = new(newLevel.levelTilemaps, newLevel.name, newLevel.style);
            EditorUtility.SetDirty(levelToModify);
        }

        public static void DeleteLevelSO(LevelSO levelSO, LevelListSO levelList)
        {
            levelList.levels.Remove(levelSO);
            EditorUtility.SetDirty(levelList);

            AssetDatabase.DeleteAsset(assetSavePath + levelSO.level.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static void DeleteLevelSO(LevelSO levelSO)
        {
            LevelListSO levelList = AssetDatabase.LoadAssetAtPath<LevelListSO>(assetListSavePath);
            DeleteLevelSO(levelSO, levelList);
        }

        public static LevelSO GetLevelSOInList(int index)
        {
            LevelListSO levelList = AssetDatabase.LoadAssetAtPath<LevelListSO>(assetListSavePath);
            return levelList.levels[index];
        }
#endif

        #endregion
    }

    [System.Serializable]
    public class LevelInfo : SaveInfo<Level>
    {
        public LevelInfo() { }
        public LevelInfo(string name, Level save, string path = "/Levels/", string extension = ".txt")
            : base(name, save, path, extension)
        {
            levelStyleIndex = save.style;
        }
        public override void SetUp(string name, Level save, string path = "/", string extension = ".json")
        {
            base.SetUp(name, save, path, extension);

            levelStyleIndex = save.style;
        }

        public int levelStyleIndex;
    }
}
