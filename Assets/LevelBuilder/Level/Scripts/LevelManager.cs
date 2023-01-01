using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelBuilder2D
{
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

            level = new(levelTilemaps, levelName);

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


        // ### Save ###

        public static void SaveLevelToDisk(Level level)
        {
            string serializedLevel = level.Serialize();

            if (!Directory.Exists(diskSavePath))
            {
                Directory.CreateDirectory(diskSavePath);
            }

            File.WriteAllText(diskSavePath + level.name + ".txt", serializedLevel);
        }

        public static void CreateLevelOnDisk(Level level)
        {
            SaveLevelToDisk(level);

            List<string> nameList;
            string[] currentList = GetLevelList();

            if (currentList != null)
                nameList = new(currentList);
            else
                nameList = new();

            nameList.Add(level.name);

            SetLevelList(nameList.ToArray());
        }

        // ### Load ###

        public static Level LoadLevelFromDisk(string filename)
        {
            Level level = null;
            string serializedLevel;
            string path = diskSavePath + filename + ".txt";

            if (File.Exists(path))
            {
                serializedLevel = File.ReadAllText(path);
                level = new Level(serializedLevel);
            }

            return level;
        }

        // ### Delete ###

        public static void DeleteLevelFromDisk(string levelName)
        {
            DeleteLevelInList(levelName);

            string path = diskSavePath + levelName + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

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
            levelToModify.level = new(newLevel.levelTilemaps, newLevel.name);
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

        


        // ### Level List ###

        [System.Serializable]
        private class LevelList
        {
            public LevelList(string[] _list) { list = _list; }

            public string[] list;
        }

        /// <summary>
        /// Get an array of levels name
        /// </summary>
        /// <returns>Array of levels name OR null</returns>
        public static string[] GetLevelList()
        {
            if (File.Exists(listSavePath))
            {
                string json = File.ReadAllText(listSavePath);
                LevelList levelList = JsonUtility.FromJson<LevelList>(json);

                return levelList.list;
            }

            return null;
        }

        /// <summary>
        /// Sets the array of levels name
        /// </summary>
        /// <param name="levels">Array of levels name</param>
        private static void SetLevelList(string[] levels)
        {
            LevelList levelList = new(levels);

            string json = JsonUtility.ToJson(levelList);

            File.WriteAllText(listSavePath, json);
        }


        private static void DeleteLevelInList(string levelToDelete)
        {
            string[] array = GetLevelList();

            if (array == null) return;

            List<string> list = new(array);
            list.Remove(levelToDelete);

            SetLevelList(list.ToArray());
        }


        // ### Extension Methods ###

        public static Sprite GetSprite(this TileBase tileBase)
        {
            if (tileBase is Tile) return (tileBase as Tile).sprite;
            if (tileBase is RuleTile) return (tileBase as RuleTile).m_DefaultSprite;
            return null;
        }

        /// <summary>
        /// Fills a rectangle on a tilemap
        /// </summary>
        /// <param name="self">The tilemap to be filled</param>
        /// <param name="tile">The tile to fill the box with</param>
        /// <param name="startPosition">The lower left corner of the box</param>
        /// <param name="endPosition">The upper right corner of the box</param>
        public static void BoxFill(this Tilemap self, TileBase tile, Vector3Int start, Vector3Int end)
        {
            //Determine directions on X and Y axis
            var xDir = start.x < end.x ? 1 : -1;
            var yDir = start.y < end.y ? 1 : -1;
            //How many tiles on each axis?
            int xCols = 1 + Mathf.Abs(start.x - end.x);
            int yCols = 1 + Mathf.Abs(start.y - end.y);
            //Start painting
            for (var x = 0; x < xCols; x++)
            {
                for (var y = 0; y < yCols; y++)
                {
                    var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                    self.SetTile(tilePos, tile);
                }
            }
        }


        public static TileBase GetTile(this Tilemap tilemap, int x, int y)
        {
            return tilemap.GetTile(new Vector3Int(x, y, 0));
        }
        public static TileBase GetTile(this Tilemap tilemap, Vector2Int pos)
        {
            return tilemap.GetTile(new Vector3Int(pos.x, pos.y, 0));
        }
        public static void SetTile(this Tilemap tilemap, Vector2Int pos, TileBase tile)
        {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tile);
        }
    }
}
