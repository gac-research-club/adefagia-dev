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
        public List<Grid> Reached { get; }
        public List<Robot.Robot> Robots { get; }
        
        // CameFrom
        private Dictionary<Grid, Grid> _cameFrom;

        // if Blocked by occupied grid
        private Grid _firstOccupied;

        public AStar()
        {
            _frontierQueue = new PriorityQueueMin();
            Reached = new List<Grid>();
            _cameFrom = new Dictionary<Grid, Grid>();
            Robots = new List<Robot.Robot>();
        }

        public bool Pathfinding(Grid start, Grid end)
        {
            // For better choosing path
            var costSoFar = new Dictionary<Grid, float>();
            
            // Starting point
            _frontierQueue.Insert(start);
            costSoFar[start] = 0;

            while (_frontierQueue.size > 0)
            {
                // Enqueue from Queue
                var current = _frontierQueue.DeleteMin();
                
                // If already reaching end. break loop
                // Pathfinding success
                if (current.Equals(end)) return true;

                // Looking Neighbor
                foreach (var neighbor in current.Neighbors)
                {
                    // Set neighbor came from
                    // Neighbor is must not null
                    if (neighbor.IsUnityNull()) continue;
                    
                    // Neighbor have not reached, grid ground and not occupied
                    if (GridManager.CheckGround(neighbor) && !Reached.Contains(neighbor) && !neighbor.IsOccupied)
                    {

                        var newCost = costSoFar[current] + 1;
                        costSoFar[neighbor] = newCost;
                        
                        // If distance is more closer update _cameFrom
                        if (newCost > costSoFar[neighbor]) continue;
                        
                        neighbor.Priority = newCost + Heuristic(end.Location, neighbor.Location);
            
                        _cameFrom[neighbor] = current;
                        _frontierQueue.Insert(neighbor);
                    }
                }
                
                // grid Has reached
                Reached.Add(current);
            }

            // Pathfinding failed
            // DebugListGrid(Reached);
            return false;
        }

        public List<Grid> Traversal(Grid start, Grid end)
        {
            var path = new List<Grid>();
            
            var current = end;
            
            // add Path from end to start
            while (!current.Equals(start))
            {
                path.Add(current);
                current = _cameFrom[current];
            }
            path.Add(start);
            
            // Reverse path to make it start to end
            path.Reverse();

            return path;
        }

        public bool BFS(Grid start, List<Vector2> dirs)
        {
            // Starting point
            _frontierQueue.Insert(start);
            
            while (_frontierQueue.size > 0)
            {
                // Enqueue from Queue
                var current = _frontierQueue.DeleteMin();

                // Looking Neighbor
                foreach (var neighbor in current.Neighbors)
                {
                    // Set neighbor came from
                    // Neighbor is must not null
                    if (neighbor.IsUnityNull()) continue;
                    
                    if(!dirs.Contains(neighbor.Location)) continue;
                    
                    // Occupied by the others Robot
                    if (neighbor.IsOccupied && !Robots.Contains(neighbor.Robot))
                    {
                        // Debug.Log($"Grid ({neighbor.Location.x},{neighbor.Location.y})");
                        Robots.Add(neighbor.Robot);
                        continue;
                    }

                    // Neighbor have not reached, grid ground and not occupied
                    if (GridManager.CheckGround(neighbor) && !Reached.Contains(neighbor))
                    {

                        // If distance is more closer update _cameFrom
                        neighbor.Priority = Heuristic(start.Location, neighbor.Location);
            
                        _frontierQueue.Insert(neighbor);
                    }
                }
                
                // grid Has reached
                current.IsHighlight = true;
                Reached.Add(current);
            }

            // Pathfinding failed
            // DebugListGrid(reached);
            return true;
        }

        public enum BFSLineType
        {
            Right,
            Up,
            Left,
            Down
        }
        public bool BFSLine(BFSLineType type, Grid start)
        {
            // Starting point
            _frontierQueue.Insert(start);
            
            while (_frontierQueue.size > 0)
            {
                // Enqueue from Queue
                var current = _frontierQueue.DeleteMin();
                
                // grid Has reached
                current.IsHighlight = true;
                Reached.Add(current);
                
                // Looking Neighbor
                Grid neighbor = null;
                switch (type)
                {
                    case BFSLineType.Right:
                        neighbor = current.Right;
                        break;
                    case BFSLineType.Up:
                        neighbor = current.Up;
                        break;
                    case BFSLineType.Left:
                        neighbor = current.Left;
                        break;
                    case BFSLineType.Down:
                        neighbor = current.Down;
                        break;
                }

                // Set neighbor came from
                // Neighbor is must not null
                if (neighbor.IsUnityNull())
                {
                    return false;
                }
                
                if (GridManager.CheckGround(neighbor) && !Reached.Contains(neighbor) && !neighbor.IsOccupied)
                {
                    _frontierQueue.Insert(neighbor);
                }
                
            }

            // Pathfinding failed
            // DebugListGrid(reached);
            return true;
        }

        public void DebugListGrid(List<Grid> grids)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var grid in grids)
            {
                sb.Append($"({grid.Location.x},{grid.Location.y})" + " ");
            }
            sb.Append("]");
                
            Debug.Log(sb);
        }
        
        public void DebugListRobot(List<Robot.Robot> robots)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var robot in robots)
            {
                sb.Append($"Robot {robot.Id}, ");
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