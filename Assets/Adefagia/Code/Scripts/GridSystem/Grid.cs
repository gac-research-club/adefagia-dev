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
        /*--------------------------------------------------------
         * Property
         *-------------------------------------------------------*/
        public int X { get; }
        public int Y { get; }
        
        public GridStatus Status { get; set; }
        public Dictionary<GridDirection, Grid> Neighbors { get; set; }
        public float Priority { get; set; }

        /*------------------------------------------------------------------------------------------------------------
         * Constructor
         *------------------------------------------------------------------------------------------------------------*/
        public Grid(int x, int y)
        {
            X = x;
            Y = y;

            Neighbors = new Dictionary<GridDirection, Grid>();
        }

        public void AddNeighbor(GridDirection gridDirection, Grid grid)
        {
            try
            {
                if (grid == null) throw new NullReferenceException("Grid null");
                
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
    }

    public enum GridDirection
    {
        Right,
        Up,
        Left,
        Down,
    }
}