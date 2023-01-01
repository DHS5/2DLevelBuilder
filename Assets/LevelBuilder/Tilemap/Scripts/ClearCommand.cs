using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public class ClearCommand : TilemapCommandManager.ICommand
    {
        private Tilemap[] tilemaps;

        private TilemapSave[] formerLevelTilemaps;

        public ClearCommand(Tilemap[] _tilemaps)
        {
            tilemaps = _tilemaps;
        }

        public bool Execute()
        {
            formerLevelTilemaps = new TilemapSave[tilemaps.Length];
            for (int i = 0; i < tilemaps.Length; i++)
            {
                formerLevelTilemaps[i] = new TilemapSave(tilemaps[i]);
                tilemaps[i].ClearAllTiles();
                tilemaps[i].CompressBounds();
            }
            return true;
        }

        public void Undo()
        {
            foreach (TilemapSave tilemapSave in formerLevelTilemaps)
                tilemapSave.Get();
        }
    }
}
