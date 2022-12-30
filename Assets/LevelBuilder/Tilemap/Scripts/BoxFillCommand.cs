using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public class BoxFillCommand : TilemapCommandManager.ICommand
    {
        private Vector3Int startPos;
        private Vector3Int endPos;
        private Tilemap tilemap;
        private TileBase tile;

        private TilemapSave formerLevelTilemap;

        public BoxFillCommand(Vector3Int _startPos, Vector3Int _endPos, Tilemap _tilemap, TileBase _tile)
        {
            startPos = _startPos;
            endPos = _endPos;
            tilemap = _tilemap;
            tile = _tile;

            formerLevelTilemap = new TilemapSave(_tilemap);
        }

        public bool Execute()
        {
            tilemap.BoxFill(tile, startPos, endPos);
            return true;
        }

        public void Undo()
        {
            formerLevelTilemap.Get();
        }
    }
}
