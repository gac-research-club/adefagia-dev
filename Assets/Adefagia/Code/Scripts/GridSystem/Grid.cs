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
    }

    public enum GridDirection
    {
        Right,
        Up,
        Left,
        Down,
    }
}