using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dhs5.AdvancedUI
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public abstract class AdvancedComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            SetUpConfig();
        }
        private void OnValidate()
        {
            SetUpConfig();
        }

        protected virtual void OnEnable()
        {
            LinkEvents();
        }
        protected virtual void OnDisable()
        {
            UnlinkEvents();
        }

        protected abstract void LinkEvents();
        protected abstract void UnlinkEvents();

        protected abstract void SetUpConfig();


        public abstract bool Interactable { get; set; }
    }
}
