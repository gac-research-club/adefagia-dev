using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.Helper
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public static class DirectionHelper
    {
        public static Direction GetOppositeDirectionTo(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                    break;
                case Direction.Down:
                    return Direction.Up;
                    break;
                case Direction.Left:
                    return Direction.Right;
                    break;
                case Direction.Right:
                    return Direction.Left;
                    break;
                default:
                    return direction;
                    break;
            }
        }
    }
}
