Serialized Asset Database for Unity3D
==================== 

##What Is It?

This repo contains a way to serialize/deserialize UnityEngine.Objects (assets) to Json.Net. It was developed using C#, and required Netwonsoft's Json.Net library to work.

##How Does It Work?

A scriptable object contains a serialized dictionary which stores paths as keys, and the assets as values. The SerializedAsset<T> class serializes only the path to the object. At runtime the asset is retrieved using this path as the dictionary key. It's handled automatically, and should be hassel free.

##How Do I Set It Up?

1. Simply import the Unity Package into your project. Also make sure you have the Json.Net library.

2. Drag the SerializedAssetDatabase prefab into the first scene.

3. Create a SerializedAsset<T> field/property in any class you want to serialize to Json. Assign the SerializedAsset.Asset property to the asset. Everything is else handled automatically.