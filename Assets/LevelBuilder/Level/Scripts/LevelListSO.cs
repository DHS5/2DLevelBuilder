using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    [CreateAssetMenu(fileName = "Level List", menuName = "Level Assets/List")]
    public class LevelListSO : ScriptableObject
    {
        public List<LevelSO> levels;
    }
}
