using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Dhs5.Utility.EventSystem;

namespace LevelBuilder2D
{
    public class TilemapCommandManager : MonoBehaviour
    {
        public interface ICommand
        {
            public bool Execute();
            public void Undo();
        }

        public static TilemapCommandManager Instance { get; private set; }

        
        private Stack<ICommand> undoCommands = new();
        private Stack<ICommand> redoCommands = new();

        private void Awake()
        {
            Instance = this;
        }


        public void Undo()
        {
            if (undoCommands.Count == 0) return;

            ICommand command = undoCommands.Pop();
            command.Undo();
            redoCommands.Push(command);

            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.UNDO_ACTION);
        }
        public void Redo()
        {
            if (redoCommands.Count == 0) return;

            ICommand command = redoCommands.Pop();
            command.Execute();
            undoCommands.Push(command);

            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.DO_ACTION);
        }


        private void Execute(ICommand command)
        {
            if (command.Execute())
            {
                undoCommands.Push(command);
                redoCommands.Clear();

                EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.DO_ACTION);
            }
        }

        public void Clear()
        {
            undoCommands.Clear();
            redoCommands.Clear();
        }


        // ### Commands ###

        public void SetTile(Vector3Int _pos, Tilemap _tilemap, TileBase _tile, TileBase _formerTile)
        {
            Execute(new SetTileCommand(_pos, _tilemap, _tile, _formerTile));
        }

        public void BoxFill(Vector3Int _startPos, Vector3Int _endPos, Tilemap _tilemap, TileBase _tile)
        {
            Execute(new BoxFillCommand(_startPos, _endPos, _tilemap, _tile));
        }

        public void Fill(Vector3Int _pos, Tilemap _tilemap, TileBase _tile)
        {
            Execute(new FillCommand(_pos, _tilemap, _tile));
        }

        public void Clear(Tilemap[] tilemaps)
        {
            Execute(new ClearCommand(tilemaps));
        }
    }
}
