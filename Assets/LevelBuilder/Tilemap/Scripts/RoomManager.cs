using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    public static class RoomManager
    {
        readonly public static Vector2Int roomSize = new(64, 32);

        private static int HLimit(int layer)
        { 
            return (roomSize.x / 2) / (layer + 1); 
        }
        private static int VLimit(int layer)
        { 
            return (roomSize.y / 2) / (layer + 1);
        }

        public static bool IsInCurrentRoom(Vector3 pos, int layer)
        {
            if (pos.x < -HLimit(layer) || pos.x > HLimit(layer) || pos.y < -VLimit(layer) || pos.y > VLimit(layer)) 
                return false;
            return true;
        }

        public static bool AreInCurrentRoom(Vector3 pos1, Vector3 pos2, int layer)
        {
            return IsInCurrentRoom(pos1, layer) && IsInCurrentRoom(pos2, layer);
        }
    }
}
