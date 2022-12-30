using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace LevelBuilder2D
{
    [System.Serializable]
    public class Level
    {
        [Tooltip("Name of the level")]
        public string name;
        [Tooltip("Array of the level tilemaps")]
        public LevelTilemap[] levelTilemaps;

        public Level(LevelTilemap[] _levelTilemaps, string _name)
        {
            levelTilemaps = _levelTilemaps;
            name = _name;
        }
        public Level(string str)
        {
            Deserialize(str);
        }

        public string Serialize()
        {
            StringBuilder builder = new();

            builder.Append(name);
            foreach (var levelTilemap in levelTilemaps)
            {
                builder.Append(";");
                builder.Append(levelTilemap.Serialize());
            }

            return builder.ToString();
        }

        public void Deserialize(string str)
        {
            string[] parts = str.Split(";", System.StringSplitOptions.RemoveEmptyEntries);

            name = parts[0];

            levelTilemaps = new LevelTilemap[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++)
            {
                levelTilemaps[i-1] = new LevelTilemap(parts[i]);
            }
        }
    }
}
