using System;
using System.Collections.Generic;
using UnityEngine;

using Adefagia.SelectObject;
using Adefagia.RobotSystem;

namespace Adefagia.GridSystem
{
  
    public class Obstacle
    {
        #region Properties
        
        private Grid _grid;
        public Grid Location => _grid; 
    
        #endregion

        #region Constructor

        /*------------------------------------------------------------------------------------------------------------
         * Constructor
         *------------------------------------------------------------------------------------------------------------*/
        public Obstacle(Grid grid)
        {
            _grid = grid;
        }

        #endregion

        public void SetObstacle()
        {
            _grid.SetObstacle();
        }


        public void SetFree()
        {
           _grid.SetFree();
        }

        public override string ToString()
        {
            return $"Obstacle ({_grid.X}, {_grid.Y})";
        }
    }

}