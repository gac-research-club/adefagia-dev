using UnityEngine;

namespace adefagia.Graph
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