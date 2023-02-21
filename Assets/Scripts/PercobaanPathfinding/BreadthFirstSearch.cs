using System.Collections;
using System.Collections.Generic;
using System.Text;
using adefagia.Collections;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.PercobaanPathfinding
{
    public class BreadthFirstSearch : MonoBehaviour
    {
        public Material frontierMaterial, neighborMaterial, reachedMaterial, groundMaterial;

        public Vector2 startLocation, goalLocation;
        public KeyCode startKey;

        // Frontier
        private PriorityQueueMin _frontierQueue;

        // Reached
        private List<Grid> _reached;
        private Dictionary<Grid, Grid> _cameFrom;

        private List<Grid> _path;

        private bool hasPathfindingTimelapse;

        private GridManager _gridManager;

        void Start()
        {
            _frontierQueue = new PriorityQueueMin();

            _reached = new List<Grid>();
            _cameFrom = new Dictionary<Grid, Grid>();

            _path = new List<Grid>();

            _gridManager = GameManager.instance.gridManager;
        }


        void Update()
        {
            // if (!GridManager.doneGenerateGrids) return;
            
            if (Input.GetKeyDown(startKey) && !hasPathfindingTimelapse)
            {
                // if (!GridManager.doneGenerateGrids) return;
                hasPathfindingTimelapse = true;
                
                ClearBFS();
                ClearPath();
                StartCoroutine(BFSTimelapse());
            }

        }

        void BFS()
        {
            // Starting point
            Graph.Grid current = _gridManager.GetGridByLocation(startLocation);
            _frontierQueue.Insert(current);

            while (_frontierQueue.size > 0)
            {
                // Get from queue
                current = _frontierQueue.DeleteMin();
                
                HasReached(current);

                // Neighbors
                foreach (var next in current.neighbors)
                {
                    // Check if already on reached
                    if (GridManager.IsGridEmpty(next)) continue;
                    if (_reached.Contains(next)) continue;
                    if (_frontierQueue.Contains(next)) continue;

        
                    // var heuristic = Heuristic(next.location, startLocation);
                    // next.priority = heuristic;
                    
                    // Debug.Log($"Grid {next.location.x},{next.location.y} heuristic : {heuristic}");
                    
                    _frontierQueue.Insert(next);
                    _cameFrom.Add(next, current);
                }
                
                // _frontierQueue.DebugListIndex();

                // current.ChangeMaterial(reachedMaterial);
            }
            
            Debug.Log("Done");
        }

        IEnumerator BFSTimelapse()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            // Starting point
            Graph.Grid current = _gridManager.GetGridByLocation(startLocation);
            Graph.Grid goal = _gridManager.GetGridByLocation(goalLocation);
            
            if(goal.IsUnityNull() || current.IsUnityNull()) yield break;
            
            _frontierQueue.Insert(current);

            while (_frontierQueue.size > 0)
            {
                // Get from queue
                current = _frontierQueue.DeleteMin();
                
                // Done
                if (current.Equals(goal))
                {
                    Debug.Log("Done BFS");
                    yield return StartCoroutine(BFSPathfinding(startLocation, goalLocation));
                    yield break;
                }
                
                HasReached(current);

                // Neighbors
                foreach (var next in current.neighbors)
                {
                    // Check if already on reached
                    if (GridManager.IsGridEmpty(next)) continue;
                    if (_reached.Contains(next)) continue;
                    if (_frontierQueue.Contains(next)) continue;

                    // var heuristic = Heuristic(next.location, goalLocation);
                    // next.priority = heuristic;
                    
                    // Debug.Log($"Grid {next.location.x},{next.location.y} heuristic : {heuristic}");
                    
                    _frontierQueue.Insert(next);

                    _cameFrom.Add(next, current);
                    
                    // next.ChangeMaterial(neighborMaterial);

                    yield return wait;
                }
                
                // _frontierQueue.DebugListIndex();

                // current.ChangeMaterial(reachedMaterial);
                
                yield return wait;
            }
        }

        void HasReached(Grid grid)
        {
            _reached.Add(grid);

            // Set material
            // grid.ChangeMaterial(frontierMaterial);
        }

        void ClearBFS()
        {
            foreach (var grid in _cameFrom)
            {
                // grid.Value.ChangeMaterial(groundMaterial);
            }

            while (_frontierQueue.size > 0)
            {
                var grid = _frontierQueue.DeleteMin();
                // grid.ChangeMaterial(groundMaterial);
            }

            _frontierQueue.Clear();
            _reached.Clear();
            _cameFrom.Clear();
        }

        void ClearPath()
        {
            foreach (var grid in _path)
            {
                // grid.ChangeMaterial(groundMaterial);
            }
            _path.Clear();
        }

        IEnumerator BFSPathfinding(Vector2 startLoc, Vector2 goalLoc)
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            var start = _gridManager.GetGridByLocation(startLoc);
            var current = _gridManager.GetGridByLocation(goalLoc);
            
            if(start.IsUnityNull() || current.IsUnityNull()) yield break;

            while (!current.Equals(start))
            {
                _path.Add(current);
                current = _cameFrom[current];
            }
            
            _path.Add(start);
            
            _path.Reverse();
            
            foreach (var grid in _path)
            {
                // grid.ChangeMaterial(frontierMaterial);
                
                yield return wait;
            }
            
            Debug.Log("Done. Langkah: " + _path.Count);
            hasPathfindingTimelapse = false;
        }

        private void DebugListGrid(List<Grid> grids)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var grid in grids)
            {
                // sb.Append(grid.id + " ");
            }
            sb.Append("]");
                
            Debug.Log(sb);
        }

        private float Heuristic(Vector2 a, Vector2 b)
        {
            // Manhattan distance on a square grid
            return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);

            // Pythagoras
            // return Vector2.Distance(a, b);
        }
    }
}
