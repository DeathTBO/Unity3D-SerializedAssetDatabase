using UnityEngine;
using UnityEditor;

namespace SerializedAssets.Storage.EditorExtensions
{
    public class SerializedAssetContext
    {
        [MenuItem("Assets/Create/Create Serialized Asset Database")]
        public static void CreateAsset()
        {
            SerializedAssetDatabase t = SerializedAssetDatabase.Instance; //This getter creates a new instance
        }

        [MenuItem("Assets/Add To Serialized Asset Database")]
        public static void AddAsset()
        {
            //This filters out non-scene assets
            Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            for (int i = 0; i < selectedAssets.Length; i++)
            {
                SerializedAssetDatabase.Instance.UpdateAsset(null, selectedAssets[i]);
            }
        }
    }
}
