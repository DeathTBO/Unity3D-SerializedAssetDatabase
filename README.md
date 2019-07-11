Serialized Asset Database for Unity3D
==================== 

## What Is It?

This repo contains a way to load UnityEngine.Objects (assets) with a path relative to the assets folder. The SerializedAsset<T> class allows for easy use with Json.Net.

## How Does It Work?

A scriptable object contains a serialized dictionary which stores paths as keys, and the assets as values. These assets can be loaded via "SerializedAssetDatabase.Instance.LoadAsset<T>(path)" where the path is relative to the assets folder. For example, "Assets/MyGameAssets/Player.prefab". It will return null if no asset was found.

The SerializedAsset<T> class serializes only the path to the object. At runtime the asset is retrieved using this path as the dictionary key, and assigns the asset as type T to the the "SerializedAsset.Asset" property. This is handled in the getter. It will return null if no asset was found.

## How Do I Set It Up?

1. Simply import the Unity Package from [Releases](https://github.com/DeathTBO/Unity3D-SerializedAssetDatabase/releases) into your project. Also make sure you have the Json.Net library.

2. Drag the SerializedAssetDatabase prefab into the first scene.

3. To add assets to the database, select assets in the file tree, and select "Add to SerializedAssetList". You can also load all assets in the project by selecting the Database file and select "Load All Assets". This ignores Editor folders, the SerializedAssetDatabase folder, and .cs files by default.

4. For use in Json, create a SerializedAsset<T> field/property. Assign the SerializedAsset.Asset property to the asset in the editor. Any UnityEngine.Object assigned to the Asset property will automatically be added to the database.