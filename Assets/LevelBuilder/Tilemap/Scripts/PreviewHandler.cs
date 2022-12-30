using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public class PreviewHandler
    {
        private struct PreviewItem
        {
            public void Set(Vector2Int _pos, TileBase _previewTile, TileBase _realTile)
            {
                pos = _pos;
                previewTile = _previewTile;
                realTile = _realTile;
            }

            public bool SameTile() { return previewTile == realTile; }
            public void ActuRealTile(TileBase _tile) { if (realTile != _tile) realTile = _tile; }
            public bool SamePos(Vector2Int _pos)
            {
                if (_pos.x == pos.x && _pos.y == pos.y) return true;
                return false;
            }

            public Vector2Int pos;
            public TileBase previewTile;
            public TileBase realTile;
        }

        private Tilemap tilemap;
        private bool show;
        private PreviewItem currentPreview;



        public void Enable() { EventManager.StartListening(EventManager.LevelBuilderEvent.SAVE_LEVEL, HidePreview); }
        public void Disable() { EventManager.StopListening(EventManager.LevelBuilderEvent.SAVE_LEVEL, HidePreview); }


        public void UpdatePreview(Tilemap _tilemap, Vector2Int _pos, TileBase _tile, bool _show)
        {
            if (!show && !_show) return;

            tilemap = _tilemap;

            if (show && !_show)
            {
                HidePreview();
                show = false;
                return;
            }
            if (!show && _show)
            {
                currentPreview.Set(_pos, _tile, _tilemap.GetTile(_pos));
                ShowPreview();
                show = true;
                return;
            }

            else if (!currentPreview.SamePos(_pos))
            {
                ActuPreview(_pos, _tile, _tilemap.GetTile(_pos));
                return;
            }
        }
        public void UpdateRealTile(TileBase _realTile)
        {
            currentPreview.ActuRealTile(_realTile);
        }


        public void HidePreview()
        {
            if (!currentPreview.SameTile())
                tilemap.SetTile(currentPreview.pos, currentPreview.realTile);
        }
        private void ShowPreview()
        {
            if (!currentPreview.SameTile())
                tilemap.SetTile(currentPreview.pos, currentPreview.previewTile);
        }

        private void ActuPreview(Vector2Int _pos, TileBase _previewTile, TileBase _relaTile)
        {
            HidePreview();
            currentPreview.Set(_pos, _previewTile, _relaTile);
            ShowPreview();
        }
    }
}
