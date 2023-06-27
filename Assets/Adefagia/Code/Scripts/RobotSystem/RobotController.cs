using System;
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
    [RequireComponent(typeof(RobotSkill))]
    [RequireComponent(typeof(RobotItem))]

    public class RobotController : MonoBehaviour
    {
        [SerializeField] private float healthPoint;
        [SerializeField] private float staminaPoint;

        private Vector3 _startPosition;
        private TeamController _teamController;
        
        private SkillController _skillController;

        private PotionController _potionController;
        private RobotMovement _robotMovement;
        private RobotAttack _robotAttack;
        private RobotSkill _robotSkill;
        private RobotItem _robotItem;
        public Robot Robot { get; set; }
        
        public TeamController TeamController => _teamController;
        public SkillController SkillController => _skillController;
        public PotionController PotionController => _potionController;

        public RobotMovement RobotMovement => _robotMovement;
        public RobotAttack RobotAttack => _robotAttack;
        public RobotSkill RobotSkill => _robotSkill;
        public RobotItem RobotItem => _robotItem;
        public GridController GridController { get; set; }

        public static event Action<Vector3> TakeDamageHappened; 

        private void Awake()
        {
            _robotMovement = GetComponent<RobotMovement>();
            _robotAttack   = GetComponent<RobotAttack>();
            _robotSkill    = GetComponent<RobotSkill>();
            _robotItem     = GetComponent<RobotItem>();
            
            _startPosition = transform.position;
        }

        private void Start()
        {
            RobotAttack.ThingHappened += OnThingHappened;

            if (Robot != null)
            {
                Robot.Damaged += OnDamaged;
                Robot.Dead += OnDead;
            }
        }

        private void Update()
        {
            healthPoint = Robot.CurrentHealth;
            staminaPoint = Robot.CurrentStamina;
        }

        public void OnThingHappened(RobotController robotController)
        {
            if (robotController.GetInstanceID() != GetInstanceID()) return;
            Debug.Log($"InstanceID: {GetInstanceID()}");
            Debug.Log($"{robotController.Robot.Name} Attack");
        }

        public void OnDamaged()
        {
            Debug.Log($"InstanceID: {GetInstanceID()}");
            Debug.Log($"{Robot.Name} Damaged");
            
            TakeDamageHappened?.Invoke(transform.position);
        }

        public void OnDead()
        {
            Debug.Log($"InstanceID: {GetInstanceID()}");
            Debug.Log($"{Robot.Name} Dead");
            _teamController.RemoveRobot(this);
            GridController.Grid.SetFree();
            Destroy(gameObject);
        }

        public void SetTeam(TeamController teamController)
        {
            _teamController = teamController;
        }

        public void SetSkill(SkillController skillController)
        {
            _skillController = skillController;
        }

        public void SetPotion(PotionController potionController){
            _potionController = potionController;
        }

        public void MovePosition(Grid grid)
        {
            var position = new Vector3(grid.X, 0, grid.Y);
            transform.position = position;
            
            // Y angle is 0 & 180
            // Look at center grid (4,4)
            var center = new Vector3(grid.X,0,4);
            transform.LookAt(center);

            var fixAngle = Math.Clamp(transform.eulerAngles.y, 0, 180);
            transform.eulerAngles = new Vector3(0, fixAngle, 0);


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
                
                // Look At Grid
                transform.LookAt(GridManager.CellToWorld(grids[current]));
                
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