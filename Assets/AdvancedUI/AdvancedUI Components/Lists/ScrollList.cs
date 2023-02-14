using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dhs5.Utility;
using System;

namespace Dhs5.AdvancedUI
{
    public interface IScrollList
    {
        public int CurrentSelectionIndex();
        public void Enable();
        public void Disable();
        public void Destroy();
        public void AutoScroll(int units);

        public event Action<int, string> OnSelectionChange;
        public void InvokeSelectionChange();

        public object GetCurrentlySelectedObject();
    }

    public abstract class ScrollList<T> : IScrollList
    {
        protected enum SwipeDirection { LEFT = -1, RIGHT = 1, UP = -1, DOWN = 1 }

        protected ScrollListComponent scrollListComponent;
        protected List<T> list;
        protected List<ScrollListSocket> sockets;
        protected DragableUI dragableObject;
        protected GameObject prefab;

        // Parameters
        protected bool isHorizontal;
        protected bool useScroll;
        protected float scrollSensitivity;
        protected float socketSize;
        protected float spaceBetweenSockets;

        protected bool useAnim;
        protected float animLerp;
        protected float animDelay;

        public ScrollList(ScrollListComponent _scrollListComponent, List<T> _list, List<ScrollListSocket> _sockets,
            DragableUI _dragableObject, GameObject _prefab,
            bool _isHorizontal, bool _useScroll, float _scrollSensitivity, float _socketSize, float _spaceBetweenSockets,
            bool _useAnim, float _animLerp, float _animDelay)
        {
            scrollListComponent = _scrollListComponent;
            list = _list;
            sockets = _sockets;
            dragableObject = _dragableObject;
            prefab = _prefab;

            isHorizontal = _isHorizontal;
            useScroll = _useScroll;
            scrollSensitivity = _scrollSensitivity;
            socketSize = _socketSize;
            spaceBetweenSockets = _spaceBetweenSockets;
            useAnim = _useAnim;
            animLerp = _animLerp;
            animDelay = _animDelay;
        }

        protected List<ScrollListObject> scrollListObjects = new();

        protected int TotalObjectNumber => sockets.Count;
        protected int RightSocketsNumber => TotalObjectNumber / 2 + 1;
        protected int LeftSocketsNumber => RightSocketsNumber - 1;

        public abstract int CurrentSelectionIndex();
        public object GetCurrentlySelectedObject()
        {
            return list[CurrentSelectionIndex()];
        }

        public event Action<int, string> OnSelectionChange;
        protected void FireSelectionChange(int index, string name) { OnSelectionChange?.Invoke(index, name); }

        public abstract void InvokeSelectionChange();

        #region List Management

        public void Destroy()
        {
            StopRepositioningCR();

            Disable();

            foreach (var item in scrollListObjects)
            {
                item.Destroy();
            }
        }

        public abstract void CreateList();

        #endregion

        #region Swipe Management

        protected bool canMove = false;

        public virtual void Enable()
        {
            canMove = true;
            if (dragableObject)
            {
                dragableObject.Drag += OnScroll;
                dragableObject.DragDelta += OnEndScroll;
            }
        }
        public virtual void Disable()
        {
            canMove = false;
            if (dragableObject)
            {
                dragableObject.Drag -= OnScroll;
                dragableObject.DragDelta -= OnEndScroll;
            }
        }

        protected virtual void OnScroll(PointerEventData pointerEventData)
        {
            if (!canMove || !useScroll) return;

            MoveScrollListObjects(isHorizontal ? pointerEventData.delta.x : pointerEventData.delta.y);
        }
        protected virtual void OnEndScroll(Vector2 delta)
        {
            if (!canMove || !useScroll) return;

            float fDelta = isHorizontal ? delta.x : delta.y;
            float absDelta = Mathf.Abs(fDelta);
            if (absDelta > scrollSensitivity)
            {
                Swipe(fDelta, Mathf.Max(1, Mathf.RoundToInt(absDelta / (socketSize + spaceBetweenSockets))));
            }
            else
            {
                RepositionAll();
            }
        }

        protected abstract void MoveScrollListObjects(float delta);

        protected virtual void Swipe(float delta, int units)
        {
            if (isHorizontal)
            {
                Swipe(delta > 0 ? SwipeDirection.LEFT : SwipeDirection.RIGHT, units);
            }
            else
            {
                Swipe(delta > 0 ? SwipeDirection.DOWN : SwipeDirection.UP, units);
            }
        }
        protected abstract void Swipe(SwipeDirection direction, int units);

        public virtual void AutoScroll(int units)
        {
            if (!canMove) return;

            int absUnits = Mathf.Abs(units);
            SwipeDirection direction = (SwipeDirection)(units / absUnits);

            Swipe(direction, absUnits);
        }

        protected abstract void RepositionAll();

        #region Coroutine Management

        protected Coroutine repositioningCoroutine;

        protected void StartRepositioningCR()
        {
            repositioningCoroutine = scrollListComponent.StartCoroutine(RepositionAllCR(animLerp, animDelay));
        }
        protected void StopRepositioningCR()
        {
            if (repositioningCoroutine != null)
                scrollListComponent.StopCoroutine(repositioningCoroutine);
        }
        #endregion

        protected abstract IEnumerator RepositionAllCR(float lerp, float delay);

        #endregion
    }
}
