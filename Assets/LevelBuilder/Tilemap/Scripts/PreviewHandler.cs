using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Dhs5.Utility.EventSystem;
using Dhs5.Utility.Tilemaps;

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


        readonly Color fadeColor = new Color(1, 1, 1, 0.5f);


        public TileBase RealTile
        {
            get { return currentPreview.realTile; }
            set { currentPreview.ActuRealTile(value); }
        }


        public void Enable() { EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.BEFORE_SAVE, HidePreview); }
        public void Disable() { EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.BEFORE_SAVE, HidePreview); }


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


        public void HidePreview()
        {
            if (!currentPreview.SameTile())
            {
                tilemap.SetTile(currentPreview.pos, currentPreview.realTile);
            }
            FadeColor(new Vector3Int(currentPreview.pos.x, currentPreview.pos.y, 0), false);
        }
        private void ShowPreview()
        {
            if (!currentPreview.SameTile())
            {
                tilemap.SetTile(currentPreview.pos, currentPreview.previewTile);
            }
            FadeColor(new Vector3Int(currentPreview.pos.x, currentPreview.pos.y, 0), true);
        }

        private void ActuPreview(Vector2Int _pos, TileBase _previewTile, TileBase _relaTile)
        {
            HidePreview();
            currentPreview.Set(_pos, _previewTile, _relaTile);
            ShowPreview();
        }

        private void FadeColor(Vector3Int pos, bool state)
        {
            if (state) tilemap.SetTileFlags(pos, TileFlags.None);
            tilemap.SetColor(pos, state ? fadeColor : Color.white);
            if (!state) tilemap.SetTileFlags(pos, TileFlags.LockColor);
        }
    }
}
