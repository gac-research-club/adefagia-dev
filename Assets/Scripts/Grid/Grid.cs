using UnityEngine;

namespace Grid
{
    public class Grid
    {
        public int index;
        public float priority;
        public int weight;
        
        public Vector2 location;
        public State state;
        public Grid[] neighbors;
        public bool occupied;

        private GameObject _gameObject;
        private Material _material;

        public Grid(int index, Vector2 location, int weight = 1, float priority = 0)
        {
            this.index = index;
            this.location = location;
            this.priority = priority;
            this.weight = weight;
            
            state = State.Empty;
            occupied = false;
        }

        public Vector3 GetLocation(float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(location.x+x, 0f+y, location.y+z);
        }

        public bool IsEmpty()
        {
            return state == State.Empty;
        }

        public void SetGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void SetMaterial(Material material)
        {
            _material = material;
        }

        public void ChangeMaterial()
        {
            _gameObject.GetComponent<MeshRenderer>().material = _material;
        }

        public Grid Right => neighbors[0];
        public Grid Up => neighbors[1];
        public Grid Left => neighbors[2];
        public Grid Down => neighbors[3];
    }

    public enum State {
        Empty,
        Ground
    }
}