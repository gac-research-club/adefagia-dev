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
        
        public int X { get; }
        public int Y { get; }
    
        #endregion

        #region Constructor

        /*------------------------------------------------------------------------------------------------------------
         * Constructor
         *------------------------------------------------------------------------------------------------------------*/
        public Obstacle(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion


        public override string ToString()
        {
            return $"Obstacle (,)";
        }
    }

}