using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dhs5.Utility;
using System;

namespace Dhs5.AdvancedUI
{
    public class InfiniteScrollList<T> : ScrollList<T>, IScrollList
    {

        readonly private List<ScrollListSocket> socketsInOrder;

        public InfiniteScrollList(ScrollListComponent _scrollListComponent, List<T> _list, List<ScrollListSocket> _sockets,
            List<ScrollListSocket> _socketsInOrder, DragableUI _dragableObject, GameObject _prefab,
            bool _isHorizontal, bool _useScroll, float _scrollSensitivity, float _socketSize, float _spaceBetweenSockets,
            bool _useAnim, float _animLerp, float _animDelay) : base(_scrollListComponent, _list, _sockets, _dragableObject, _prefab,
            _isHorizontal, _useScroll, _scrollSensitivity, _socketSize, _spaceBetweenSockets, _useAnim, _animLerp, _animDelay)
        {
            socketsInOrder = _socketsInOrder;

            CreateList();
        }
        private ScrollListSocket MaxSocket => socketsInOrder.Get(-1);
        private ScrollListSocket MinSocket => socketsInOrder[0];

        public override int CurrentSelectionIndex()
        {
            return sockets[0].ScrollListObject.Index;
        }

        public override void InvokeSelectionChange()
        {
            ScrollListObject currentSelection = sockets[0].ScrollListObject;
            FireSelectionChange(currentSelection.Index, currentSelection.GetName(list[currentSelection.Index]));
        }

        #region List Management

        private void AddToScrollListObjects(object obj, int objectIndex, int socketIndex)
        {
            ScrollListObject scrollListObject = GameObject.Instantiate(prefab, sockets[socketIndex].transform)
                .GetComponent<ScrollListObject>();
            scrollListObject.Set(obj, objectIndex);

            sockets[socketIndex].ScrollListObject = scrollListObject;
            scrollListObjects.Add(scrollListObject);
        }

        public override void CreateList()
        {
            // Right sockets
            for (int i = 0; i < RightSocketsNumber; i++)
            {
                AddToScrollListObjects(list.Get(i), list.ValidIndex(i), i * 2);
            }

            // Left sockets
            for (int i = 0; i < LeftSocketsNumber; i++)
            {
                AddToScrollListObjects(list.Get(-i - 1), list.ValidIndex(-i - 1), i * 2 + 1);
            }
        }

        #endregion

        #region Swipe Management

        protected override void MoveScrollListObjects(float delta)
        {
            foreach (var obj in scrollListObjects)
            {
                obj.Move(delta, isHorizontal);
            }
        }
        protected override void Swipe(SwipeDirection direction, int units)
        {
            int extremeObjectIndex;
            ScrollListObject extremeObject;

            for (int u = 0; u < units; u++)
            {
                if (direction == SwipeDirection.RIGHT || direction == SwipeDirection.DOWN)
                {
                    extremeObject = MinSocket.ScrollListObject;
                    for (int i = 0; i < TotalObjectNumber - 1; i++)
                    {
                        socketsInOrder[i].ScrollListObject = socketsInOrder.Get(i + 1).ScrollListObject;
                    }
                    extremeObjectIndex = list.ValidIndex(socketsInOrder[TotalObjectNumber - 1].ScrollListObject.Index + 1);
                    socketsInOrder[TotalObjectNumber - 1].ScrollListObject = extremeObject;
                    socketsInOrder[TotalObjectNumber - 1].ScrollListObject.Set(list[extremeObjectIndex], extremeObjectIndex);
                }
                else
                {
                    extremeObject = MaxSocket.ScrollListObject;
                    for (int i = TotalObjectNumber - 1; i > 0; i--)
                    {
                        socketsInOrder[i].ScrollListObject = socketsInOrder.Get(i - 1).ScrollListObject;
                    }
                    extremeObjectIndex = list.ValidIndex(socketsInOrder[0].ScrollListObject.Index - 1);
                    socketsInOrder[0].ScrollListObject = extremeObject;
                    socketsInOrder[0].ScrollListObject.Set(list[extremeObjectIndex], extremeObjectIndex);
                }
                extremeObject.ImmediateReposition = true;
            }

            RepositionAll();

            if (units != 0)
            {
                InvokeSelectionChange();
            }
        }

        protected override void RepositionAll()
        {
            if (useAnim)
            {
                StartRepositioningCR();
                return;
            }
            foreach (var obj in scrollListObjects)
            {
                obj.LocalReset();
            }
        }
        protected override IEnumerator RepositionAllCR(float lerp, float delay)
        {
            canMove = false;

            foreach (var obj in scrollListObjects)
            {
                if (obj.ImmediateReposition)
                    obj.LocalReset();
            }

            float currentLerp = lerp;
            while (currentLerp < 0.99f)
            {
                currentLerp = Mathf.Lerp(currentLerp, 1, lerp);
                foreach (var obj in scrollListObjects)
                {
                    obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition, Vector3.zero, lerp);
                }
                yield return new WaitForSeconds(delay);
            }
            foreach (var obj in scrollListObjects)
            {
                obj.LocalReset();
            }

            canMove = true;
        }
        #endregion
    }
}
