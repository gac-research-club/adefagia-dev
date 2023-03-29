using System;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.RobotSystem
{
    [RequireComponent(typeof(RobotMovement))]
    public class RobotController : MonoBehaviour
    {
        private Vector3 _startPosition;
        private TeamController _teamController;
        private RobotMovement _robotMovement;
        
        public Robot Robot { get; set; }
        public RobotMovement RobotMovement => _robotMovement;

        private void Awake()
        {
            _robotMovement = GetComponent<RobotMovement>();
            
            _startPosition = transform.position;
        }

        private void Update()
        {
            // If the team is teamActive
            // if (_teamController == BattleManager.TeamActive && this == _teamController.RobotControllerSelected)
            // {
            //     if (Input.GetMouseButtonDown(0))
            //     {
            //         Debug.Log(this);
            //     }
            // }
        }

        public void SetTeam(TeamController teamController)
        {
            _teamController = teamController;
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

        public override string ToString()
        {
            return $"Controller {Robot.Name}";
        }
    }
}