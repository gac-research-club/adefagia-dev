using System;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.Inventory;
using Adefagia.RobotSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.PlayerAction
{
    public class RobotSkill : MonoBehaviour
    {
        public static event Action<GridController> ObstacleHitHappened;
        public static event Action<Robot> LaunchSkill;  
        public static event Action<RobotController> LaunchSkillController;
        public static event Action<RobotController, Grid> SkillImpactEvent;

        // robotController = current select
        // gridController = another select robot
        public void Skill(
            RobotController robotController, 
            GridController gridController)
        {
            if (gridController == null)
            {
                Debug.LogWarning("Skill failed");
                return;
            }
            
            // Set skill from robot skillSelected
            Skill skill = robotController.Robot.SkillSelected;

            robotController.Robot.DecreaseStamina(skill.StaminaRequirement);
            
            // LaunchSkill?.Invoke(robotController.Robot);
            LaunchSkillController?.Invoke(robotController);

            // means the robot is considered to move
            robotController.Robot.HasSkill = true;
            
            var grid = gridController.Grid;
            
            // Debug.Log("Obstacle Hit: " + gridCtrl.Grid);
            ObstacleHitHappened?.Invoke(gridController);

            // Take impact
            var gridsImpact = HighlightMovement.GetGridImpact(skill, robotController, grid);
            SkillImpact(robotController, gridsImpact);

            // if grid is not robot then miss
            if (grid.Status != GridStatus.Robot)
            {
                GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Skill {skill.Name} Launched");
                Debug.Log("Skill Miss");
                return;
            }
            
            // a robot at other grid attacked by the current robot
            gridController.RobotController.Robot.TakeDamage(skill.Value);
            // gridController.RobotController.Robot.healthBar.UpdateHealthBar(gridController.RobotController.Robot.CurrentHealth); (Robot)
            
            // if grid is robot ally then friendly fire
            var teamController = gridController.RobotController.TeamController;
            
            if (teamController == BattleManager.TeamActive)
            {
                Debug.Log($"Friendly fire to {gridController.RobotController.Robot}");
                return;
            }

            GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Skill {skill.Name} launched to {gridController.RobotController.Robot}");
            GameManager.instance.logManager.DamageCalculation(robotController.TeamController.Team.teamName, skill.Value);
            
            Debug.Log($"Skill {skill.Name} launched to {gridController.RobotController.Robot}");
        }

        private void SkillImpact(RobotController playerRobot, List<Grid> gridsImpact)
        {
            foreach (var grid in gridsImpact)
            {
                SkillImpactEvent?.Invoke(playerRobot, grid);
            }
        }
    }
    
}