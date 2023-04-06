using Adefagia.BattleMechanism;
using Adefagia.PlayerAction;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Adefagia.RobotSystem
{
    [RequireComponent(typeof(RobotMovement))]
    [RequireComponent(typeof(RobotAttack))]
    public class RobotController : MonoBehaviour
    {
        [SerializeField] private float healthPoint;
        [SerializeField] private float staminaPoint;

        private Vector3 _startPosition;
        private TeamController _teamController;
        
        private RobotMovement _robotMovement;
        private RobotAttack _robotAttack;
        
        public Robot Robot { get; set; }
        public TeamController TeamController => _teamController;
        public RobotMovement RobotMovement => _robotMovement;
        public RobotAttack RobotAttack => _robotAttack;
        public GridController GridController { get; set; }

        private void Awake()
        {
            _robotMovement = GetComponent<RobotMovement>();
            _robotAttack   = GetComponent<RobotAttack>();
            
            _startPosition = transform.position;
        }

        private void Update()
        {
            healthPoint = Robot.CurrentHealth;
            staminaPoint = Robot.CurrentStamina;

            if(Robot.IsDead){
                Destroy(gameObject);
            }
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
            // move with lerp
        }

        /*--------------------------------------------------------------------------------------
         * Move Robot smoothly for each grid
         *--------------------------------------------------------------------------------------*/
        public IEnumerator MoveRobotPosition(List<Grid> grids, float speed)
        {
            var current = 0;
            while (Vector3.Distance(transform.position, GridManager.CellToWorld(grids[^1])) > 0.01f)
            {
                var step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, GridManager.CellToWorld(grids[current]), step);
                
                if (Vector3.Distance(transform.position, GridManager.CellToWorld(grids[current])) < 0.01f)
                {
                    current++;
                }

                yield return null;
            }

            transform.position = GridManager.CellToWorld(grids[^1]);
        }

        public void MoveWithDoTween(List<Grid> grids)
        {
            // move to position with some transition
            // move with lerp

            var wayPoints = new List<Vector3>();
            foreach (var grid in grids)
            {
                wayPoints.Add(GridManager.CellToWorld(grid));
            }

            transform.DOPath(
                path: wayPoints.ToArray(), 
                duration: 0.2f * wayPoints.Count,
                pathType: PathType.Linear,
                pathMode: PathMode.TopDown2D
            );
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
