using System;
using Adefagia.BattleMechanism;
using Adefagia.PlayerAction;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public static event UnityAction<int, bool> TurnAnimation;
        public static event UnityAction<int, bool> MoveAnimation;
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
            RobotMovement.RobotBotMove += OnRobotBotMove;
            RobotAttack.RobotBotAttack += OnRobotBotAttack;

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

        // Set Skill Controller
        public void SetSkill(SkillController skillController)
        {
            _skillController = skillController;
        }

        // Get Skill Controller
        public Skill GetSkill(int indexSkill){
            return SkillController.ChooseSkill(indexSkill);
        }

        public void SetPotion(PotionController potionController){
            _potionController = potionController;
        }

        public void MovePosition(Grid grid)
        {
            var position = new Vector3(grid.X * GridManager.GridLength, 0, grid.Y * GridManager.GridLength);
            transform.position = position;
            
            // Y angle is 0 & 180
            // Look at center grid (4,4)
            var center = new Vector3(grid.X * GridManager.GridLength,0,4 * GridManager.GridLength);
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
            var current = 1;
            var turn = true;
            var interpolationFramesCount = 120; // Number of frames to completely interpolate between the 2 positions
            var elapsedFrames = 0;

            var body = transform.GetChild(2).gameObject;
            var id = body.GetInstanceID();

            while (Vector3.Distance(transform.position, GridManager.CellToWorld(grids[^1])) > 0.01f)
            {

                if (turn)
                {
                    var dir = TurnArround(GridManager.CellToWorld(grids[current]));
                    var start = transform.forward;
                    
                    Debug.Log("Direction :" + dir);
                    Debug.Log("Start:" + start);

                    if (dir != Vector3.zero && dir != start)
                    {
                        yield return new WaitForSeconds(0.2f);
                        
                        MoveAnimation?.Invoke(id, false);
                        TurnAnimation?.Invoke(id, true);

                        while (elapsedFrames < interpolationFramesCount)
                        {
                            var interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
                            transform.forward = Vector3.Slerp(start, dir, interpolationRatio);
                            yield return null;
                            elapsedFrames += 1;
                        }
                        
                        TurnAnimation?.Invoke(id, false);
                        transform.forward = dir;
                        
                        yield return new WaitForSeconds(0.2f);
                    }

                    turn = false;
                    elapsedFrames = 0;
                }
                
                MoveAnimation?.Invoke(id, true);

                var step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, GridManager.CellToWorld(grids[current]), step);

                if (Vector3.Distance(transform.position, GridManager.CellToWorld(grids[current])) < 0.01f)
                {
                    // Look At Grid
                    body.transform.DOLocalMove(Vector3.zero, 0.2f); 
                    current++;
                    turn = true;
                }

                yield return null;
            }

            transform.position = GridManager.CellToWorld(grids[^1]);
            
            MoveAnimation?.Invoke(id, false);
        }

        private Vector3 TurnArround(Vector3 targetPosition)
        {
            var root = transform.position;  
            var result = (targetPosition - root).normalized;
            return result;
        }

        private void OnRobotBotMove(List<RobotController> robotControllers, Team team)
        {
            // Find enemy robot
            if(this == null) return;
            
            if(_teamController.Team == team) return;
            
            robotControllers.Add(this);

        }

        private void OnRobotBotAttack(List<RobotController> robotControllers, Team team)
        {
            // Find enemy robot
            if(this == null) return;
            
            if(_teamController.Team == team) return;
            
            robotControllers.Add(this);
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