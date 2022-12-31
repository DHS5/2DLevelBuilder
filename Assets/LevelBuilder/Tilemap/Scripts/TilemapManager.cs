using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

namespace LevelBuilder2D
{
    public enum Brush
    {
        PAINT = 0,
        BOX = 1,
        FILL = 2,
        PICK = 3
    }

    public class TilemapManager : MonoBehaviour
    {
        [Header("Event System")]
        [SerializeField] private EventSystem eventSystem;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap[] tilemaps;

        [Header("Main camera")]
        [SerializeField] private Camera mainCamera;

        [Header("Line Renderer")]
        [SerializeField] private LineRenderer lineRenderer;

        // Inputs
        private LevelBuilder_InputActions inputActions;

        // Preview
        public PreviewHandler PreviewHandler { get; private set; }



        // Private variables

        private Vector2 mousePos;
        private Vector2 MousePos
        {
            get { return mousePos; }
            set { mousePos = value; }
        }
        private Vector2 startMousePos;
        private Vector2 StartMousePos
        {
            get { return startMousePos; }
            set { startMousePos = value; }
        }

        private TileBase currentTile;
        public TileBase CurrentTile
        {
            get { return currentTile; }
            private set { currentTile = value; }
        }

        private int currentTilemapLayer;
        public int CurrentTilemapLayer
        {
            get { return currentTilemapLayer; }
            private set { currentTilemapLayer = value; }
        }

        private Brush currentBrush = Brush.PAINT;
        public Brush CurrentBrush
        {
            get { return currentBrush; }
            private set { currentBrush = value; }
        }


        public Tilemap[] Tilemaps
        {
            get { return tilemaps; }
            set { tilemaps = value; }
        }
        public Tilemap CurrentTilemap
        { 
            get { return Tilemaps[CurrentTilemapLayer]; }
        }

        private bool isPointerOnUI;


        // Static Actions
        public static Action<Item> SetTileAction;

        public static Action<Brush> SetBrushAction;

        // Private Actions
        private event Action LeftMouse;
        private event Action LeftMouseUp;
        private event Action LeftMouseDown;
        private event Action RightMouse;
        private event Action RightMouseUp;
        private event Action RightMouseDown;



        private void Awake()
        {
            inputActions = new LevelBuilder_InputActions();
            PreviewHandler = new PreviewHandler();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, Open);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, Close);

            SetTileAction += GetCurrentTile;
            SetBrushAction += GetCurrentBrush;
        }
        private void OnDisable()
        {
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, Open);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, Close);

            SetTileAction -= GetCurrentTile;
            SetBrushAction -= GetCurrentBrush;
        }

        private void Open()
        {
            inputActions.Enable();

            inputActions.LevelBuilder.MousePosition.performed += OnMouseMove;

            inputActions.LevelBuilder.MouseLeftClick.performed += OnLeftMouseDown;
            inputActions.LevelBuilder.MouseLeftClick.canceled += OnLeftMouseUp;
            inputActions.LevelBuilder.MouseRightClick.performed += OnRightMouseDown;
            inputActions.LevelBuilder.MouseRightClick.canceled += OnRightMouseUp;

            inputActions.LevelBuilder.Undo.performed += Undo;
            inputActions.LevelBuilder.Redo.performed += Redo;

            MapBrushActions();

            PreviewHandler.Enable();
        }

        private void Close()
        {
            inputActions.Disable();

            inputActions.LevelBuilder.MousePosition.performed -= OnMouseMove;

            inputActions.LevelBuilder.MouseLeftClick.performed -= OnLeftMouseDown;
            inputActions.LevelBuilder.MouseLeftClick.canceled -= OnLeftMouseUp;
            inputActions.LevelBuilder.MouseRightClick.performed -= OnRightMouseDown;
            inputActions.LevelBuilder.MouseRightClick.canceled -= OnRightMouseUp;

            inputActions.LevelBuilder.Undo.performed -= Undo;
            inputActions.LevelBuilder.Redo.performed -= Redo;

            ClearTilemaps();

            PreviewHandler.Disable();
        }


        // ### Tilemap ###

        private void ClearTilemaps()
        {
            foreach (Tilemap tilemap in tilemaps)
            {
                tilemap.ClearAllTiles();
                tilemap.CompressBounds();
            }
            TilemapCommandManager.Instance.Clear();
        }


        // ### Listeners ###

        private void GetCurrentTile(Item item) { CurrentTile = item.tile; CurrentTilemapLayer = item.layer; }

        private void GetCurrentBrush(Brush brush) { CurrentBrush = brush; MapBrushActions(); }


        // ### Helpers ###

        private void SetTile(Vector3 pos, Tilemap tilemap, TileBase tile)
        {
            Vector3Int tilemapPos = GetTilemapPosition(pos, tilemap);
            if (RoomManager.IsInCurrentRoom(WorldPos(pos), CurrentTilemapLayer))
            {
                TilemapCommandManager.Instance.SetTile(tilemapPos, tilemap, tile, PreviewHandler.RealTile);
                PreviewHandler.RealTile = tile;
            }
        }
        private void FillTile(Vector3 pos, Tilemap tilemap, TileBase tile)
        {
            Vector3Int tilemapPos = GetTilemapPosition(pos, tilemap);
            if (RoomManager.IsInCurrentRoom(WorldPos(pos), CurrentTilemapLayer))
            {
                //tilemap.FloodFill(tilemapPos, tile);
                TilemapCommandManager.Instance.Fill(tilemapPos, tilemap, tile);
            }
        }
        private void FillBox(Vector3 startPos, Vector3 endPos, Tilemap tilemap, TileBase tile)
        {
            Vector3Int startTilemapPos = GetTilemapPosition(startPos, tilemap);
            Vector3Int endTilemapPos = GetTilemapPosition(endPos, tilemap);
            if (RoomManager.AreInCurrentRoom(startPos, endPos, CurrentTilemapLayer))
            {
                //tilemap.BoxFill(tile, startTilemapPos, endTilemapPos);
                TilemapCommandManager.Instance.BoxFill(startTilemapPos, endTilemapPos, tilemap, tile);
            }
        }
        private void GetTile(Vector3 pos)
        {
            Vector3Int tilemapPos;
            for (int i = tilemaps.Length - 1; i >= 0; i--)
            {
                tilemapPos = GetTilemapPosition(pos, tilemaps[i]);
                if (tilemaps[i].HasTile(tilemapPos))
                {
                    Item item = new Item { tile = tilemaps[i].GetTile(tilemapPos), layer = i };
                    ItemToggle.OnPickTile.Invoke(item);
                    return;
                }
            }
        }

        private Vector3Int GetTilemapPosition(Vector3 pos, Tilemap tilemap)
        {
            return tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(pos));
        }
        private Vector3 WorldPos(Vector3 pos) 
        { 
            return mainCamera.ScreenToWorldPoint(pos); 
        }

        // ### Inputs ###

        private void Update()
        {
            isPointerOnUI = eventSystem.IsPointerOverGameObject();
            if (!isPointerOnUI)
            {
                // Actions on the Tilemap
                if (inputActions.LevelBuilder.MouseLeftClick.ReadValue<float>() > 0 && LeftMouse != null) { LeftMouse(); }
                if (inputActions.LevelBuilder.MouseRightClick.ReadValue<float>() > 0 && RightMouse != null) { RightMouse(); }
            }
        }


        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            MousePos = ctx.ReadValue<Vector2>();

            // Preview
            UpdatePreview();

            // Line renderer
            UpdateLineRenderer();
        }

        private void OnLeftMouseDown(InputAction.CallbackContext ctx)
        {
            if (!isPointerOnUI && LeftMouseDown != null)
                LeftMouseDown();
        }
        private void OnLeftMouseUp(InputAction.CallbackContext ctx)
        {
            if (!isPointerOnUI && LeftMouseUp != null)
                LeftMouseUp();
        }
        private void OnRightMouseDown(InputAction.CallbackContext ctx)
        {
            if (!isPointerOnUI && RightMouseDown != null)
                RightMouseDown();
        }
        private void OnRightMouseUp(InputAction.CallbackContext ctx)
        {
            if (!isPointerOnUI && RightMouseUp != null)
                RightMouseUp();
        }

        private void MapBrushActions()
        {
            LeftMouse = null;
            RightMouse = null;
            LeftMouseUp = null;
            RightMouseUp = null;
            LeftMouseDown = null;
            RightMouseDown = null;

            switch(CurrentBrush)
            {
                case Brush.PAINT:
                    LeftMouse = LeftPaint;
                    RightMouse = RightPaint;
                    break;
                case Brush.BOX:
                    LeftMouseUp = LeftBox;
                    RightMouseUp = RightBox;
                    LeftMouseDown = StartLeftBox;
                    RightMouseDown = StartRightBox;
                    break;
                case Brush.FILL:
                    LeftMouse = LeftFill;
                    RightMouse = RightFill;
                    break;
                case Brush.PICK:
                    LeftMouse = LeftPick;
                    break;
            }
        }


        // ### Tilemap actions ###

        private void RightPaint()
        {
            SetTile(MousePos, tilemaps[CurrentTilemapLayer], null);
        }
        private void LeftPaint()
        {
            SetTile(MousePos, tilemaps[CurrentTilemapLayer], CurrentTile);
        }
        private void RightBox()
        {
            lineRendererActive = false;
            UpdateLineRenderer();
            FillBox(StartMousePos, MousePos, tilemaps[CurrentTilemapLayer], null);
            MapBrushActions();
        }
        private void LeftBox()
        {
            lineRendererActive = false;
            UpdateLineRenderer();
            FillBox(StartMousePos, MousePos, tilemaps[CurrentTilemapLayer], CurrentTile);
            MapBrushActions();
        }
        private void StartLeftBox()
        {
            StartMousePos = MousePos;
            lineRendererActive = true;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            RightMouseUp = null;
            RightMouseDown = null;
        }
        private void StartRightBox()
        {
            StartMousePos = MousePos;
            lineRendererActive = true;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            LeftMouseUp = null;
            LeftMouseDown = null;
        }
        private void RightFill()
        {
            FillTile(MousePos, tilemaps[CurrentTilemapLayer], null);
        }
        private void LeftFill()
        {
            FillTile(MousePos, tilemaps[CurrentTilemapLayer], CurrentTile);
        }
        private void LeftPick()
        {
            GetTile(MousePos);
        }

        private void Undo(InputAction.CallbackContext ctx)
        {
            TilemapCommandManager.Instance.Undo();
        }
        private void Redo(InputAction.CallbackContext ctx)
        {
            TilemapCommandManager.Instance.Redo();
        }


        // ### Preview ###

        private void UpdatePreview()
        {
            Vector3Int tilemapPos = GetTilemapPosition(MousePos, CurrentTilemap);
            PreviewHandler.UpdatePreview(CurrentTilemap, new Vector2Int(tilemapPos.x, tilemapPos.y), CurrentTile,
                !isPointerOnUI && CurrentBrush == Brush.PAINT && RoomManager.IsInCurrentRoom(WorldPos(MousePos), CurrentTilemapLayer));
        }


        // ### Line Renderer ###

        private bool lineRendererActive;

        private void UpdateLineRenderer()
        {
            if (isPointerOnUI) lineRendererActive = false;

            lineRenderer.enabled = lineRendererActive;

            if (lineRendererActive)
            {
                lineRenderer.positionCount = 4;
                lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(StartMousePos));
                lineRenderer.SetPosition(1, new Vector2(mainCamera.ScreenToWorldPoint(StartMousePos).x,
                    mainCamera.ScreenToWorldPoint(MousePos).y));
                lineRenderer.SetPosition(2, mainCamera.ScreenToWorldPoint(MousePos));
                lineRenderer.SetPosition(3, new Vector2(mainCamera.ScreenToWorldPoint(MousePos).x,
                    mainCamera.ScreenToWorldPoint(StartMousePos).y));
                
            }
        }
    }
}
