using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.Events
{
    [Serializable]
    public class AdvancedUnityEvent
    {
        public BaseEventAction action;

        #region Custom Property Drawer variables
        [SerializeField] UnityEngine.Object obj;
        [SerializeField] private int componentInstanceID;
        [SerializeField] private int metadataToken;
        [SerializeField] private float propertyHeight;
        #endregion

        #region Parameters
        // Int
        [SerializeField] private int int0;
        [SerializeField] private int int1;
        [SerializeField] private int int2;
        [SerializeField] private int int3;
        [SerializeField] private int int4;
        // Float
        [SerializeField] private float float0;
        [SerializeField] private float float1;
        [SerializeField] private float float2;
        [SerializeField] private float float3;
        [SerializeField] private float float4;
        // Bool
        [SerializeField] private bool bool0;
        [SerializeField] private bool bool1;
        [SerializeField] private bool bool2;
        [SerializeField] private bool bool3;
        [SerializeField] private bool bool4;
        // String
        [SerializeField] private string string0;
        [SerializeField] private string string1;
        [SerializeField] private string string2;
        [SerializeField] private string string3;
        [SerializeField] private string string4;
        #endregion

        public void Trigger()
        {
            if (action == null)
            {
                Debug.LogError("Action is null");
                return;
            }
            action.Invoke();
        }
    }

    [Serializable]
    public class BaseEventAction
    {
        public BaseEventAction() { }

        public virtual void Invoke() { }
    }

    [Serializable]
    public class EventAction : BaseEventAction
    {
        private Action action;

        public EventAction(Action _action)
        {
            action = _action;
        }

        public override void Invoke()
        {
            action?.Invoke();
        }
    }
    [Serializable]
    public class EventAction<T1> : BaseEventAction
    {
        private Action<T1> action;

        T1 arg0;
        public EventAction(Action<T1> _action, T1 _arg0)
        {
            action = _action;
            arg0 = _arg0;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0);
        }
    }
    public class EventAction<T1, T2> : BaseEventAction
    {
        private Action<T1, T2> action;

        T1 arg0;
        T2 arg1;
        public EventAction(Action<T1, T2> _action, T1 _arg0, T2 _arg1)
        {
            action = _action;
            arg0 = _arg0;
            arg1 = _arg1;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0, arg1);
        }
    }
    public class EventAction<T1, T2, T3> : BaseEventAction
    {
        private Action<T1, T2, T3> action;

        T1 arg0;
        T2 arg1;
        T3 arg2;
        public EventAction(Action<T1, T2, T3> _action, T1 _arg0, T2 _arg1, T3 _arg2)
        {
            action = _action;
            arg0 = _arg0;
            arg1 = _arg1;
            arg2 = _arg2;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0, arg1, arg2);
        }
    }
    public class EventAction<T1, T2, T3, T4> : BaseEventAction
    {
        private Action<T1, T2, T3, T4> action;

        T1 arg0;
        T2 arg1;
        T3 arg2;
        T4 arg3;
        public EventAction(Action<T1, T2, T3, T4> _action, T1 _arg0, T2 _arg1, T3 _arg2, T4 _arg3)
        {
            action = _action;
            arg0 = _arg0;
            arg1 = _arg1;
            arg2 = _arg2;
            arg3 = _arg3;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0, arg1, arg2, arg3);
        }
    }
    public class EventAction<T1, T2, T3, T4, T5> : BaseEventAction
    {
        private Action<T1, T2, T3, T4, T5> action;

        T1 arg0;
        T2 arg1;
        T3 arg2;
        T4 arg3;
        T5 arg4;
        public EventAction(Action<T1, T2, T3, T4, T5> _action, T1 _arg0, T2 _arg1, T3 _arg2, T4 _arg3, T5 _arg4)
        {
            action = _action;
            arg0 = _arg0;
            arg1 = _arg1;
            arg2 = _arg2;
            arg3 = _arg3;
            arg4 = _arg4;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0, arg1, arg2, arg3, arg4);
        }
    }
}
