using System;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.ObstacleSystem
{
    public class ObstacleController : MonoBehaviour
    {
        public ObstacleElement ObstacleElement;
        public Obstacle Obstacle { get; set; }
        public Grid Grid { get; set; }

        private int _hitCount = 0;

        public static event Action<Vector3> ObstacleDestroyed;
        public static event Action<Vector3> ObstacleHit;

        private void Start()
        {
            RobotAttack.ObstacleHitHappened += OnObstacleHitHappened;
            RobotSkill.ObstacleHitHappened += OnObstacleHitHappened;
            RobotAttack.RobotBotAttackObstacle += OnRobotBotAttackObstacle;
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

        private void OnObstacleHitHappened(GridController gridController)
        {
            // Only run on affected obstacle
            if (gridController.Grid != Grid) return;

            ObstacleHit?.Invoke(transform.position);
            GameManager.instance.logManager.LogStep($"An Obstacle has been hit!", LogManager.LogText.Warning);
                
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
        
    }
}