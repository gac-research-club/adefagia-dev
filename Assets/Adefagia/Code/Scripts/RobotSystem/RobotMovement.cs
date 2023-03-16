using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.Collections;

// Using DOTween Plugin
// using DG.Tweening;


namespace Adefagia.RobotSystem
{
    public class RobotMovement : MonoBehaviour
    {
        public Robot Robot { private get; set; }

        private GridManager _gridManager;

        // public bool teamActive;

        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;

        }
        
        void Update()
        {
            // Testing
            if (Input.GetKeyDown(KeyCode.P) && Robot.IsSelect)
            {
                Move(GridManager.GetGrid());
            }
        }

        public bool Move(Grid end)
        {
            var aStar = new AStar();

            // Make sure grid is selected and on ground grid
            if (end != null && GridManager.CheckGround(end))
            {
                // If path finding success
                if (aStar.Pathfinding(Robot.Grid, end))
                {
                    // get path
                    var path = aStar.Traversal(Robot.Grid, end);
                    
                    // Robot move to destination path
                    StartCoroutine(MoveToPath(path));
                    
                    // clear occupied start grid
                    Robot.Grid.Free();
                    
                    // Change robot grid to end, then Occupy it
                    Robot.Move(end);
                    Robot.Grid.Occupy();
                    
                    Robot.ResetDefaultGrid();
                    Robot.SetGridRange();

                    return true;
                }
            }
            
            Debug.LogWarning("Pathfinding failed");

            // If end is missing or pathfinding failed
            return false;
        }


        /*----------------------------------------------------------------------------------------------------/
         * MovePositionToGrid(Grid grid)
         * Move the Robot into spesific the Grid. Movement is using the transform.position.
         * Adding transition on movement with DOTween
         * params: 
         *  - Grid destinationGrid  // Grid can be got from function GetGridByLocation(vector2 location)
         *  
         *----------------------------------------------------------------------------------------------------*/
        private void MovePositionToGrid(Grid grid)
        {
            transform.position = grid.GetLocation();
        }

        private void RotateRobot(Grid grid)
        {
            transform.LookAt(grid.GetLocation());
        }

        IEnumerator MoveToPath(List<Grid> path)
        {
            foreach (var grid in path)
            {
                RotateRobot(grid);
                MovePositionToGrid(grid);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void DebugNeighborPosition(StringBuilder sb, string position, Grid grid)
        {
            try
            {
                // sb.Append($"\n{position}: {grid.gridType}");
            }
            catch (NullReferenceException)
            {
                sb.Append($"\n{position}: Null");
            }
        }
    }
}