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
        public Vector2 endLocation;
        
        private Grid _grid;
        private Grid _robotLastGrid;
        private GridManager _gridManager;

        private Robot _robot;

        // public bool teamActive;

        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
        }
        
        void Update()
        {
            
            // if (active)
            // {
            //     if (teamActive)
            //     {
            //         // GameManager.instance.spawnManager.SelectRobot(_robot);
            //     }
            // }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeGridIdle(_grid.Right);
                MovePositionToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ChangeGridIdle(_grid.Up);
                MovePositionToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ChangeGridIdle(_grid.Left);
                MovePositionToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ChangeGridIdle(_grid.Down);
                MovePositionToGrid(_grid);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Move(endLocation);
            }
        }

        public void SetRobot(Robot robot)
        {
            _robot = robot;
        }

        public bool Move(Vector2 location)
        {
            var aStar = new AStar();
            var end = _gridManager.GetGridByLocation(location);
                    
            if(GridManager.IsGridEmpty(end))
            {
                Debug.LogWarning("Grid not found", this);
                return false;
            }

            // if (end.IsOccupied())
            // {
            //     Debug.LogWarning("Grid is occupied", this);
            //     return false;
            // }

            if (aStar.Pathfinding(_grid, end))
            {
                
                var path = aStar.Traversal(_grid, end);
                StartCoroutine(MoveToPath(path));

                aStar.DebugListGrid(path);
                _robotLastGrid = path[^1];

                return true; 
            }

            _robotLastGrid = aStar.GetFirstOccupied();
            
            if (_robotLastGrid.IsUnityNull())
            {
                Debug.LogWarning("Jalan Buntu");
                return false;
            }
            
            var blocked = aStar.Traversal(_grid, _robotLastGrid);
            StartCoroutine(MoveToPath(blocked));

            Debug.LogWarning("Blocked by other");
            return true;
        }

        public void ChangeGridIdle(Grid grid)
        {
            if (GridManager.IsGridEmpty(grid)) return;
        
            _grid = grid;
        }

        public Grid GetLast()
        {
            return _robotLastGrid;
        }

        public void MovePositionToGrid(Grid grid)
        {
            transform.position = grid.GetLocation(y:1);
        }

        IEnumerator MoveToPath(List<Grid> path)
        {
            foreach (var grid in path)
            {
                ChangeGridIdle(grid);
                MovePositionToGrid(grid);

                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private void OnGUI()
        {
            // if(!teamActive) return;
            // if(!active) return;
            // if (!GridManager.doneGenerateGrids) return;
            if (_grid.IsUnityNull()) return;
        
            // var textLeft = $"Berdiri {_grid.location.x}, {_grid.location.y}"; 
            // GUI.Box (new Rect (10,Screen.height-10-50,100,50), textLeft);
        
            var text = "";
        
            StringBuilder textTopRight = new StringBuilder();
            textTopRight.Append("Neighboors: ");
            DebugNeighborPosition(textTopRight, "Kanan", _grid.Right);
            DebugNeighborPosition(textTopRight, "Atas", _grid.Up);
            DebugNeighborPosition(textTopRight, "Kiri", _grid.Left);
            DebugNeighborPosition(textTopRight, "Bawah", _grid.Down);
        
            // text = $"Node {_grid.id} = ({_grid.location.x},{_grid.location.y})";

            // Make a background box
            GUI.Box(new Rect(10, 10, 100, 50), text);
            GUI.Box (new Rect (Screen.width - 10 - 100,10,100,100), textTopRight.ToString());
        
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