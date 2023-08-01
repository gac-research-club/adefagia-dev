using System;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.ObstacleSystem;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotAttack : MonoBehaviour
    {
        public static event Action<List<RobotController>, Team> RobotBotAttack; 
        public static event Action<List<ObstacleController>> RobotBotAttackObstacle; 
        public static event Action<RobotController> ThingHappened;
        public static event Action<GridController> ObstacleHitHappened;

        // robotController = current select
        // gridController = another select robot
        public void Attack(RobotController robotController,GridController gridController)
        {
            ThingHappened?.Invoke(robotController);
            ObstacleHitHappened?.Invoke(gridController);
            
            if (gridController == null)
            {
                Debug.LogWarning("Attack failed");
                GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Attack failed");
                return;
            }
            
            // means the robot is considered to move
            robotController.Robot.HasAttack = true;

            var grid = gridController.Grid;

            // Attack into Obstacle
            if (grid.Status == GridStatus.Obstacle)
            {
                
                Debug.Log("Attack Obstacle");
            }

            // if grid is not robot then miss
            if (grid.Status != GridStatus.Robot)
            {
                // Debug.Log("Attack Miss");
                GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name}  - Attack Miss");
                return;
            }
            
            // robotController.Robot.DecreaseStamina();

            // a robot at other grid attacked by the current robot
            gridController.RobotController.Robot.TakeDamage(robotController.Robot.Damage);
            
            // update the attacked robot health bar
            gridController.RobotController.Robot.healthBar.UpdateHealthBar(gridController.RobotController.Robot.CurrentHealth);
            
            // if grid is robot ally then friendly fire
            var teamController = gridController.RobotController.TeamController;

            if (teamController == BattleManager.TeamActive)
            {
                Debug.Log($"Friendly fire to {gridController.RobotController.Robot}");
                GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Friendly fire to {gridController.RobotController.Robot}");
                GameManager.instance.logManager.DamageCalculation(robotController.TeamController.Team.teamName, robotController.Robot.Damage);
                return;
            }

            // Debug.Log($"Attack to {gridController.RobotController.Robot}");
            GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Attack to {gridController.RobotController.Robot}");
            GameManager.instance.logManager.DamageCalculation(robotController.TeamController.Team.teamName, robotController.Robot.Damage);
        }

        public void AttackBot(RobotController robotController)
        {
            var test = new List<RobotController>();
            
            RobotBotAttack?.Invoke(test, robotController.TeamController.Team);

            var attack = false;
            foreach (var vaRobotController in test)
            {
                if (!attack)
                {
                    if (BattleManager.highlightMovement.CheckGridOnHighlight(vaRobotController.GridController))
                    {
                        Attack(robotController, vaRobotController.GridController);
                        attack = true;
                    }
                }
            }
        }
    }
}