using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

namespace LevelBuilder2D
{
    [System.Serializable]
    public class LevelTilemap
    {
        public BoundsInt bounds;
        public Vector4[] tiles;

        public LevelTilemap(BoundsInt _bounds, Vector4[] _tiles)
        {
            bounds = _bounds;
            tiles = _tiles;
        }
        public LevelTilemap(string str)
        {
            Deserialize(str);
        }

        public string Serialize()
        {
            StringBuilder builder = new();
            builder.Append("{");
            builder.Append($"[{bounds.min.x},{bounds.min.y},{bounds.max.x},{bounds.max.y}]");
            for (int i = 0; i < tiles.Length; i++)
            {
                builder.Append($"[{tiles[i].x},{tiles[i].y},{tiles[i].z},{tiles[i].w}]");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public void Deserialize(string str)
        {
            char[] separators = { '[', ']', '{', '}' };
            string[] parts = str.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            GetBounds(parts[0].Split(","));
            GetTiles(parts);
        }

        private void GetBounds(string[] strings)
        {
            bounds.min = new Vector3Int(int.Parse(strings[0]),int.Parse(strings[1]),0);
            bounds.max = new Vector3Int(int.Parse(strings[2]),int.Parse(strings[3]),0);
        }

        private void GetTiles(string[] strings)
        {
            tiles = new Vector4[strings.Length - 1];
            string[] str;

            for (int i = 0; i < strings.Length - 1; i++)
            {
                str = strings[i+1].Split(",");
                tiles[i] = new Vector4(int.Parse(str[0]), int.Parse(str[1]), int.Parse(str[2]), int.Parse(str[3]));
            }
        }
    }
}
