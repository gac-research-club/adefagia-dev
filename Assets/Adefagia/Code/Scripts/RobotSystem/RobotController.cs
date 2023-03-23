using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.RobotSystem
{
    public class RobotController : MonoBehaviour
    {
        public Robot Robot { get; set; }
        
        public GridController GridController { get; set; }

        public void MovePosition(Grid grid)
        {
            var position = new Vector3(grid.X, 0, grid.Y);
            transform.position = position;
        }
    }
}