using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SerializedAssets.Storage
{
    [System.Serializable]
    public class SerializedAssetDatabase : ScriptableObject
    {
        const string INSTANCE_PATH = @"Assets/SerializedAssetDatabase/Data/SerializedAssetDatabase.asset";

        const string LOG_DIRECTORY_PATH = @"Assets/SerializedAssetDatabase/Logs/";
        
        const string LOG_PATH = LOG_DIRECTORY_PATH + "CleanUp.log";

        static SerializedAssetDatabase instance;

        public static SerializedAssetDatabase Instance
        {
            get
            {
#if UNITY_EDITOR
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<SerializedAssetDatabase>(INSTANCE_PATH);

                    if (instance == null)
                    {
                        AssetDatabase.CreateAsset(CreateInstance<SerializedAssetDatabase>(), INSTANCE_PATH);

                        instance = AssetDatabase.LoadAssetAtPath<SerializedAssetDatabase>(INSTANCE_PATH);
                    }
                }
#endif

                return instance;
            }

            set
            {
#if UNITY_EDITOR
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<SerializedAssetDatabase>(INSTANCE_PATH);

                    if (instance == null)
                    {
                        AssetDatabase.CreateAsset(CreateInstance<SerializedAssetDatabase>(), INSTANCE_PATH);

                        instance = AssetDatabase.LoadAssetAtPath<SerializedAssetDatabase>(INSTANCE_PATH);
                    }
                }
#endif

                instance = value;
            }
        }
        
        [SerializeField]
        public SerializedAssetDictionary serializedAssets;
        
        public SerializedAssetDatabase()
        {
            serializedAssets = new SerializedAssetDictionary();
        }

        public void Save()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) //Don't make any data changes while the editor is in play mode
                return;

            EditorUtility.SetDirty(this);

            AssetDatabase.SaveAssets();
#endif
        }

        public void Clean()
        {
#if UNITY_EDITOR
            string log = "";

            string[] keys = serializedAssets.dictionary.Keys.ToArray();
            
            foreach (string path in keys)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    log += "Removing Blank Asset Path at " + path + System.Environment.NewLine;

                    serializedAssets.Remove(path);
                    continue;
                }
                
                if (serializedAssets[path] != null || AssetDatabase.LoadAssetAtPath<Object>(path) != null)
                    continue;

                log += "Removing Missing Asset at " + path + System.Environment.NewLine;

                serializedAssets.Remove(path);
            }

            if (log.Length == 0)
            {
                Debug.Log("Nothing to clean!");
                return;
            }

            Debug.Log($"Clean up has been logged to {LOG_PATH}");

            if (!Directory.Exists(LOG_DIRECTORY_PATH))
                Directory.CreateDirectory(LOG_DIRECTORY_PATH);
            
            File.WriteAllText(LOG_PATH, log);
#endif
        }

        public void Remove(string key)
        {
            serializedAssets.Remove(key);
        }

        public void Remove(Object asset)
        {
            serializedAssets.Remove(asset);
        }
        
        public void RemoveAll()
        {
            serializedAssets.RemoveAll();
        }
        
        public void RegenerateProjectAssets(SerializedAssetDatabaseBlacklist blacklist = null)
        {
            #if UNITY_EDITOR

            if(blacklist == null)
                blacklist = SerializedAssetDatabaseBlacklist.Default;
            
            serializedAssets = new SerializedAssetDictionary();
            
            foreach (string file in GetAssetFiles(@"Assets/", blacklist))
            {
                serializedAssets.Add(file, AssetDatabase.LoadAssetAtPath(file, typeof(Object)));
            }
            
            #endif
        }

        public void UpdateAsset<T>(T oldAsset, T newAsset) where T : Object
        {
#if UNITY_EDITOR

            if (EditorApplication.isPlaying) //Don't make any data changes while the editor is in play mode
                return;

            if (newAsset == null) //If you are un-assigning an asset, don't make changes to the asset list
                return;

            string oldPath;
            string newPath = AssetDatabase.GetAssetPath(newAsset);

            if (serializedAssets.ContainsAsset(newAsset)) //If the new asset already exists, update its path
            {
                if (!serializedAssets.ContainsPath(newPath)) //As long as the new path doesn't already exist
                    serializedAssets.UpdatePath(serializedAssets[oldAsset], newPath);

                return;
            }

            if (oldAsset == newAsset && serializedAssets.ContainsAsset(oldAsset)) //If the assets are the same, update both the asset and path
            {
                oldPath = serializedAssets.UpdateAsset(oldAsset, newAsset);

                serializedAssets.UpdatePath(oldPath, newPath);

                return;
            }

            serializedAssets.Add(newPath, newAsset); //Add the new asset entry
#endif
        }

        public T LoadAssetAtPath<T>(string path) where T : Object
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            if (!serializedAssets.ContainsPath(path))
            {
                //If you're in the editor, check to see if the asset does exist, but simply hasn't been loaded.
                //If it does add back into the database.
            #if UNITY_EDITOR
                T obj = AssetDatabase.LoadAssetAtPath<T>(path);

                if (obj != null)
                {
                    serializedAssets.Add(path, obj);

                    Debug.Log($"Added missing Serialized Asset at path: {path}");
                }
                else
                {
                    Debug.Log($"Asset missing at path: {path}.");
                }

                return obj;
            #endif
                
                return null;
            }

            return serializedAssets[path] as T;
        }
        
        public string GetAssetPath(Object asset)
        {
            if (asset == null)
                return "";

            if (!serializedAssets.ContainsAsset(asset))
            {
            #if UNITY_EDITOR
                string path = AssetDatabase.GetAssetPath(asset);

                if (string.IsNullOrWhiteSpace(path))
                {
                    serializedAssets.Add(path, asset);

                    Debug.Log($"Added missing Serialized Asset at path: {path}");
                }
                else
                {
                    Debug.Log($"Asset missing at path: {path}.");
                }
                
                return path;
            #endif
                
                return null;
            }

            return serializedAssets[asset];
        }

        static IEnumerable GetAssetFiles(string baseDir, SerializedAssetDatabaseBlacklist blacklist)
        {
            string[] fileNames = Directory.GetFiles(baseDir);

            for (int i = 0; i < fileNames.Length; i++)
            {
                if(fileNames[i].EndsWith(".meta"))
                    continue;
                
                if(blacklist.CheckFile(fileNames[i]))
                    continue;
                
                yield return fileNames[i];
            }

            string[] directoryNames = Directory.GetDirectories(baseDir);

            for (int i = 0; i < directoryNames.Length; i++)
            {
                if(blacklist.CheckDirectory(directoryNames[i]))
                    continue;
                
                foreach(string file in GetAssetFiles(directoryNames[i], blacklist))
                    yield return file;
            }
        }
    }
}
