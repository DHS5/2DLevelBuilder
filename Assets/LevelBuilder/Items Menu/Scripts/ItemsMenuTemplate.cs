using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    [CreateAssetMenu(fileName = "Items Menu Template", menuName = "Items Menu/Template")]
    public class ItemsMenuTemplate : ScriptableObject
    {
        public ItemsMenuCategory[] categories;
    }
}
