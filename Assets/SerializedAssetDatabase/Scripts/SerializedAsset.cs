using System;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using SerializedAssets.Storage;
using Object = UnityEngine.Object;

namespace SerializedAssets
{
    [System.Serializable]
    public class SerializedAsset<T> where T : Object
    {
        //[SerializeField]
        [JsonIgnore]
        [System.NonSerialized]
        T asset;
    
        [JsonIgnore]
        public T Asset
        {
            get
            {
                if (asset != null)
                    return asset;

                if (string.IsNullOrWhiteSpace(AssetPath))
                    return null;
            
                if(SerializedAssetDatabase.Instance != null)
                    asset = SerializedAssetDatabase.Instance.LoadAssetAtPath<T>(AssetPath);

                if (asset == null)
                {
                    Debug.LogError($"Asset loading failed. Missing serialized asset. Attempted to load asset at path: {AssetPath}{Environment.NewLine}Try regenerating the Serialized Asset Database or adding the asset manually.");
                    AssetPath = "";
                }

                return asset;
            }

            set
            {
                if (value == null)
                {
                    asset = value;
                    
                    AssetPath = "";

                    return;
                }

                if (SerializedAssetDatabase.Instance == null)
                {
                    Debug.LogError(
                        $"No instance of SerializedAssetDatabase. Make sure you have the SerializedAssetDatabase prefab in the first scene.{Environment.NewLine}If the problem persists try changing the script execution order, making SerializedAssetSingleton initialized earlier.");
                    return;
                }

                SerializedAssetDatabase.Instance.UpdateAsset(asset, value);
                
                AssetPath = SerializedAssetDatabase.Instance.GetAssetPath(value);
                
                if(!string.IsNullOrWhiteSpace(AssetPath))
                    asset = value;
            }
        }
    
        [SerializeField]
        public string AssetPath
        {
            get;
            set;
        }
    
        public SerializedAsset()
        {
            AssetPath = "";

            asset = null;
        }

        public SerializedAsset(T t)
        {
            if (t == null)
            {
                asset = null;

                AssetPath = "";
                return;
            }

            AssetPath = SerializedAssetDatabase.Instance.GetAssetPath(t);

            asset = t;
        }

        public SerializedAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                asset = null;

                AssetPath = "";
                return;
            }
        
            AssetPath = path;

            asset = SerializedAssetDatabase.Instance.LoadAssetAtPath<T>(path);
        }
        
        public void EditorGUI()
        {
#if UNITY_EDITOR
            Asset = EditorGUILayout.ObjectField("Asset:", Asset, typeof(T), false) as T;
#endif
        }
    }
}
