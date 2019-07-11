using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using SerializedAssets.Storage;
using UnityEditor;
using UnityEngine;

public class CustomClassEditor : EditorWindow
{
	const string CUSTOM_CLASS_PATH = "Assets/Example/CustomClasses.json";
	
	[SerializeField]
	List<CustomClass> customClasses;
	
	[MenuItem("/Test/Custom Class Editor")]
	static void Init()
	{
		CustomClassEditor window = GetWindow<CustomClassEditor>();

		window.titleContent = new GUIContent("Custom Class Editor");
	}

	void OnEnable()
	{
		customClasses = new List<CustomClass>();

		if (!File.Exists(CUSTOM_CLASS_PATH))
			return;
		
		string fileContents = File.ReadAllText(CUSTOM_CLASS_PATH);

		customClasses = JsonConvert.DeserializeObject<List<CustomClass>>(fileContents);
	}

	void OnGUI()
	{
		for (int i = 0; i < customClasses.Count; i++)
		{
			EditorGUILayout.BeginVertical("Box");
			
			customClasses[i].Name = EditorGUILayout.TextField("Name:", customClasses[i].Name);
			
			customClasses[i].Points = EditorGUILayout.IntField("Points:", customClasses[i].Points);

			customClasses[i].Prefab.EditorGUI();
			
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginHorizontal("Box");

		if (GUILayout.Button("Save"))
			Save();
		
		if (GUILayout.Button("Add"))
			customClasses.Add(new CustomClass());
		
		EditorGUILayout.EndHorizontal();
	}

	void Save()
	{
		string serializedData = JsonConvert.SerializeObject(customClasses);
		
		File.WriteAllText(CUSTOM_CLASS_PATH, serializedData);
	}
}
