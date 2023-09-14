using System;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.ObstacleSystem
{
    public class ObstacleController : MonoBehaviour
    {
        public ObstacleElement ObstacleElement;
        public Obstacle Obstacle { get; set; }
        public Grid Grid { get; set; }

        public bool isHighlighted;

        private int _hitCount = 0;

        public HighlightRobot HighlightRobot { get; set; }

        public static event Action<Vector3> ObstacleDestroyed;
        public static event Action<Vector3> ObstacleHit;

        private void Start()
        {
            HighlightRobot = GetComponent<HighlightRobot>();

            RobotAttack.ObstacleHitHappened += OnObstacleHitHappened;
            RobotSkill.ObstacleHitHappened += OnObstacleHitHappened;
            RobotAttack.RobotBotAttackObstacle += OnRobotBotAttackObstacle;
        }

        private void OnEnable()
        {
            HighlightMovement.AreaObstacleHighlight += OnHighlighted;
            HighlightMovement.AreaCleanHighlight += OnCleanHighlighted;
            RobotSkill.SkillImpactEvent += OnSkillImpactEvent;
        }

        private void Update()
        {
            if (_hitCount >= ObstacleElement.MaxHitCount)
            {
                // Affected if has been hit
                ObstacleDestroyed?.Invoke(transform.position);
                Grid.SetFree();
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            HighlightMovement.AreaObstacleHighlight -= OnHighlighted;
            HighlightMovement.AreaCleanHighlight -= OnCleanHighlighted;
            RobotSkill.SkillImpactEvent -= OnSkillImpactEvent;
        }

        private void OnObstacleHitHappened(GridController gridController)
        {
            // Only run on affected obstacle
            if (gridController.Grid != Grid) return;

            ObstacleHit?.Invoke(transform.position);
            GameManager.instance.logManager.LogStep($"An Obstacle has been hit on Grid (${Grid.X}, ${Grid.Y})! ", LogManager.LogText.Warning);

            IncrementHit();
        }

        private void IncrementHit()
        {
            // Only for obstacle destructible
            if (ObstacleElement.ObstacleType == ObstacleType.Destructible)
            {
                // increase hit count
                _hitCount += 1;
            }
        }

        private void OnRobotBotAttackObstacle(List<ObstacleController> obstacleControllers)
        {

        }

        private void OnHighlighted(ObstacleController obstacleController)
        {
            obstacleController.HighlightRobot.ChangeColor();
            obstacleController.isHighlighted = true;
        }

        private void OnCleanHighlighted()
        {
            if (isHighlighted)
            {
                HighlightRobot.ResetColor();
                isHighlighted = false;
            }
        }

        private void OnSkillImpactEvent(RobotController enemy, Grid grid)
        {
            if (grid != Grid) return;

            // TODO: Count hit
            IncrementHit();
        }

    }
}