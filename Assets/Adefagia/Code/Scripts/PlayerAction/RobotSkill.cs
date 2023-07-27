using System;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.PlayerAction
{
    public class RobotSkill : MonoBehaviour
    {
        public static event Action<GridController> ObstacleHitHappened;
        
        // robotController = current select
        // gridController = another select robot
        public void Skill(
            RobotController robotController, 
            GridController gridController, 
            int skillChoosed,
            Dictionary<Grid, GridController> gridImpacts)
            {
                
                
                if (gridController == null)
                {
                    Debug.LogWarning("Skill failed");
                    return;
                }
                
                var skillController = robotController.SkillController;
                var skill = skillController.ChooseSkill(skillChoosed);
            
                // means the robot is considered to move
                robotController.Robot.HasSkill = true;
                
                var grid = gridController.Grid;
                
                // Debug.Log("Obstacle Hit: " + gridCtrl.Grid);
                ObstacleHitHappened?.Invoke(gridController);

                // Take impact
                foreach (var gridCtrl in gridImpacts.Values)
                {
                    if (gridCtrl == null) return;
                    
                    if (gridCtrl.Grid.Status == GridStatus.Obstacle)
                    {
                        // Debug.Log("Obstacle Hit: " + gridCtrl.Grid);
                        ObstacleHitHappened?.Invoke(gridCtrl);
                    }
                    else if (gridCtrl.Grid.Status == GridStatus.Robot)
                    {
                        gridCtrl.RobotController.Robot.TakeDamage(skill.Value * 0.5f);
                        gridCtrl.RobotController.Robot.healthBar.UpdateHealthBar(gridCtrl.RobotController.Robot.CurrentHealth);
                    }
                }

                // if grid is not robot then miss
                if (grid.Status != GridStatus.Robot)
                {
                    GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Skill {skill.Name} Launched");
                    Debug.Log("Skill Miss");
                    return;
                }
                
                robotController.Robot.DecreaseStamina(skill.StaminaRequirement);

                // a robot at other grid attacked by the current robot
                gridController.RobotController.Robot.TakeDamage(skill.Value);
                gridController.RobotController.Robot.healthBar.UpdateHealthBar(gridController.RobotController.Robot.CurrentHealth);
                
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

            public static Skill GetSkill(RobotController robotController, int index)
            {
                var skillController = robotController.SkillController;
                return skillController.ChooseSkill(index);
            }
    }
    
}