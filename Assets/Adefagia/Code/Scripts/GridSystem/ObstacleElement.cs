using UnityEngine;

namespace Adefagia.GridSystem
{
    [CreateAssetMenu(fileName = "ObstacleElement", menuName = "Obstacle/ObstacleElement")]
    public class ObstacleElement : ScriptableObject
    {
        public ObstacleType obstacleType;
        public GameObject prefab;
    }
    
    public enum ObstacleType {
        Fragile,
        Solid
    }
}