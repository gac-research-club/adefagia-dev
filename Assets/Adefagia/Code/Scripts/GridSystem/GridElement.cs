using UnityEngine;

namespace Adefagia.GridSystem
{
    [CreateAssetMenu(fileName = "GridElement", menuName = "Grid/GridElement")]
    public class GridElement : ScriptableObject
    {
        public GridType gridType;
        public GameObject prefab;
    }
    
    public enum GridType {
        Ground,
        Empty
    }
}