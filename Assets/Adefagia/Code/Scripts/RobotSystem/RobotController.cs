using Adefagia.BattleMechanism;
using Adefagia.PlayerAction;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using System.Collections;

namespace Adefagia.RobotSystem
{
    [RequireComponent(typeof(RobotMovement))]
    [RequireComponent(typeof(RobotAttack))]
    public class RobotController : MonoBehaviour
    {
        [SerializeField] private float healthPoint;
        private float elapsedTime;
        private float desiredDuration = 0.49f;
        
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
        public IEnumerator MoveRobotPosition(Grid grid)
        {
            var position = new Vector3(grid.X, 0, grid.Y);

            elapsedTime = 0;
            while(elapsedTime < desiredDuration)
            {
                transform.position = Vector3.Lerp(transform.position, position, elapsedTime / desiredDuration);
                elapsedTime += Time.deltaTime;
                Debug.Log(elapsedTime);
                yield return null;
            }
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