using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dhs5.Utility;
using System;

namespace Dhs5.AdvancedUI
{
    public class SimpleScrollList<T> : ScrollList<T>, IScrollList
    {

        readonly private GameObject objectContainer;

        public SimpleScrollList(ScrollListComponent _scrollListComponent, List<T> _list, List<ScrollListSocket> _sockets,
            GameObject _objectContainer, DragableUI _dragableObject, GameObject _prefab,
            bool _isHorizontal, bool _useScroll, float _scrollSensitivity, float _socketSize, float _spaceBetweenSockets,
            bool _useAnim, float _animLerp, float _animDelay) : base(_scrollListComponent, _list, _sockets, _dragableObject, _prefab,
            _isHorizontal, _useScroll, _scrollSensitivity, _socketSize, _spaceBetweenSockets, _useAnim, _animLerp, _animDelay)
        {
            objectContainer = _objectContainer;

            CreateList();
        }

        private ScrollListSocket currentSocket;
        private ScrollListSocket CurrentSocket
        {
            get { return currentSocket; }
            set { currentSocket = value; objectContainer.transform.SetParent(currentSocket.transform, true); }
        }

        public override int CurrentSelectionIndex()
        {
            return CurrentSocket.Index;
        }

        public override void InvokeSelectionChange()
        {
            ScrollListObject currentSelection = scrollListObjects[CurrentSocket.Index];
            FireSelectionChange(CurrentSocket.Index, currentSelection.GetName(list[currentSelection.Index]));
        }

        #region List Management

        private void SetUpScrollListObjectRect()
        {
            RectTransform rectTransform = prefab.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.SetSizeWithCurrentAnchors
                (isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, socketSize);
        }

        private void AddToScrollListObjects(object obj, int index)
        {
            ScrollListObject scrollListObject = GameObject.Instantiate(prefab, objectContainer.transform)
                .GetComponent<ScrollListObject>();
            scrollListObject.Set(obj, index);

            scrollListObjects.Add(scrollListObject);
        }

        public override void CreateList()
        {
            SetUpScrollListObjectRect();

            CurrentSocket = sockets[0];

            for (int i = 0; i < list.Count; i++)
            {
                AddToScrollListObjects(list[i], i);
            }
        }

        #endregion

        #region Swipe Management

        protected override void MoveScrollListObjects(float delta)
        {
            objectContainer.transform.Translate(new Vector3(isHorizontal ? delta : 0, isHorizontal ? 0 : delta, 0));
        }

        protected override void Swipe(SwipeDirection direction, int units)
        {
            int currentIndex;

            for (int u = 0; u < units; u++)
            {
                currentIndex = CurrentSocket.Index;

                if (direction == SwipeDirection.RIGHT || direction == SwipeDirection.DOWN)
                {
                    if (currentIndex + 1 < sockets.Count)
                    {
                        CurrentSocket = sockets[currentIndex + 1];
                    }
                }
                else
                {
                    if (currentIndex > 0)
                    {
                        CurrentSocket = sockets[currentIndex - 1];
                    }
                }
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
            objectContainer.transform.LocalReset();
        }
        protected override IEnumerator RepositionAllCR(float lerp, float delay)
        {
            canMove = false;

            float currentLerp = lerp;
            while (currentLerp < 0.99f)
            {
                currentLerp = Mathf.Lerp(currentLerp, 1, lerp);
                objectContainer.transform.localPosition = Vector3.Lerp(objectContainer.transform.localPosition, Vector3.zero, lerp);
                yield return new WaitForSeconds(delay);
            }
            objectContainer.transform.LocalReset();

            canMove = true;
        }
        #endregion
    }
}
