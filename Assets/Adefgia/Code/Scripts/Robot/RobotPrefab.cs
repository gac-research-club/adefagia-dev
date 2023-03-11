using UnityEngine;

namespace adefagia.Robot
{
    [System.Serializable]
    public class RobotPrefab
    {
        public int id;
        public Vector2 location;
        public GameObject gameObject;

        [System.Serializable]
        public struct Status
        {
            public float healthPoint;
            public float attackDamage;
        }

        public Status status;
    }
}