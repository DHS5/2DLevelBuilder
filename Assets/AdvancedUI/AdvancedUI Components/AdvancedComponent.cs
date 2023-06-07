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

            SetUpGraphics();
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
        protected abstract void SetUpGraphics();


        public abstract bool Interactable { get; set; }


        public void ShowObjectOnStack(GameObject go) { if (go == null) return; UIStack.Show(go); }
        public void BackOnStack() { UIStack.Back(); }
        public void ClearStack() { UIStack.Clear(); }
        public void StartNewStack(GameObject go) { if (go == null) return; UIStack.Clear(go); }


        [Header("Style Sheet Container")]
        [SerializeField] protected StyleSheetContainer styleSheetContainer;
    }
}
