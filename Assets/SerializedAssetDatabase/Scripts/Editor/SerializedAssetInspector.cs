//This inspector class is pretty heavy on performance
//Testing over 1000 entries made the window very laggy
//For now the minimize button allows you to clean/regenerate
//Todo: Fix performance on SerializedAssetInspector

using UnityEngine;
using UnityEditor;

namespace SerializedAssets.Storage.EditorExtensions
{
    [CustomEditor(typeof(SerializedAssetDatabase))]
    public class SerializedAssetInspector : Editor
    {
        SerializedAssetDatabase database;

        Object tempObject;

        string[] keys;

        bool minimize;

        public void OnEnable()
        {
            database = target as SerializedAssetDatabase;

            if (SerializedAssetDatabase.Instance == null)
                SerializedAssetDatabase.Instance = database;
            
            keys = database.serializedAssets.GetKeyList();

            minimize = true;
        }

        public override void OnInspectorGUI()
        {
            bool reloadKeys = false;
            
            EditorGUILayout.BeginVertical();

            if (!minimize)
            {
                if (GUILayout.Button("Minimize List"))
                {
                    minimize = true;
                }
                
                for (int i = 0; i < keys.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.TextField(keys[i], GUILayout.MinWidth(100f));

                    Object assetObject = database.serializedAssets[keys[i]];

                    assetObject =
                        EditorGUILayout.ObjectField(assetObject, typeof(Object), false, GUILayout.MinWidth(100f));

                    string assetPath = AssetDatabase.GetAssetPath(assetObject);

                    if (!database.serializedAssets.ContainsAsset(assetObject) || database.serializedAssets[assetObject] != assetPath)
                    {
                        database.UpdateAsset(database.serializedAssets[keys[i]], assetObject);
                    }

                    if (GUILayout.Button("X"))
                    {
                        database.Remove(keys[i]);

                        reloadKeys = true;
                    }

                    EditorGUILayout.EndHorizontal();

                    if (GUI.changed)
                        reloadKeys = true;
                }

                EditorGUILayout.Space();
            }
            else
            {
                if (GUILayout.Button("Maximize List"))
                {
                    minimize = false;
                }
            }

            EditorGUILayout.BeginHorizontal();

            tempObject = EditorGUILayout.ObjectField(tempObject, typeof(Object), false, GUILayout.MinWidth(100f));

            if (GUILayout.Button("Add Item"))
            {
                if (tempObject != null && !database.serializedAssets.ContainsAsset(tempObject))
                {
                    database.serializedAssets.Add(AssetDatabase.GetAssetPath(tempObject), tempObject);

                    tempObject = null;

                    reloadKeys = true;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Clean"))
            {
                reloadKeys = true;
                database.Clean();
            }
            
            if (GUILayout.Button("Load All Assets"))
            {
                reloadKeys = true;
                database.RegenerateProjectAssets();
            }

            EditorGUILayout.Space(15f);

            if (GUILayout.Button("Remove All Assets"))
            {
                reloadKeys = true;
                database.RemoveAll();
            }

            EditorGUILayout.LabelField($"Asset Count: {keys.Length}");

            EditorGUILayout.EndVertical();

            if (reloadKeys)
            {
                EditorUtility.SetDirty(database);
                
                keys = database.serializedAssets.GetKeyList();
            }
        }
    }
}
