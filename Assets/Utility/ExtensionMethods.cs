using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace Dhs5.Utility
{
    public static class ExtensionMethods
    {
        #region Transform

        /// <summary>
        /// Resets the transform in world space
        /// </summary>
        /// <param name="transform"></param>
        public static void WorldReset(this Transform transform)
        {
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
        }
        
        /// <summary>
        /// Resets the transform in local space
        /// </summary>
        /// <param name="transform"></param>
        public static void LocalReset(this Transform transform)
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Sets the world position of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public static void SetWorldPosition(this Transform transform, float x = 0, float y = 0, float z = 0)
        {
            transform.position = new Vector3(x, y, z);
        }
        /// <summary>
        /// Resets the world position of the transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ResetWorldPosition(this Transform transform)
        {
            transform.position = Vector3.zero;
        }
        /// <summary>
        /// Sets the local position of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public static void SetLocalPosition(this Transform transform, float x = 0, float y = 0, float z = 0)
        {
            transform.localPosition = new Vector3(x, y, z);
        }
        /// <summary>
        /// Resets the local position of the transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ResetLocalPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Sets the world rotation of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x">X euler rotation</param>
        /// <param name="y">Y euler rotation</param>
        /// <param name="z">Z euler rotation</param>
        public static void SetWorldEulerRotation(this Transform transform, float x = 0, float y = 0, float z = 0)
        {
            transform.rotation = Quaternion.Euler(x, y, z);
        }
        /// <summary>
        /// Resets the world rotation of the transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ResetWorldRotation(this Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }
        /// <summary>
        /// Sets the local rotation of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x">X euler rotation</param>
        /// <param name="y">Y euler rotation</param>
        /// <param name="z">Z euler rotation</param>
        public static void SetLocalEulerRotation(this Transform transform, float x = 0, float y = 0, float z = 0)
        {
            transform.localRotation = Quaternion.Euler(x, y, z);
        }
        /// <summary>
        /// Resets the local rotation of the transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ResetLocalRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Sets the local scale of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x">X scale</param>
        /// <param name="y">Y scale</param>
        /// <param name="z">Z scale</param>
        public static void SetLocalScale(this Transform transform, float x = 1, float y = 1, float z = 1)
        {
            transform.localScale = new Vector3(x, y, z);
        }
        /// <summary>
        /// Resets the local scale of the transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ResetLocalScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }
        #endregion

        #region RectTransform
        /// <summary>
        /// Sets the anchor of this rect transform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="anchor">Anchor type</param>
        /// <param name="resetPos">Whether to reset the position of the rect transform</param>
        public static void SetAnchor(this RectTransform rectTransform, RectTransformAnchor anchor, bool resetPos = false)
        {
            switch (anchor)
            {
                case RectTransformAnchor.MIDDLE_CENTER:
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    break;
                case RectTransformAnchor.TOP_CENTER:
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    rectTransform.anchorMin = new Vector2(0.5f, 1);
                    break;
                case RectTransformAnchor.BOTTOM_CENTER:
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    break;
                case RectTransformAnchor.MIDDLE_LEFT:
                    rectTransform.anchorMax = new Vector2(0, 0.5f);
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    break;
                case RectTransformAnchor.TOP_LEFT:
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.anchorMin = new Vector2(0, 1);
                    break;
                case RectTransformAnchor.BOTTOM_LEFT:
                    rectTransform.anchorMax = new Vector2(0, 0);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    break;
                case RectTransformAnchor.MIDDLE_RIGHT:
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    break;
                case RectTransformAnchor.TOP_RIGHT:
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.anchorMin = new Vector2(1, 1);
                    break;
                case RectTransformAnchor.BOTTOM_RIGHT:
                    rectTransform.anchorMax = new Vector2(1, 0);
                    rectTransform.anchorMin = new Vector2(1, 0);
                    break;
                case RectTransformAnchor.FULL_STRETCH:
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    break;
                default:
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    break;
            }

            if (resetPos) rectTransform.anchoredPosition = new Vector2(0,0);
        }
        /// <summary>
        /// Sets the anchored size of the rect transform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="width">Width of the rect transform</param>
        /// <param name="height">Height of the rect transform</param>
        /// <param name="setWidth">Whether to set the width</param>
        /// <param name="setHeight">Whether to set the height</param>
        public static void SetSize(this RectTransform rectTransform,
            float width, float height, bool setWidth = true, bool setHeight = true)
        {
            if (setWidth)
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            if (setHeight)
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        /// <summary>
        /// Get the size of the rect transform as a Vector2
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns>Vector2 representing width and height of the rect transform</returns>
        public static Vector2 GetSize(this RectTransform rectTransform)
        {
            return new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
        #endregion

        #region List
        public static int ValidIndex<T>(this List<T> list, int index)
        {
            while (index < 0) index += list.Count;

            return index % list.Count;
        }
        public static T Get<T>(this List<T> list, int index)
        {
            return list[list.ValidIndex(index)];
        }
        public static T Last<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
        #endregion

        #region LayerMask
        public static LayerMask Everything(this LayerMask mask)
        {
            return ~0;
        }
        public static LayerMask Nothing(this LayerMask mask)
        {
            return 0;
        }
        /// <summary>
        /// Whether a layer is included in this LayerMask
        /// </summary>
        /// <param name="mask">This mask</param>
        /// <param name="layer">The layer to compare</param>
        /// <returns>True if the layer is included</returns>
        public static bool Include(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) != 0;
        }
        /// <summary>
        /// Whether a layer is excluded from this LayerMask
        /// </summary>
        /// <param name="mask">This mask</param>
        /// <param name="layer">The layer to compare</param>
        /// <returns>True if the layer is excluded</returns>
        public static bool Exclude(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) == 0;
        }
        #endregion

        #region Editor

#if UNITY_EDITOR
        // Gets value from SerializedProperty - even if value is nested
        public static object GetParent(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;

            FieldInfo field = null;

            string[] paths = property.propertyPath.Split('.');
            for (int i = 0; i < paths.Length - 1; i++)
            {
                var type = obj.GetType();
                field = type.GetField(paths[i]);
                obj = field.GetValue(obj);
            }
            return obj;
        }
        
        public static object GetParentField(this SerializedProperty property, string fieldName)
        {
            object obj = property.serializedObject.targetObject;

            FieldInfo field = null;

            string[] paths = property.propertyPath.Split('.');
            for (int i = 0; i < paths.Length - 1; i++)
            {
                var type = obj.GetType();
                field = type.GetField(paths[i]);
                obj = field.GetValue(obj);
            }
            var type2 = obj.GetType();
            field = type2.GetField(fieldName);
            if (field == null) return null;
            obj = field.GetValue(obj);
            return obj;
        }
#endif

        #endregion
    }

    public enum RectTransformAnchor
    {
        MIDDLE_CENTER, TOP_CENTER, BOTTOM_CENTER,
        MIDDLE_LEFT, TOP_LEFT, BOTTOM_LEFT,
        MIDDLE_RIGHT, TOP_RIGHT, BOTTOM_RIGHT,

        FULL_STRETCH
    }
}
