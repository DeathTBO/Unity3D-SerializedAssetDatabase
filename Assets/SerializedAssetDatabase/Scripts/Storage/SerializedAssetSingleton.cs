using UnityEngine;

namespace SerializedAssets.Storage
{
    public class SerializedAssetSingleton : MonoBehaviour
    {
        public SerializedAssetDatabase serializedAssetDatabase;

        void Awake()
        {
            if (SerializedAssetDatabase.Instance != null)
            {
                gameObject.SetActive(false);
                return;
            }
        
            SerializedAssetDatabase.Instance = serializedAssetDatabase;
        }
    }
}
