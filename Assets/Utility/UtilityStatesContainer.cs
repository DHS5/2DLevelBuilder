using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility
{
    [CreateAssetMenu(fileName = "UtilityStates", menuName = "Utility/States/Container")]
    public class UtilityStatesContainer : ScriptableObject
    {
        #region ZDebug
        [Header("ZDebug"), Space]
        [SerializeField] private List<ZDebugGroup> debugGroups = new();

        public bool AllZDebugGroups { get { return debugGroups.Contains(ZDebugGroup.ALL); } }
        public bool NoneZDebugGroups { get { return debugGroups.Contains(ZDebugGroup.NONE); } }
        public bool ZDebugGroupsContains(ZDebugGroup group)
        {
            return debugGroups.Contains(group);
        }

        [Space, SerializeField] private Color onScreenDebugColor;
        public Color OnScreenDebugColor { get { return onScreenDebugColor; } set { onScreenDebugColor = value; } }

        [Space, SerializeField] private float onScreenDebugTime;
        public float OnScreenDebugTime { get { return onScreenDebugTime; } set { onScreenDebugTime = value; } }

        #endregion
    }
}
