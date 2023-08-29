using System;
using System.Collections.Generic;
using UnityEngine;

using Adefagia.SelectObject;
using Adefagia.RobotSystem;

namespace Adefagia.GridSystem
{
    public enum GridStatus
    {
        Free,
        Robot,
        Obstacle,
    }
    
    public class Grid
    {
        #region Properties
        
        public int X { get; }
        public int Y { get; }
        
        public GridStatus Status { get; private set; }
        public Dictionary<GridDirection, Grid> Neighbors { get; }
        public float Priority { get; set; }
        public Vector2Int Location => new (X, Y);
 
        #endregion

        #region Constructor

        /*------------------------------------------------------------------------------------------------------------
         * Constructor
         *------------------------------------------------------------------------------------------------------------*/
        public Grid(int x, int y)
        {
            X = x;
            Y = y;

            Neighbors = new Dictionary<GridDirection, Grid>();
        }

        #endregion

        public void AddNeighbor(GridDirection gridDirection, Grid grid)
        {
            try
            {
                if (grid == null) throw new NullReferenceException("Grid null");
                
                // Add neighbor by direction
                Neighbors.Add(gridDirection, grid);
            }
            // Error null reference
            catch (NullReferenceException) {}
        }
        
        public Grid GetNeighbor(GridDirection gridDirection)
        {
            try
            {
                return Neighbors[gridDirection];
            }
            // If key not found
            catch (KeyNotFoundException)
            {
                return null;
            } 
        }

        public void SetOccupied()
        {
            Status = GridStatus.Robot;
        }

        public void SetObstacle(){
            Status = GridStatus.Obstacle;
        }

        public void SetFree()
        {
            Status = GridStatus.Free;
        }

        public static bool IsOccupied(Grid grid)
        {
            return grid.Status == GridStatus.Robot;
        }

        public override string ToString()
        {
            return $"Grid ({X}, {Y})";
        }
        
        public static GridDirection GetDirection(Grid start, Grid destination)
        {
            var direction = GetVectorDirection(start, destination);
            if (direction == new Vector2Int(1, 0))
            {
                return GridDirection.Right;
            }

            if (direction == new Vector2Int(-1, 0))
            {
                return GridDirection.Left;
            }
            if (direction == new Vector2Int(0, 1))
            {
                return GridDirection.Up;
            }
            return GridDirection.Down;
        }
        
        public static Vector2Int GetVectorDirection(Grid start, Grid destination)
        {
            var result = ((Vector2)destination.Location - start.Location) / (Heuristic(start, destination));

            var x = result.x == 0 ? 0 : result.x / Mathf.Abs(result.x);
            var y = result.y == 0 ? 0 :result.y / Mathf.Abs(result.y);
            
            return new Vector2Int((int)x, (int)y);
        }
        
        public static int Heuristic(Grid start, Grid destination)
        {
            return Mathf.Abs(destination.X - start.X) + Mathf.Abs(destination.Y - start.Y);
        }
    }

    public enum GridDirection
    {
        Right,
        Up,
        Left,
        Down,
    }
}