Serialized Asset Database for Unity3D
==================== 

## What Is It?

This repo contains a way to serialize/deserialize UnityEngine.Objects (assets) to Json.Net. It was developed using C#, and required Netwonsoft's Json.Net library to work.

## How Does It Work?

A scriptable object contains a serialized dictionary which stores paths as keys, and the assets as values. The SerializedAsset<T> class serializes only the path to the object. At runtime the asset is retrieved using this path as the dictionary key. It's handled automatically, and should be hassel free.

## How Do I Set It Up?

1. Simply import the Unity Package into your project. Also make sure you have the Json.Net library.

2. Drag the SerializedAssetDatabase prefab into the first scene.

3. To add assets to the database, select assets in the file tree, and select "Add to SerializedAssetList". You can also load all assets in the project by selecting the Database file and select "Load All Assets". This ignores Editor folders, the SerializedAssetDatabase folder, and .cs files by default.

4. For use in Json, create a SerializedAsset<T> field/property. Assign the SerializedAsset.Asset property to the asset in the editor. Any UnityEngine.Object assigned to the Asset property will automatically be added to the database.