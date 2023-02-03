using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Dhs5.Utility.Tilemaps;

namespace LevelBuilder2D
{
    public class FillCommand : TilemapCommandManager.ICommand
    {
        private Vector3Int pos;
        private Tilemap tilemap;
        private TileBase tile;

        private TilemapSave formerLevelTilemap;

        public FillCommand(Vector3Int _pos, Tilemap _tilemap, TileBase _tile)
        {
            pos = _pos;
            tilemap = _tilemap;
            tile = _tile;
        }

        public bool Execute()
        {
            if (tilemap.GetTile(pos.x, pos.y) == tile) return false;

            formerLevelTilemap = new TilemapSave(tilemap);

            tilemap.FloodFill(pos, tile);
            return true;
        }

        public void Undo()
        {
            formerLevelTilemap.Get();
        }
    }
}
