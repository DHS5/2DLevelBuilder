using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public class SetTileCommand : TilemapCommandManager.ICommand
    {
        private Vector3Int pos;
        private Tilemap tilemap;

        private TileBase formerTile;
        private TileBase newTile;

        public SetTileCommand(Vector3Int _pos, Tilemap _tilemap, TileBase _tile)
        {
            pos = _pos;
            tilemap = _tilemap;
            newTile = _tile;

            formerTile = tilemap.GetTile(pos);
        }

        public bool Execute()
        {
            if (tilemap.GetTile(pos.x, pos.y) == newTile) return false;

            tilemap.SetTile(pos, newTile);
            return true;
        }

        public void Undo()
        {
            tilemap.SetTile(pos, formerTile);
        }
    }
}
