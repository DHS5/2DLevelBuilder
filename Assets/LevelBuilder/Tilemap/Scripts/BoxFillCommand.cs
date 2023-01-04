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

            formerLevelTilemap = new TilemapSave(_tilemap, GetBoundsFromBox(_startPos, _endPos));
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


        private BoundsInt GetBoundsFromBox(Vector3Int _startPos, Vector3Int _endPos)
        {
            Vector3Int min = new Vector3Int(Mathf.Min(_startPos.x, _endPos.x), Mathf.Min(_startPos.y, _endPos.y), 0);
            Vector3Int max = new Vector3Int(Mathf.Max(_startPos.x, _endPos.x), Mathf.Max(_startPos.y, _endPos.y), 0);

            return new BoundsInt(min, max - min + Vector3Int.one);
        }
    }
}
