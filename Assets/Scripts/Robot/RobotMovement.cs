using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using adefagia.Graph;
using adefagia.PercobaanPathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.Robot
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
                Move();
            }
        }

        private bool Move()
        {
            var aStar = new AStar();
            var end = _gridManager.GetGridSelect();

            // Make sure grid is selected and on ground grid
            if (!end.IsUnityNull() && GridManager.CheckGround(end))
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

                    return true;
                }
            }
            
            Debug.LogWarning("Pathfinding failed");

            // If end is missing or pathfinding failed
            return false;
        }
        

        // Only move transform position
        private void MovePositionToGrid(Grid grid)
        {
            transform.position = grid.GetLocation();
        }

        IEnumerator MoveToPath(List<Grid> path)
        {
            foreach (var grid in path)
            {
                MovePositionToGrid(grid);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void DebugNeighborPosition(StringBuilder sb, string position, Graph.Grid grid)
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