using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SerializedAssets.Storage
{
    [System.Serializable]
    public class SerializedAssetDictionary : ISerializationCallbackReceiver
    {
        [SerializeField]
        string[] paths;

        [SerializeField]
        Object[] assets;

        public Dictionary<string, Object> dictionary;

        //A reverse dictionary allows you to look up from key or value
        public Dictionary<Object, string> reverseDictionary;

        public int Count => dictionary.Count;

        public Object this[string path]
        {
            get
            {
                if (!ContainsPath(path))
                    return null;

                return dictionary[path];
            }
        }

        public string this[Object obj]
        {
            get
            {
                if (!ContainsAsset(obj))
                    return "";

                return reverseDictionary[obj];
            }
        }
        
        public string[] GetKeyList()
        {
            return dictionary.Keys.ToArray();
        }

        public SerializedAssetDictionary()
        {
            dictionary = new Dictionary<string, Object>();

            reverseDictionary = new Dictionary<Object, string>();
        }

        public void OnBeforeSerialize()
        {
            paths = new string[dictionary.Count];

            assets = new Object[dictionary.Count];

            int i = 0;

            foreach (string key in dictionary.Keys)
            {
                paths[i] = key;

                assets[i] = dictionary[key];

                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            dictionary = new Dictionary<string, Object>();

            reverseDictionary = new Dictionary<Object, string>();

            if (paths == null || paths.Length == 0 || assets == null || assets.Length == 0)
                return;

            //Used for in editor lookups
            Object asset = null;
            string path = "";
            
            for (int i = 0; i < paths.Length; i++)
            {
                if (assets[i] == null)
                {
                    #if UNITY_EDITOR

                    asset = AssetDatabase.LoadAssetAtPath<Object>(paths[i]);
                    
                    if(asset != null)
                        goto AssetFound; //Break the if statement
                    
                    #endif
                    
                    Debug.Log($"Serialized Asset Dictionary is missing asset at path: {paths[i]}");
                    continue;
                }
                
                AssetFound:

                if (string.IsNullOrWhiteSpace(paths[i]))
                {
                    #if UNITY_EDITOR

                    path = AssetDatabase.GetAssetPath(asset != null ? asset : assets[i]);

                    if(!string.IsNullOrWhiteSpace(path))
                        goto PathFound;

                    #endif
                    
                    Debug.Log($"Serialized Asset Dictionary is missing asset: {assets[i]}");
                    continue;
                }
                
                PathFound:
                
                dictionary.Add(paths[i], assets[i]);
                reverseDictionary.Add(assets[i], paths[i]);
            }
        }

        public void Add(string path, Object asset)
        {
            if (ContainsPath(path))
            {
                Debug.LogWarning($"Serialized Asset Dictionary Already Contains Path: {path}");
                return;
            }

            if (ContainsAsset(asset))
            {
                Debug.LogWarning($"Serialized Asset Dictionary Already Contains Asset: {asset}");
                return;
            }

            dictionary.Add(path, asset);

            reverseDictionary.Add(asset, path);
        }

        public void Remove(string path)
        {
            if (!dictionary.ContainsKey(path))
            {
                Debug.LogWarning($"The Serialized Dictionary Does Not Contain Path: {path}");
                return;
            }

            Object asset = dictionary[path];

            if (ContainsAsset(asset))
                reverseDictionary.Remove(asset);
            else
                Debug.LogError(
                    $"The Serialized Dictionary Does Not Contain Asset: {asset}.");

            dictionary.Remove(path);
        }

        public void Remove(Object asset)
        {
            if (!reverseDictionary.ContainsKey(asset))
            {
                Debug.LogWarning($"The Serialized Dictionary Does Not Contain Asset: {asset}");
                return;
            }

            string path = reverseDictionary[asset];

            if (ContainsPath(path))
                dictionary.Remove(path);
            else
                Debug.LogError(
                    $"The Serialized Dictionary Does Not Contain Path: {path}.");

            reverseDictionary.Remove(asset);
        }

        public void RemoveAll()
        {
            dictionary.Clear();

            reverseDictionary.Clear();
        }

        public Object UpdatePath(string oldPath, string newPath)
        {
            Object value = dictionary[oldPath];

            reverseDictionary[value] = newPath;

            dictionary.Remove(oldPath);

            dictionary.Add(newPath, value);

            return value;
        }

        public string UpdateAsset(Object oldAsset, Object newAsset)
        {
            string key = reverseDictionary[oldAsset];

            dictionary[key] = newAsset;

            reverseDictionary.Remove(oldAsset);

            reverseDictionary.Add(newAsset, key);

            return key;
        }

        public bool ContainsPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            
            return dictionary.ContainsKey(path);
        }

        public bool ContainsAsset(Object asset)
        {
            if (asset == null)
                return false;
            
            return reverseDictionary.ContainsKey(asset);
        }
    }
}
