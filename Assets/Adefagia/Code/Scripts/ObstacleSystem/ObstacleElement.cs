using UnityEngine;
using UnityEngine.Serialization;

namespace Adefagia.ObstacleSystem
{
    [CreateAssetMenu(fileName = "ObstacleElement", menuName = "Obstacle/ObstacleElement")]
    public class ObstacleElement : ScriptableObject
    {
        public string Name;
        public ObstacleType ObstacleType;
        public int MaxHitCount;
        public GameObject Prefab;
    }
    
    public enum ObstacleType {
        Destructible
    }
}