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
        private GridManager _gridManager;

        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
        }
        
        void Update()
        {
            if (!Spawner.doneSpawn) return;

            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeGridBerdiri(_grid.Right);
                MoveToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ChangeGridBerdiri(_grid.Up);
                MoveToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ChangeGridBerdiri(_grid.Left);
                MoveToGrid(_grid);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ChangeGridBerdiri(_grid.Down);
                MoveToGrid(_grid);
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                Move(endLocation);
            }
        }

        public void Move(Vector2 location)
        {
            var aStar = new AStar();
            var end = _gridManager.GetGridByLocation(location);
                    
            if(GridManager.IsGridEmpty(end))
            {
                Debug.LogWarning("Grid not found", this);
                return;
            }

            aStar.Pathfinding(_grid, end);
                
            var path = aStar.Traversal(_grid, end);
            StartCoroutine(MoveToPath(path));
        }

        public void ChangeGridBerdiri(Grid grid)
        {
            if (GridManager.IsGridEmpty(grid)) return;
        
            _grid = grid;
        }

        void MoveToGrid(Grid grid)
        {
            transform.position = grid.GetLocation( );
        }

        IEnumerator MoveToPath(List<Grid> path)
        {
            foreach (var grid in path)
            {
                ChangeGridBerdiri(grid);
                MoveToGrid(grid);

                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private void OnGUI()
        {
        
            if (!GridManager.doneGenerateGrids) return;
            if (_grid.IsUnityNull()) return;
        
            var textLeft = $"Berdiri {_grid.location.x}, {_grid.location.y}"; 
            GUI.Box (new Rect (10,Screen.height-10-50,100,50), textLeft);
        
            var text = "";
        
            StringBuilder textTopRight = new StringBuilder();
            textTopRight.Append("Neighboors: ");
            DebugNeighborPosition(textTopRight, "Kanan", _grid.Right);
            DebugNeighborPosition(textTopRight, "Atas", _grid.Up);
            DebugNeighborPosition(textTopRight, "Kiri", _grid.Left);
            DebugNeighborPosition(textTopRight, "Bawah", _grid.Down);
        
            text = $"Node {_grid.index} = ({_grid.location.x},{_grid.location.y})";

            // Make a background box
            GUI.Box(new Rect(10, 10, 100, 50), text);
            GUI.Box (new Rect (Screen.width - 10 - 100,10,100,100), textTopRight.ToString());
        
        }

        private void DebugNeighborPosition(StringBuilder sb, string position, Graph.Grid grid)
        {
            try
            {
                sb.Append($"\n{position}: {grid.state}");
            }
            catch (NullReferenceException)
            {
                sb.Append($"\n{position}: Null");
            }
        }
    }
}