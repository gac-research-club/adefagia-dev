using UnityEngine;

namespace Playground.Grid
{
    public class Grid
    {
        public int index;
        public Vector2 location;
        public State state;
        public Grid[] neighboors;
        public bool occupied;

        public Grid()
        {
            state = State.Empty;
        }

        public Vector3 GetLocation()
        {
            return new Vector3(location.x, 0.5f, location.y);
        }

        public Grid Right => neighboors[0];
        public Grid Up => neighboors[1];
        public Grid Left => neighboors[2];
        public Grid Down => neighboors[3];
    }

    public enum State {
        Empty,
        Ground
    }
}