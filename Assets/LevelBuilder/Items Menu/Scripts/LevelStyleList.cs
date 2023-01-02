using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    [CreateAssetMenu(fileName = "Level Style List", menuName = "Items Menu/Style List")]
    public class LevelStyleList : ScriptableObject
    {
        public ItemsMenuContent[] menuContents;
    }
}
