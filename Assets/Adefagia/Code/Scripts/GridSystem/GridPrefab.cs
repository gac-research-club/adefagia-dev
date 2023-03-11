using UnityEngine;

namespace Adefagia.GridSystem
{
    [System.Serializable]
    public class GridPrefab
    {
        [System.Serializable]
        public struct DataPrefab
        {
            public GridType gridType;
            public GameObject prefab;
        }

        public DataPrefab dataPrefab;
    }
}