using System.Collections.Generic;
using System.Text;
using adefagia.Collections;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.PercobaanPathfinding
{
    public class AStar
    {
        // Frontier
        private readonly PriorityQueueMin _frontierQueue;
        
        // Reached
        public readonly List<Grid> reached;
        
        // CameFrom
        private Dictionary<Grid, Grid> _cameFrom;

        // if Blocked by occupied grid
        private Grid _firstOccupied;

        public AStar()
        {
            _frontierQueue = new PriorityQueueMin();
            reached = new List<Grid>();
            _cameFrom = new Dictionary<Grid, Grid>();
        }

        public bool Pathfinding(Grid start, Grid end)
        {
            var costSoFar = new Dictionary<Grid, float>();
            
            // Starting point
            _frontierQueue.Insert(start);
            costSoFar[start] = 0;

            while (_frontierQueue.size > 0)
            {
                var current = _frontierQueue.DeleteMin();
                
                if (current.Equals(end)) return true;

                // Looking Neighbor
                foreach (var neighbor in current.neighbors)
                {
                    // Set neighbor came from
                    if (GridManager.IsGridEmpty(neighbor)) continue;
                    // if (neighbor.IsOccupied())
                    // {
                    //     if (_firstOccupied.IsUnityNull())
                    //     {
                    //         _firstOccupied = current;
                    //     }
                    //     continue;
                    // }
                    
                    if (reached.Contains(neighbor)) continue;

                    var newCost = costSoFar[current] + 1;
                    
                    if (costSoFar.ContainsKey(neighbor)) continue;
                    costSoFar[neighbor] = newCost;

                    if (newCost > costSoFar[neighbor]) continue;
                    // neighbor.priority = newCost + Heuristic(end.location, neighbor.location);
                    
                    _cameFrom[neighbor] = current;
                    _frontierQueue.Insert(neighbor);
                }
                
                // Has reached
                reached.Add(current);
            }

            // Debug.Log(_firstOccupied.index);
            return false;
        }

        public List<Grid> Traversal(Grid start, Grid end)
        {
            var path = new List<Grid>();
            
            var current = end;
            
            while (!current.Equals(start))
            {
                path.Add(current);
                current = _cameFrom[current];
            }
            path.Add(start);
            path.Reverse();

            return path;
        }

        public Grid GetFirstOccupied()
        {
            return _firstOccupied;
        }
        
        public void DebugListGrid(List<Grid> grids)
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
        }
    }
}