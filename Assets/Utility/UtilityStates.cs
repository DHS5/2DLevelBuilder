using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dhs5.Utility
{
    public static class UtilityStates
    {
        #region Container

        private static readonly string containerPath = "UtilityStates";

        private static UtilityStatesContainer container;
        private static UtilityStatesContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = Resources.Load<UtilityStatesContainer>(containerPath);
                    if (container == null) ZDebug.LogE("Utility States Container can't be found");
                }
                return container;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Window/Dhs5 Utility/UtilityStates Container", priority = 0)]
        private static void GetContainer()
        {
            Selection.activeObject = Container;
        }
#endif

        #endregion

        #region ZDebug
        public static bool ZDebugGroupsContains(ZDebugGroup group) 
        {
            if (Container == null || Container.NoneZDebugGroups) return false;
            return Container.AllZDebugGroups || Container.ZDebugGroupsContains(group);
        }

        // Debug On Screen
        public static Color OnScreendebugColor
        {
            get
            {
                if (Container == null) return Color.white;
                return Container.OnScreenDebugColor;
            }
            set
            {
                if (Container != null) Container.OnScreenDebugColor = value;
            }
        }
        public static float OnScreendebugTime
        {
            get
            {
                if (Container == null) return 5.0f;
                return Container.OnScreenDebugTime;
            }
            set
            {
                if (Container != null) Container.OnScreenDebugTime = value;
            }
        }

        #endregion
    }
}
