using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MassFileCreator : EditorWindow
{
    const string MASS_FILE_PATH = "Assets/Test/MassFiles/";

    int createFileCount;

    bool processing;
    
    [MenuItem("/Test/Mass File Editor")]
    static void Init()
    {
        MassFileCreator window = GetWindow<MassFileCreator>();

        window.titleContent = new GUIContent("Mass File Editor");
    }

    void OnEnable()
    {
        
    }

    void OnGUI()
    {
        createFileCount = EditorGUILayout.IntField("Create File Count:", createFileCount);

        if (GUILayout.Button("Create Files") && !processing)
        {
            processing = true;
            CreateFiles(createFileCount);
            processing = false;
        }
    }

    void CreateFiles(int files)
    {
        if (!Directory.Exists(MASS_FILE_PATH))
            Directory.CreateDirectory(MASS_FILE_PATH);
        
        string file;
        
        while (files > 0)
        {
            file = Path.Combine(MASS_FILE_PATH, $"{files}.txt");
            
            if(!File.Exists(file))
                File.Create(file);
            
            files--;
        }
    }
}
