using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dhs5.Utility.Tilemaps
{
    public static class TilemapUtility
    {

        #region Extension Methods

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

        #endregion
    }
}
