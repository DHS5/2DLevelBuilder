using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    public static class UIStack
    {
        private static Stack<GameObject> mainStack;
        public static GameObject TopObject => mainStack.Peek();

        /// <summary>
        /// Show a new GameObject on the main stack
        /// </summary>
        /// <param name="go">GameObject to show</param>
        public static void Show(GameObject go)
        {
            if (mainStack == null) mainStack = new();

            if (TopObject != null)
                TopObject.SetActive(false);

            go.SetActive(true);
            mainStack.Push(go);
        }
        /// <summary>
        /// Go back to the previous GameObject on the main stack
        /// </summary>
        public static void Back()
        {
            if (mainStack == null)
            {
                mainStack = new();
                return;
            }

            if (mainStack.Count > 0)
                mainStack.Pop().SetActive(false);

            if (TopObject != null)
                TopObject.SetActive(true);
        }
        /// <summary>
        /// Clears the main stack and potentially set the new base object
        /// </summary>
        /// <param name="go">New base object of the main stack</param>
        public static void Clear(GameObject go = null)
        {
            if (mainStack == null) mainStack = new();
            else
            {
                foreach (var item in mainStack)
                {
                    item.SetActive(false);
                }

                mainStack.Clear();
            }

            if (go != null)
            {
                go.SetActive(true);
                mainStack.Push(go);
            }
        }
    }
}
