using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Dhs5.Utility.SaveSystem;

namespace LevelBuilder2D
{
    [System.Serializable]
    public class Level : SaveClass
    {
        [Tooltip("Name of the level")]
        public string name;
        [Tooltip("Style index of the level")]
        public int style;
        [Tooltip("Array of the level tilemaps")]
        public LevelTilemap[] levelTilemaps;

        public Level(LevelTilemap[] _levelTilemaps, string _name, int _style)
        {
            levelTilemaps = _levelTilemaps;
            name = _name;
            style = _style;
        }
        public Level(string str)
        {
            Deserialize(str);
        }
        public Level() 
        {
            name = "Empty";
            style = 0;
            levelTilemaps = new LevelTilemap[0];
        }

        public string Serialize()
        {
            StringBuilder builder = new();

            builder.Append(name);
            builder.Append(";");
            builder.Append(style);
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
            style = int.Parse(parts[1]);

            levelTilemaps = new LevelTilemap[parts.Length - 2];
            for (int i = 2; i < parts.Length; i++)
            {
                levelTilemaps[i-2] = new LevelTilemap(parts[i]);
            }
        }

        protected override void CopyOperation(SaveClass saveClass)
        {
            Level other = saveClass as Level;

            levelTilemaps = other.levelTilemaps;
            name = other.name;
            style = other.style;
        }

        public override void Save(string filename, string filepath, string extension)
        {
            string serializedLevel = Serialize();

            SaveSystem.SaveText(serializedLevel, filename, extension, filepath);
        }

        public override bool TryLoad<T>(string filename, string filepath, string extension)
        {
            bool result = SaveSystem.TryLoadText(out string serializedLevel, filename, extension, filepath);
            if (result) Deserialize(serializedLevel);
            return result;
        }
    }
}
