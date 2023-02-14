using System.Collections;
using System.Collections.Generic;
using System.Text;
using Collections;
using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace PercobaanPathfinding
{
    public class BreadthFirstSearch : MonoBehaviour
    {
        public Material frontierMaterial, neighborMaterial, reachedMaterial, groundMaterial;

        public Vector2 startLocation, goalLocation;
        public KeyCode startKey;

        // Frontier
        private PriorityQueueMin _frontierQueue;

        // Reached
        private List<Grid.Grid> _reached;
        private Dictionary<Grid.Grid, Grid.Grid> _cameFrom;

        private List<Grid.Grid> _path;

        private bool hasPathfindingTimelapse;

        void Start()
        {
            _frontierQueue = new PriorityQueueMin();

            _reached = new List<Grid.Grid>();
            _cameFrom = new Dictionary<Grid.Grid, Grid.Grid>();

            _path = new List<Grid.Grid>();
        }


        void Update()
        {
            if (!GridManager.doneGenerateGrids) return;
            
            if (Input.GetKeyDown(startKey) && !hasPathfindingTimelapse)
            {
                if (!GridManager.doneGenerateGrids) return;
                hasPathfindingTimelapse = true;
                
                ClearBFS();
                ClearPath();
                StartCoroutine(BFSTimelapse());
            }

        }

        void BFS()
        {
            // Starting point
            Grid.Grid current = GridManager.instance.GetGridByLocation(startLocation);
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

        
                    var heuristic = Heuristic(next.location, startLocation);
                    next.priority = heuristic;
                    
                    // Debug.Log($"Grid {next.location.x},{next.location.y} heuristic : {heuristic}");
                    
                    _frontierQueue.Insert(next);
                    _cameFrom.Add(next, current);
                    
                    next.SetMaterial(neighborMaterial);
                    next.ChangeMaterial();
                }
                
                // _frontierQueue.DebugListIndex();

                current.SetMaterial(reachedMaterial);
                current.ChangeMaterial();
            }
            
            Debug.Log("Done");
        }

        IEnumerator BFSTimelapse()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            // Starting point
            Grid.Grid current = GridManager.instance.GetGridByLocation(startLocation);
            Grid.Grid goal = GridManager.instance.GetGridByLocation(goalLocation);
            
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

                    var heuristic = Heuristic(next.location, goalLocation);
                    next.priority = heuristic;
                    
                    // Debug.Log($"Grid {next.location.x},{next.location.y} heuristic : {heuristic}");
                    
                    _frontierQueue.Insert(next);

                    _cameFrom.Add(next, current);
                    
                    next.SetMaterial(neighborMaterial);
                    next.ChangeMaterial();

                    yield return wait;
                }
                
                // _frontierQueue.DebugListIndex();

                current.SetMaterial(reachedMaterial);
                current.ChangeMaterial();
                
                yield return wait;
            }
        }

        void HasReached(Grid.Grid grid)
        {
            _reached.Add(grid);

            // Set material
            grid.SetMaterial(frontierMaterial);
            grid.ChangeMaterial();
        }

        void ClearBFS()
        {
            foreach (var grid in _cameFrom)
            {
                grid.Value.SetMaterial(groundMaterial);
                grid.Value.ChangeMaterial();
            }

            while (_frontierQueue.size > 0)
            {
                var grid = _frontierQueue.DeleteMin();
                grid.SetMaterial(groundMaterial);
                grid.ChangeMaterial();
            }

            _frontierQueue.Clear();
            _reached.Clear();
            _cameFrom.Clear();
        }

        void ClearPath()
        {
            foreach (var grid in _path)
            {
                grid.SetMaterial(groundMaterial);
                grid.ChangeMaterial();
            }
            _path.Clear();
        }

        IEnumerator BFSPathfinding(Vector2 startLoc, Vector2 goalLoc)
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            var start = GridManager.instance.GetGridByLocation(startLoc);
            var current = GridManager.instance.GetGridByLocation(goalLoc);
            
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
                grid.SetMaterial(reachedMaterial);
                grid.ChangeMaterial();
                
                yield return wait;
            }
            
            Debug.Log("Done. Langkah: " + _path.Count);
            hasPathfindingTimelapse = false;
        }

        private void DebugListGrid(List<Grid.Grid> grids)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var grid in grids)
            {
                sb.Append(grid.index + " ");
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
