using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    [System.Serializable]
    public struct ItemTemplate
    {
        public int number;
        public string name;
        [TextArea]
        public string description;
    }

    [System.Serializable]
    public class ItemsMenuCategory
    {
        public string categoryName = "Default";
        public int categoryNumber = 0;
        public int categoryTilemapLayer = 0;
        [Space]
        public ItemTemplate[] items;
    }
}
