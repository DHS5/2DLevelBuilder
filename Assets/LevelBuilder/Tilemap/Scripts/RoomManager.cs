using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    public static class RoomManager
    {
        readonly public static Vector2Int roomSize = new(64, 32);

        private static Vector2Int roomNumber = new(0, 0);

        private static int HLimit
        {
            get { return (roomSize.x / 2); }
        }
        private static int VLimit
        {
            get { return (roomSize.y / 2); }
        }

        public static bool IsInCurrentRoom(Vector3 pos)
        {
            if (pos.x < -HLimit || pos.x > HLimit || pos.y < -VLimit || pos.y > VLimit)
                return false;
            return true;
        }

        public static bool AreInCurrentRoom(Vector3 pos1, Vector3 pos2)
        {
            return IsInCurrentRoom(pos1) && IsInCurrentRoom(pos2);
        }

        public static BoundsInt RoomBounds(Tilemap tilemap)
        {
            return new BoundsInt(
                tilemap.WorldToCell(new Vector3(-HLimit, -VLimit, 0)),
                new Vector3Int(roomSize.x - 1, roomSize.y - 1, 0));
        }

        /// <summary>
        /// Change room in the selected direction
        /// </summary>
        /// <param name="upDown">UP = 1 ; DOWN = -1</param>
        /// <param name="leftRight">RIGHT = 1 ; LEFT = -1</param>
        /// <param name="tilemaps">Tilemaps to move</param>
        public static void ChangeRoom(int upDown, int leftRight, Tilemap[] tilemaps)
        {
            roomNumber += new Vector2Int(upDown, leftRight);
            foreach (Tilemap tilemap in tilemaps)
            {
                tilemap.gameObject.transform.localPosition += new Vector3(- leftRight * roomSize.x, - upDown * roomSize.y, 0);
            }
        }


        public static void BackToFirstRoom(Tilemap[] tilemaps)
        {
            ChangeRoom(-roomNumber.x, -roomNumber.y, tilemaps);
        }


        public static void CreateRoomLimit(Tilemap tilemap, TileBase roomLimitTile)
        {
            for (int x = - HLimit - 1; x < HLimit + 1; x++)
            {
                tilemap.SetTile(new Vector2Int(x, VLimit), roomLimitTile);
                tilemap.SetTile(new Vector2Int(x, -VLimit - 1), roomLimitTile);
            }
            for (int y = - VLimit - 1; y < VLimit + 1; y++)
            {
                tilemap.SetTile(new Vector2Int(HLimit, y), roomLimitTile);
                tilemap.SetTile(new Vector2Int(-HLimit - 1, y), roomLimitTile);
            }
        }
    }
}
