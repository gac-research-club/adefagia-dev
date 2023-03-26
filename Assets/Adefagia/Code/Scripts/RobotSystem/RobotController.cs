using System;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.RobotSystem
{
    public class RobotController : MonoBehaviour
    {
        private Vector3 _startPosition;
        public Robot Robot { get; set; }

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void MovePosition(Grid grid)
        {
            var position = new Vector3(grid.X, 0, grid.Y);
            transform.position = position;
            
            // TODO: move to position with some transition
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
        }
    }
}