using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using SerializedAssets.Storage;

public class Spawner : MonoBehaviour
{
    public TextAsset jsonText;

    List<CustomClass> customClasses;
    
    void Start()
    {
        customClasses = JsonConvert.DeserializeObject<List<CustomClass>>(jsonText.text);
        
        for (int i = 0; i < customClasses.Count; i++)
        {
            //Using the SerializedAsset
            if (customClasses[i].Prefab.Asset == null)
                continue;
            
            Transform t = Instantiate(customClasses[i].Prefab.Asset).transform;

            t.name = customClasses[i].Name;
            
            t.position = new Vector3(1.5f * i, 0f, 0f);
        }

        //Loading the asset directly
        GameObject resourcePrefab = SerializedAssetDatabase.Instance.LoadAssetAtPath<GameObject>("");

        if (resourcePrefab == null)
            return;

        Transform resourceTransform = Instantiate(resourcePrefab).transform;

        resourceTransform.position = new Vector3(1.5f * customClasses.Count, 0f, 0f);
    }
}
