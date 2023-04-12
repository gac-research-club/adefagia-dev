﻿using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotSkill : MonoBehaviour
    {
        public void Skill(RobotController robotController, GridController gridController, int skillChoosed)
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

            // if grid is not robot then miss
            if (grid.Status != GridStatus.Robot)
            {
                Debug.Log("Attack Miss");
                return;
            }
            
            robotController.Robot.DecreaseStamina(skill.StaminaReq);

            // a robot at other grid attacked by the current robot
            gridController.RobotController.Robot.TakeDamage(skill.Damage);
            
            // if grid is robot ally then friendly fire
            var teamController = gridController.RobotController.TeamController;
            
            if (teamController == BattleManager.TeamActive)
            {
                Debug.Log($"Friendly fire to {gridController.RobotController.Robot}");
                return;
            }

            Debug.Log($"Skill {skill.Name} launched to {gridController.RobotController.Robot}");
        }
    }
}