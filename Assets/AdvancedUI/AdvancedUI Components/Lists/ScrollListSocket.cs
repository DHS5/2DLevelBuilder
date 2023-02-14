using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public class ScrollListSocket : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;

        public float Width 
        { 
            get { return rectTransform.rect.width; }
            set { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value); } 
        }
        public float Height 
        {
            get { return rectTransform.rect.height; }
            set { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value); } 
        }


        private ScrollListObject scrollListObject;

        public ScrollListObject ScrollListObject
        {
            get { return scrollListObject; }
            set { scrollListObject = value; ParentScrollListObject(); }
        }

        public int Index;

        private void ParentScrollListObject()
        {
            ScrollListObject.transform.SetParent(rectTransform, true);
        }
    }
}
