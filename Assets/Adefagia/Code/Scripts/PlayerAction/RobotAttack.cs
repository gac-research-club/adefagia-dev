using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotAttack : MonoBehaviour
    {
        public void Attack(RobotController robotController,GridController gridController)
        {
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

            // if grid is not robot then miss
            if (grid.Status != GridStatus.Robot)
            {
                Debug.Log("Attack Miss");
                BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                                $"- Attack Miss");
                return;
            }
            
            // robotController.Robot.DecreaseStamina();

            // a robot at other grid attacked by the current robot
            gridController.RobotController.Robot.TakeDamage(robotController.Robot.Damage);
            
            // update the attacked robot healthbar
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

            Debug.Log($"Attack to {gridController.RobotController.Robot}");
            BattleManager.battleLog.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} " +
                                            $"- Attack to {gridController.RobotController.Robot}");
        }
    }
}