using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.Images
{
    public static class ImageUtility
    {
        /// <summary>
        /// Returns a screenshot of the current screen
        /// (Recommended to wait for the end of the frame)
        /// </summary>
        /// <returns>Screenshot as Texture2D</returns>
        public static Texture2D Screenshot()
        {
            Texture2D screenshot = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();
            return screenshot;
        }
    }
}
