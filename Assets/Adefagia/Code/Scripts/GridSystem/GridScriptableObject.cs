using UnityEngine;

namespace Adefagia.GridSystem
{
    [CreateAssetMenu(fileName = "Grid", menuName = "ScriptableObjects/Grid")]
    public class GridScriptableObject : ScriptableObject
    {
        public GridPrefab gridPrefab;
    }
    
}
