using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public class TilemapSave
    {
        private struct TileSave
        {
            public TileSave(int x, int y, TileBase _tile)
            {
                pos = new Vector2Int(x, y);
                tile = _tile;
            }

            public TileBase tile;
            public Vector2Int pos;
        }

        List<TileSave> tiles;
        Tilemap tilemap;

        public TilemapSave(Tilemap _tilemap)
        {
            tilemap = _tilemap;
            Save(_tilemap);
        }

        private void Save(Tilemap tilemap)
        {
            tiles = new();

            TileBase tile;
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    tile = tilemap.GetTile(x, y);
                    if (tile != null)
                    {
                        tiles.Add(new TileSave(x, y, tile));
                    }
                }
            }
        }

        public void Get()
        {
            tilemap.ClearAllTiles();

            foreach (TileSave tileSave in tiles)
            {
                tilemap.SetTile(tileSave.pos, tileSave.tile);
            }
        }
    }
}
