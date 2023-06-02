using System;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotAttack : MonoBehaviour
    {
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
                BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                                $"- Attack failed");
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
                BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                                $"- Attack Miss");
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
                BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                                $"- Friendly fire to {gridController.RobotController.Robot}");
                return;
            }

            // Debug.Log($"Attack to {gridController.RobotController.Robot}");
            BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                            $"- Attack to {gridController.RobotController.Robot}");
        }
    }
}