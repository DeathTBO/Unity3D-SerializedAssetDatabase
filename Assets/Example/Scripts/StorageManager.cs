using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializedAssets.Storage;

public class StorageManager : MonoBehaviour
{
    public SerializedAssetDatabase serializedAssetDatabase;

    void Start()
    {
        SerializedAssetDatabase.Instance = serializedAssetDatabase;
    }
}
