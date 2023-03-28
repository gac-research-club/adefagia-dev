using System.Collections.Generic;

using UnityEngine;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.RobotSystem;

namespace Adefagia.Collections
{
    public class BFS
    {
        public enum BFSLineType
        {
            Right,
            Up,
            Left,
            Down
        }
        
        // Frontier
        private readonly PriorityQueueMin _frontierQueue;
        
        // Reached
        public List<Grid> Reached { get; }
        public List<Robot> Robots { get; }
        
        // CameFrom
        private Dictionary<Grid, Grid> _cameFrom;

        // if Blocked by occupied grid
        private Grid _firstOccupied;
        
        public BFS()
        {
            _frontierQueue = new PriorityQueueMin();
            Reached = new List<Grid>();
            _cameFrom = new Dictionary<Grid, Grid>();
            Robots = new List<Robot>();
        }
        
        // public bool BFSArea(Grid start, List<Vector2> dirs)
        // {
        //     // Starting point
        //     _frontierQueue.Insert(start);
        //     
        //     while (_frontierQueue.size > 0)
        //     {
        //         // Enqueue from Queue
        //         var current = _frontierQueue.DeleteMin();
        //
        //         // Looking Neighbor
        //         foreach (var neighbor in current.Neighbors)
        //         {
        //             // Set neighbor came from
        //             // Neighbor is must not null
        //             if (neighbor == null) continue;
        //             
        //             if(!dirs.Contains(neighbor.Location)) continue;
        //             
        //             // Occupied by the others Robot
        //             if (neighbor.IsOccupied && !Robots.Contains(neighbor.Robot))
        //             {
        //                 // Debug.Log($"Grid ({neighbor.Location.x},{neighbor.Location.y})");
        //                 Robots.Add(neighbor.Robot);
        //                 continue;
        //             }
        //
        //             // Neighbor have not reached, grid ground and not occupied
        //             if (GridManager.CheckGround(neighbor) && !Reached.Contains(neighbor))
        //             {
        //
        //                 // If distance is more closer update _cameFrom
        //                 neighbor.Priority = Heuristic(start.Location, neighbor.Location);
        //     
        //                 _frontierQueue.Insert(neighbor);
        //             }
        //         }
        //         
        //         // grid Has reached
        //         current.IsHighlight = true;
        //         Reached.Add(current);
        //     }
        //
        //     // Pathfinding failed
        //     // DebugListGrid(reached);
        //     return true;
        // }
        
        // public bool BFSLine(BFSLineType type, Grid start)
        // {
        //     // Starting point
        //     _frontierQueue.Insert(start);
        //     
        //     while (_frontierQueue.size > 0)
        //     {
        //         // Enqueue from Queue
        //         var current = _frontierQueue.DeleteMin();
        //         
        //         // grid Has reached
        //         current.IsHighlight = true;
        //         Reached.Add(current);
        //         
        //         // Looking Neighbor
        //         Grid neighbor = null;
        //         switch (type)
        //         {
        //             case BFSLineType.Right:
        //                 neighbor = current.Right;
        //                 break;
        //             case BFSLineType.Up:
        //                 neighbor = current.Up;
        //                 break;
        //             case BFSLineType.Left:
        //                 neighbor = current.Left;
        //                 break;
        //             case BFSLineType.Down:
        //                 neighbor = current.Down;
        //                 break;
        //         }
        //
        //         // Set neighbor came from
        //         // Neighbor is must not null
        //         if (neighbor == null)
        //         {
        //             return false;
        //         }
        //         
        //         if (GridManager.CheckGround(neighbor) && !Reached.Contains(neighbor) && !neighbor.IsOccupied)
        //         {
        //             _frontierQueue.Insert(neighbor);
        //         }
        //         
        //     }
        //
        //     // Pathfinding failed
        //     // DebugListGrid(reached);
        //     return true;
        // }
        
        private float Heuristic(Vector2 a, Vector2 b)
        {
            // Manhattan distance on a square grid
            return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);
        }
    }
}