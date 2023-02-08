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
}
