using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Dhs5.Utility.SaveSystem
{
    public static class SaveSystem
    {
        #region JSON
        /// <summary>
        /// Save a serializable class at the given path
        /// </summary>
        /// <typeparam name="T">Any serializable class type</typeparam>
        /// <param name="classToSave">The class to save must be marked [Serializable]</param>
        /// <param name="filename">Filename without extension</param>
        /// <param name="path">Path of the file, starting from persistent data path (already included), must start and end with "/"</param>
        public static void SaveClassToJSON<T>(T classToSave, string filename, string path = "/")
        {
            string directoryPath = Application.persistentDataPath + path;
            SetUpDirectory(directoryPath);
            string completePath = directoryPath + filename + ".json";
            File.WriteAllText(completePath, JsonUtility.ToJson(classToSave));
        }

        /// <summary>
        /// Try to load a serialiable class at the given path
        /// </summary>
        /// <typeparam name="T">Any serializable class type</typeparam>
        /// <param name="classToLoad">The class to load must be marked [Serializable]</param>
        /// <param name="filename">Filename without extension</param>
        /// <param name="path">Path of the file, starting from persistent data path (already included), must start and end with "/"</param>
        /// <returns>Whether the JSON file exists</returns>
        public static bool TryLoadClassFromJSON<T>(out T classToLoad, string filename, string path = "/")
        {
            classToLoad = default(T);
            string completePath = Application.persistentDataPath + path + filename + ".json";
            bool exists = File.Exists(completePath);
            if (exists)
            {
                 classToLoad = JsonUtility.FromJson<T>(File.ReadAllText(completePath));
            }
            return exists;
        }
        public static void DeleteJSON(string filename, string path = "/")
        {
            string completePath = Application.persistentDataPath + path + filename + ".json";
            DeleteFile(completePath);
        }
        #endregion

        #region PNG
        /// <summary>
        /// Saves a Texture2D as a PNG at the given path
        /// </summary>
        /// <param name="texture">Texture2D to save</param>
        /// <param name="filename">Name of the file</param>
        /// <param name="path">Path of the file, starting from persistent data path if overridePath = false, must start and end with "/" either way</param>
        /// <param name="overridePath">Whether to override the path or start from persistent data path</param>
        public static void SaveTexture2D(Texture2D texture, string filename, string path = "/", bool overridePath = false)
        {
            string directoryPath = (overridePath ? "" : Application.persistentDataPath) + path;
            SetUpDirectory(directoryPath);
            string completePath = directoryPath + filename + ".png";
            File.WriteAllBytes(path, texture.EncodeToPNG());
        }

        /// <summary>
        /// Loads a Texture2D as a PNG at the given path
        /// </summary>
        /// <param name="texture">Texture2D to load</param>
        /// <param name="filename">Name of the file</param>
        /// <param name="path">Path of the file, starting from persistent data path if overridePath = false, must start and end with "/" either way</param>
        /// <param name="overridePath">Whether to override the path or start from persistent data path</param>
        /// <returns>Whether the PNG file exists</returns>
        public static bool TryLoadTexture2D(out Texture2D texture, string filename, string path = "/", bool overridePath = false)
        {
            string completePath = (overridePath ? "" : Application.persistentDataPath) + path + filename + ".png";
            texture = new Texture2D(Screen.width, Screen.height);
            bool exists = File.Exists(completePath);
            if (exists)
            {
                texture.LoadImage(File.ReadAllBytes(completePath));
                // Uploads changes to GPU
                texture.Apply(true, true);
            }
            return exists;
        }
        public static void DeletePNG(string filename, string path = "/", bool overridePath = false)
        {
            string completePath = (overridePath ? "" : Application.persistentDataPath) + path + filename + ".png";
            DeleteFile(completePath);
        }
        #endregion

        #region Private Useful Functions
        private static void SetUpDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        private static void DeleteFile(string completePath)
        {
            if (File.Exists(completePath))
            {
                File.Delete(completePath);
            }
        }
        #endregion

        // Save PNG and other type of files (simple info with playerPrefs etc...)
    }

    #region Save Classes

    [Serializable]
    public class SavesRepertory<T> where T : SaveClass
    {
        #region Constructor
        public SavesRepertory(string name = "SavesRepertory", string path = "/Saves/")
        {
            repertoryName = name;
            repertoryPath = path;

            Load();
        }
        #endregion

        #region Serialized properties
        [SerializeField] private string repertoryName;
        [SerializeField] private string repertoryPath;

        [SerializeField] private string lastSaveInfoName;

        [SerializeField] private Dico<string, SaveInfo<T>> saveInfos = new(0);
        #endregion

        #region Accessors
        public Dico<string, SaveInfo<T>> SaveInfos { get { return saveInfos; } }

        public SaveInfo<T> LastSaveInfo
        {
            get
            {
                if (TryGetLastSaveInfo(out SaveInfo<T> lastSaveInfo)) return lastSaveInfo;
                return null;
            }
        }
        #endregion

        #region Private properties
        private bool loaded = false;
        #endregion

        #region Public Functions

        // Modify Actions
        public void Add(string saveName, T save)
        {
            SaveInfo<T> saveInfo = new SaveInfo<T>(saveName, save, repertoryPath);
            Add(saveInfo);
            SaveSystem.SaveClassToJSON(save, saveInfo.filename, repertoryPath);
        }
        public bool Remove(string saveName)
        {
            if (!loaded)
            {
                Load();
                if (!loaded) return false;
            }
            if (saveInfos.Pop(saveName, out SaveInfo<T> saveInfo))
                SaveSystem.DeleteJSON(saveInfo.filename, saveInfo.filepath);
            Save();
            return true;
        }
        public bool Remove(SaveInfo<T> saveInfo) 
        {
            if (!loaded)
            {
                Load();
                if (!loaded) return false;
            }
            saveInfos.Remove(saveInfo.saveName);
            SaveSystem.DeleteJSON(saveInfo.filename, saveInfo.filepath);
            Save();
            return true;
        }

        // Get Infos Actions
        public bool TryGetInfo(string saveName, out SaveInfo<T> saveInfo)
        {
            return saveInfos.TryGet(saveName, out saveInfo);
        }
        public SaveInfo<T> GetInfo(string saveName)
        {
            if (TryGetInfo(saveName, out SaveInfo<T> saveInfo)) return saveInfo;
            return null;
        }
        /// <summary>
        /// Returns the last (by date) save info
        /// </summary>
        /// <returns>Last save info <b>! Can be null !</b></returns>
        public bool TryGetLastSaveInfo(out SaveInfo<T> lastSaveInfo)
        {
            if (string.IsNullOrWhiteSpace(lastSaveInfoName))
            {
                saveInfos.Sort((x, y) => x.saveDate.CompareTo(y.saveDate));
                lastSaveInfoName = saveInfos.Last.saveName;
            }
            return TryGetInfo(lastSaveInfoName, out lastSaveInfo);
        }
        public List<string> GetInfosNameList()
        {
            List<string> names = new();
            saveInfos.Sort((x, y) => x.saveDate.CompareTo(y.saveDate));
            foreach (Pair<string, SaveInfo<T>> entry in saveInfos)
            {
                names.Add(entry.key);
            }

            return names;
        }

        // Get Saves Actions
        public bool TryGetSave(string saveName, out T save)
        {
            save = null;
            bool gotInfo = TryGetInfo(saveName, out SaveInfo<T> saveInfo);
            if (gotInfo) return saveInfo.TryGetSave(out save);
            return false;
        }
        public T GetSave(string saveName)
        {
            TryGetSave(saveName, out T save);
            return save;
        }
        /// <summary>
        /// Returns the last (by date) save
        /// </summary>
        /// <returns>Last save <b>! Can be null !</b></returns>
        public bool TryGetLastSave(out T lastSave)
        {
            if (string.IsNullOrWhiteSpace(lastSaveInfoName))
            {
                saveInfos.Sort((x, y) => x.saveDate.CompareTo(y.saveDate));
                lastSaveInfoName = saveInfos.Last.saveName;
            }
            return TryGetSave(lastSaveInfoName, out lastSave);
        }
        public bool TryCopyFromSave(string saveName, T saveToCopyIn)
        {
            if (TryGetInfo(saveName, out SaveInfo<T> saveInfo))
            {
                return saveInfo.TryCopyFromSave(saveToCopyIn);
            }
            return false;
        }
        #endregion

        #region Private Functions
        private void Save()
        {
            saveInfos.Serialize();
            SaveSystem.SaveClassToJSON(this, repertoryName, repertoryPath);
        }
        private void Load()
        {
            if (SaveSystem.TryLoadClassFromJSON(out SavesRepertory<T> saves, repertoryName, repertoryPath))
            {
                Copy(saves);
                loaded = true;
            }
        }
        private void Copy(SavesRepertory<T> saves)
        {
            lastSaveInfoName = saves.lastSaveInfoName;
            saveInfos = saves.SaveInfos;
        }
        private void Add(SaveInfo<T> saveInfo)
        {
            if (!loaded)
            {
                Load();
                if (!loaded)
                {
                    saveInfos = new(0);
                }
            }
            saveInfos.Set(saveInfo.saveName, saveInfo);
            lastSaveInfoName = saveInfo.saveName;
            Save();
        }
        #endregion
    }

    [Serializable]
    public class SaveInfo<T> where T : SaveClass
    {
        #region Constructors
        public SaveInfo(string name, T save, string path = "/")
        {
            saveName = name;
            filename = name + "_SAVE";
            filepath = path;
            saveDate = Date.CurrentDate;
        }
        #endregion

        #region Properties
        public string saveName;
        public string filename;
        public string filepath;
        public Date saveDate;

        #endregion

        #region Accessors
        public T Save
        {
            get
            {
                if (TryGetSave(out T save)) return save;
                return null;
            }
        }
        #endregion

        #region Public Functions
        public bool TryGetSave(out T save)
        {
            return SaveSystem.TryLoadClassFromJSON(out save, filename, filepath);
        }
        public bool TryCopyFromSave(T saveToCopyIn)
        {
            bool canCopy = TryGetSave(out T save);
            if (canCopy) saveToCopyIn.Copy(save);
            return canCopy;
        }
        #endregion
    }
    /// <summary>
    /// Class to inherit to use the SaveRepertory system
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [Serializable]
    /// private class SaveClassTest : SaveClass
    /// {
    ///     // Properties to save here
    /// 
    ///     protected override void CopyOperation(SaveClass saveClass)
    ///     {
    ///         SaveClassTest saveClassTest = saveClass as SaveClassTest;
    ///         
    ///         // Implement the copy operation here
    ///     }
    /// }
    /// ]]>
    ///</code>
    /// </example>
    public abstract class SaveClass
    {
        #region Default Methods

        /// <summary>
        /// Copy the content of a SaveClass into another.
        /// Use this function to treat how you want the datas to be copied in your SaveClass rather than just doing an assignation.
        /// <b>Always check the type at the beginning of the implementation</b>
        /// </summary>
        /// <param name="saveClass">SaveClass instance to copy the datas from</param>
        public void Copy(SaveClass saveClass)
        {
            if (saveClass.GetType() != this.GetType())
            {
                Debug.LogError("Tried a SaveClass copy on wrong class type");
                return;
            }

            CopyOperation(saveClass);
        }

        protected abstract void CopyOperation(SaveClass saveClass);

        #endregion
    }


    #endregion

    #region Serializable Structs

    #region Vectors
    [Serializable] public struct Float4
    {
        // ### Constructors ###
        public Float4(float _x = 0, float _y = 0, float _z = 0, float _w = 0) { x = _x; y = _y; z = _z; w = _w; }
        public Float4(Quaternion quaternion) { x = quaternion.x; y = quaternion.y; z = quaternion.z; w = quaternion.w; }
        public Float4(Vector3 vector3) { x = vector3.x; y = vector3.y; z = vector3.z; w = 0; }
        public Float4(Vector2 vector2) { x = vector2.x; y = vector2.y; z = 0; w = 0; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, z, w); }
        public Vector3 GetVector3() { return new Vector3(x, y, z); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public float x;
        public float y;
        public float z;
        public float w;
    }
    [Serializable] public struct Float3
    {
        // ### Constructors ###
        public Float3(float _x = 0, float _y = 0, float _z = 0) { x = _x; y = _y; z = _z; }
        public Float3(Quaternion quaternion) { x = quaternion.x; y = quaternion.y; z = quaternion.z; }
        public Float3(Vector3 vector3) { x = vector3.x; y = vector3.y; z = vector3.z; }
        public Float3(Vector2 vector2) { x = vector2.x; y = vector2.y; z = 0; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, z, 0); }
        public Quaternion GetQuaternionEuler() { return Quaternion.Euler(x, y, z); }
        public Vector3 GetVector3() { return new Vector3(x, y, z); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public float x;
        public float y;
        public float z;
    }
    [Serializable] public struct Float2
    {
        // ### Constructors ###
        public Float2(float _x = 0, float _y = 0) { x = _x; y = _y; }
        public Float2(Quaternion quaternion) { x = quaternion.x; y = quaternion.y; }
        public Float2(Vector3 vector3) { x = vector3.x; y = vector3.y; }
        public Float2(Vector2 vector2) { x = vector2.x; y = vector2.y; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, 0, 0); }
        public Quaternion GetQuaternionEuler() { return Quaternion.Euler(x, y, 0); }
        public Vector3 GetVector3() { return new Vector3(x, y, 0); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public float x;
        public float y;
    }
    [Serializable] public struct Int4
    {
        // ### Constructors ###
        public Int4(int _x = 0, int _y = 0, int _z = 0, int _w = 0) { x = _x; y = _y; z = _z; w = _w; }
        public Int4(Quaternion quaternion) { x = (int)quaternion.x; y = (int)quaternion.y; z = (int)quaternion.z; w = (int)quaternion.w; }
        public Int4(Vector3Int vector3) { x = vector3.x; y = vector3.y; z = vector3.z; w = 0; }
        public Int4(Vector2Int vector2) { x = vector2.x; y = vector2.y; z = 0; w = 0; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, z, w); }
        public Vector3Int GetVector3Int() { return new Vector3Int(x, y, z); }
        public Vector3 GetVector3() { return new Vector3(x, y, z); }
        public Vector2Int GetVector2Int() { return new Vector2Int(x, y); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public int x;
        public int y;
        public int z;
        public int w;
    }
    [Serializable] public struct Int3
    {
        // ### Constructors ###
        public Int3(int _x = 0, int _y = 0, int _z = 0) { x = _x; y = _y; z = _z; }
        public Int3(float fill) { x = (int)fill; y = (int)fill; z = (int)fill; }
        public Int3(Quaternion quaternion) { x = (int)quaternion.x; y = (int)quaternion.y; z = (int)quaternion.z; }
        public Int3(Vector3Int vector3) { x = vector3.x; y = vector3.y; z = vector3.z; }
        public Int3(Vector2Int vector2) { x = vector2.x; y = vector2.y; z = 0; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, z, 0); }
        public Quaternion GetQuaternionEuler() { return Quaternion.Euler(x, y, z); }
        public Vector3Int GetVector3Int() { return new Vector3Int(x, y, z); }
        public Vector3 GetVector3() { return new Vector3(x, y, z); }
        public Vector2Int GetVector2Int() { return new Vector2Int(x, y); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public int x;
        public int y;
        public int z;
    }
    [Serializable] public struct Int2
    {
        // ### Constructors ###
        public Int2(int _x = 0, int _y = 0) { x = _x; y = _y; }
        public Int2(float fill) { x = (int)fill; y = (int)fill; }
        public Int2(Quaternion quaternion) { x = (int)quaternion.x; y = (int)quaternion.y; }
        public Int2(Vector3Int vector3) { x = vector3.x; y = vector3.y; }
        public Int2(Vector2Int vector2) { x = vector2.x; y = vector2.y; }

        // ### Getters ###
        public Quaternion GetQuaternion() { return new Quaternion(x, y, 0, 0); }
        public Quaternion GetQuaternionEuler() { return Quaternion.Euler(x, y, 0); }
        public Vector3Int GetVector3Int() { return new Vector3Int(x, y, 0); }
        public Vector3 GetVector3() { return new Vector3(x, y, 0); }
        public Vector2Int GetVector2Int() { return new Vector2Int(x, y); }
        public Vector2 GetVector2() { return new Vector2(x, y); }

        // ### Properties ###
        public int x;
        public int y;
    }
    #endregion
    #region Date
    [Serializable] public struct Date
    {
        // ### Constructors ###
        public Date(DateTime date)
        {
            year = date.Year;
            month = date.Month;
            day = date.Day;
            hour = date.Hour;
            minute = date.Minute;
            second = date.Second;
        }
        public Date(int _year = 0, int _month = 1, int _day = 1, int _hour = 0, int _minute = 0, int _second = 0)
        {
            year = _year;
            month = Mathf.Clamp(_month, 1, 12);
            day = Mathf.Clamp(_day, 1, DateTime.DaysInMonth(year, month));
            hour = Mathf.Clamp(_hour, 0, 23);
            minute = Mathf.Clamp(_minute, 0, 59);
            second = Mathf.Clamp(_second, 0, 59);
        }

        // ### Getters ###
        public DateTime GetDate()
        {
            return new DateTime(year, month, day, hour, minute, second);
        }
        public string GetStringFormated()
        {
            return GetDate().ToString("dd-MM-yyyy HH:mm");
        }

        // ### Public Functions ###
        public int CompareTo(Date other)
        {
            return GetDate().CompareTo(other.GetDate());
        }

        // ### Static Functions ###
        public static Date CurrentDate { get { return new Date(DateTime.Now); } }

        // ### Properties ###
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;
    }
    #endregion
    #region Pairs
    [Serializable] public struct Pair<T,U>
    {
        public Pair(T _key, U _value) { key = _key; value = _value; }

        // ### Properties ###
        public T key;
        public U value;
    }
    #endregion
    #region Dictionnaries
    [Serializable]
    public struct Dico<T,U> : ISerializationCallbackReceiver, IEnumerable
    {
        #region Constructors
        /// <summary>
        /// Creates a new Dico
        /// </summary>
        /// <param name="zero">Useless, no impact</param>
        public Dico(int zero)
        {
            dictionary = new();
            pairs = new();
        }
        public Dico(Dictionary<T,U> _dictionary)
        {
            dictionary = _dictionary;
            pairs = new();
            Serialize();
        }
        public Dico(List<Pair<T,U>> _pairs)
        {
            dictionary = new();
            pairs = _pairs;
            SetUpWithPairs();
        }
        #endregion

        #region Accessors
        public Dictionary<T, U> GetDictionary { get { return dictionary; } }
        public bool Exists { get { return dictionary != null; } }

        // List accessors
        public U First { get { return pairs[0].value; } }
        public U Last { get { return pairs[pairs.Count - 1].value; } }
        #endregion

        #region Public Functions
        /// <summary>
        /// Resets the Dico
        /// </summary>
        public void Reset()
        {
            dictionary = new();
            pairs = new();
        }
        /// <summary>
        /// Sets up the Dico from the lists
        /// (Use this function to populate the Dico on Awake/Start after populating the lists in the inspector)
        /// </summary>
        public void SetUpWithPairs()
        {
            dictionary = new();
            for (int i = 0; i < pairs.Count; i++)
            {
                dictionary[pairs[i].key] = pairs[i].value;
            }
        }
        /// <summary>
        /// Serializes the Dico with the Dictionary entries
        /// (Used to populate the lists before Serialization)
        /// </summary>
        public void Serialize()
        {
            pairs = new();
            foreach (KeyValuePair<T, U> entry in dictionary)
            {
                pairs.Add(new(entry.Key, entry.Value));
            }
        }
        public void Set(T key, U value) 
        { 
            if (!Exists) dictionary = new();
            dictionary[key] = value;
            Serialize();
        }
        public bool TryAdd(T key, U value) { return dictionary.TryAdd(key, value); }
        public void Remove(T key)
        {
            if (!Exists) return;
            dictionary.Remove(key);
            Serialize();
        }
        public bool Pop(T key, out U value)
        {
            bool canPop = TryGet(key, out value);
            Remove(key);
            return canPop;
        }
        public U Get(T key) { return dictionary[key]; }
        public bool TryGet(T key, out U value) 
        {
            value = default(U);
            if (!Exists) return false;
            return dictionary.TryGetValue(key, out value); 
        }
        public bool ContainsKey(T key) 
        {
            if (!Exists) return false;
            return dictionary.ContainsKey(key); 
        }
        public bool ContainsValue(U value) 
        {
            if (!Exists) return false;
            return dictionary.ContainsValue(value); 
        }
        public U Find(Predicate<U> match)
        {
            Serialize();
            return pairs.Find(x => match(x.value)).value;
        }
        public void Sort(Comparison<U> comparison)
        {
            Serialize();
            pairs.Sort((x, y) => comparison(x.value, y.value));
        }
        #endregion

        #region Serialization Callbacks
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            SetUpWithPairs();
        }
        #endregion

        #region Enumerable
        public IEnumerator GetEnumerator()
        {
            foreach (Pair<T,U> entry in pairs)
            {
                yield return entry;
            }
        }
        #endregion

        // ### Properties ###
        Dictionary<T, U> dictionary;

        [SerializeField] List<Pair<T,U>> pairs;
    }
    #endregion

    #endregion
}
