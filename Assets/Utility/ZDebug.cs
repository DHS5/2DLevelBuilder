using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Dhs5.Utility
{
    public enum ZDebugGroup
    {
        ALL = 0,

        TEST,

        NONE = 100
    }

    public static class ZDebug
    {
        #region Private Helpers
        private static string ArrayAsString<T>(T[] list, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            return sb.ToString();
        }
        private static string ArrayAsString<T>(T[][] list, string separator = " ", string lineSeparator = "\n")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(ArrayAsString(item, separator));
                sb.Append(lineSeparator);
            }
            return sb.ToString();
        }
        #endregion

        #region Simple Debug
        public static void Log(object obj)
        {
            Debug.Log(obj);
        }
        public static void Log(string str)
        {
            Debug.Log(str);
        }
        public static void Log(string separator, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            Log(sb.ToString());
        }
        public static void Log<T>(T[] list, bool sameLine = false, string separator = " ")
        {
            if (sameLine)
            {
                Log(ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                Log(item);
            }
        }
        public static void Log<T>(T[][] list, bool allSameLine = false, string lineSeparator = "\n", 
            bool sameLine = true, string separator = " ")
        {
            if (allSameLine)
            {
                Log(ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                Log(item, sameLine, separator);
            }
        }
        public static void Log<T>(T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    Log(sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) Log(sb.ToString());
        }

        public static void Log<T>(List<T> list, bool sameLine = false, string separator = " ")
        {
            Log(list.ToArray(), sameLine, separator);
        }

        public static void LogFamily(Transform t)
        {
            StringBuilder sb = new();
            sb.Append(t);
            while (t != t.root)
            {
                sb.Append(" <- ");
                t = t.parent;
                sb.Append(t);
            }
            Log(sb.ToString());
        }
#endregion
        
        #region Debug Warning
        public static void LogW(object obj)
        {
            Debug.LogWarning(obj);
        }
        public static void LogW(string str)
        {
            Debug.LogWarning(str);
        }
        public static void LogW(string separator, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            LogW(sb.ToString());
        }
        public static void LogW<T>(T[] list, bool sameLine = false, string separator = " ")
        {
            if (sameLine)
            {
                LogW(ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                LogW(item);
            }
        }
        public static void LogW<T>(T[][] list, bool allSameLine = false, string lineSeparator = "\n", 
            bool sameLine = true, string separator = " ")
        {
            if (allSameLine)
            {
                LogW(ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                LogW(item, sameLine, separator);
            }
        }
        public static void LogW<T>(T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    LogW(sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) LogW(sb.ToString());
        }

        public static void LogW<T>(List<T> list, bool sameLine = false, string separator = " ")
        {
            LogW(list.ToArray(), sameLine, separator);
        }
#endregion
        
        #region Debug Error
        public static void LogE(object obj)
        {
            Debug.LogError(obj);
        }
        public static void LogE(string str)
        {
            Debug.LogError(str);
        }
        public static void LogE(string separator, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            LogE(sb.ToString());
        }
        public static void LogE<T>(T[] list, bool sameLine = false, string separator = " ")
        {
            if (sameLine)
            {
                LogE(ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                LogE(item);
            }
        }
        public static void LogE<T>(T[][] list, bool allSameLine = false, string lineSeparator = "\n", 
            bool sameLine = true, string separator = " ")
        {
            if (allSameLine)
            {
                LogE(ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                LogE(item, sameLine, separator);
            }
        }
        public static void LogE<T>(T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    LogE(sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) LogE(sb.ToString());
        }

        public static void LogE<T>(List<T> list, bool sameLine = false, string separator = " ")
        {
            LogE(list.ToArray(), sameLine, separator);
        }
        #endregion

        #region Debug Assertion
        public static void LogA(bool condition, object obj)
        {
            if (!condition)
                Debug.LogAssertion(obj);
        }
        public static void LogA(bool condition, string str)
        {
            if (!condition)
                Debug.LogAssertion(str);
        }
        public static void LogA(bool condition, string separator, params object[] objs)
        {
            if (condition) return;
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            LogE(sb.ToString());
        }
        public static void LogA<T>(bool condition, T[] list, bool sameLine = false, string separator = " ")
        {
            if (condition) return;
            if (sameLine)
            {
                LogE(ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                LogE(item);
            }
        }
        public static void LogA<T>(bool condition, T[][] list, bool allSameLine = false, string lineSeparator = "\n",
            bool sameLine = true, string separator = " ")
        {
            if (condition) return;
            if (allSameLine)
            {
                LogE(ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                LogE(item, sameLine, separator);
            }
        }
        public static void LogA<T>(bool condition, T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            if (condition) return;
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    LogE(sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) LogE(sb.ToString());
        }

        public static void LogA<T>(bool condition, List<T> list, bool sameLine = false, string separator = " ")
        {
            LogA(condition, list.ToArray(), sameLine, separator);
        }

        public static void LogAFamily(bool condition, ZDebugGroup group, Transform t)
        {
            if (condition) return;
            StringBuilder sb = new();
            sb.Append(t);
            while (t != t.root)
            {
                sb.Append(" <- ");
                t = t.parent;
                sb.Append(t);
            }
            LogE(sb.ToString());
        }
        #endregion

        #region Debug Group
        public static void LogG(ZDebugGroup group, object obj)
        {
            if (UtilityStates.ZDebugGroupsContains(group))
                Debug.Log(obj);
        }
        public static void LogG(ZDebugGroup group, string str)
        {
            if (UtilityStates.ZDebugGroupsContains(group))
                Debug.Log(str);
        }
        public static void LogG(ZDebugGroup group, string separator, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            LogG(group, sb.ToString());
        }
        public static void LogG<T>(ZDebugGroup group, T[] list, bool sameLine = false, string separator = " ")
        {
            if (sameLine)
            {
                LogG(group, ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                LogG(group, item);
            }
        }
        public static void LogG<T>(ZDebugGroup group, T[][] list, bool allSameLine = false, string lineSeparator = "\n",
            bool sameLine = true, string separator = " ")
        {
            if (allSameLine)
            {
                LogG(group, ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                LogG(group, item, sameLine, separator);
            }
        }
        public static void LogG<T>(ZDebugGroup group, T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    LogG(group, sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) LogG(group, sb.ToString());
        }

        public static void LogG<T>(ZDebugGroup group, List<T> list, bool sameLine = false, string separator = " ")
        {
            LogG(group, list.ToArray(), sameLine, separator);
        }

        public static void LogGFamily(ZDebugGroup group, Transform t)
        {
            StringBuilder sb = new();
            sb.Append(t);
            while (t != t.root)
            {
                sb.Append(" <- ");
                t = t.parent;
                sb.Append(t);
            }
            LogG(group, sb.ToString());
        }
        #endregion

        #region Debug Screen

        private static ZDebugCanvas debugCanvas;
        private static ZDebugCanvas DebugCanvas
        {
            get
            {
                if (debugCanvas == null)
                {
                    debugCanvas = GameObject.Instantiate(Resources.Load("ZDebug Canvas") as GameObject)
                        .GetComponent<ZDebugCanvas>();
                    if (debugCanvas == null) LogE("ZDebug Canvas can't be found");
                    else debugCanvas.transform.SetAsLastSibling();
                }
                return debugCanvas;
            }
        }
        public static Color ScreenLogColor 
        { get { return UtilityStates.OnScreendebugColor; } set { UtilityStates.OnScreendebugColor = value; } }
        public static float ScreenLogTime 
        { get { return UtilityStates.OnScreendebugTime; } set { UtilityStates.OnScreendebugTime = value; } }

        public static void LogS(object obj)
        {
            Log(obj);
            DebugCanvas.LogOnScreen(obj.ToString());
        }
        public static void LogS(string str)
        {
            Log(str);
            DebugCanvas.LogOnScreen(str);
        }
        public static void LogS(string separator, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objs)
            {
                sb.Append(item);
                sb.Append(separator);
            }
            LogS(sb.ToString());
        }
        public static void LogS<T>(T[] list, bool sameLine = false, string separator = " ")
        {
            if (sameLine)
            {
                LogS(ArrayAsString(list));
                return;
            }
            foreach (var item in list)
            {
                LogS(item);
            }
        }
        public static void LogS<T>(T[][] list, bool allSameLine = false, string lineSeparator = "\n",
            bool sameLine = true, string separator = " ")
        {
            if (allSameLine)
            {
                LogS(ArrayAsString(list, separator, lineSeparator));
                return;
            }
            foreach (var item in list)
            {
                LogS(item, sameLine, separator);
            }
        }
        public static void LogS<T>(T[,] list, bool allSameLine = true, string lineSeparator = "\n", string separator = " ")
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int j = 0; j < list.GetLength(1); j++)
                {
                    sb.Append(list[i, j].ToString());
                    sb.Append(separator);
                }
                if (allSameLine)
                    sb.Append(lineSeparator);
                else
                {
                    LogS(sb.ToString());
                    sb.Clear();
                }
            }
            if (allSameLine) LogS(sb.ToString());
        }

        public static void LogS<T>(List<T> list, bool sameLine = false, string separator = " ")
        {
            LogS(list.ToArray(), sameLine, separator);
        }

        public static void LogSFamily(Transform t)
        {
            StringBuilder sb = new();
            sb.Append(t);
            while (t != t.root)
            {
                sb.Append(" <- ");
                t = t.parent;
                sb.Append(t);
            }
            LogS(sb.ToString());
        }
        #endregion
    }
}
