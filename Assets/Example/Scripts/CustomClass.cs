using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using SerializedAssets;

[Serializable]
public class CustomClass
{
    [SerializeField]
    public string name;
    
    [SerializeField]
    public string Name
    {
        get;
        set;
    }

    [SerializeField]
    public int Points
    {
        get;
        set;
    }

    [SerializeField]
    public SerializedAsset<GameObject> Prefab
    {
        get;
        set;
    }

    public CustomClass()
    {
        name = "No";
        Prefab = new SerializedAsset<GameObject>();
    }
}
