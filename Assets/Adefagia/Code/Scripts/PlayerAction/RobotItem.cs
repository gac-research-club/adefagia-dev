using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotItem : MonoBehaviour
    {
        // robotController = current select
        // gridController = another select robot
        public void Item(RobotController robotController, Potion potion)
        {   
            
            // potion.HasUsed = true;

            foreach (UsableItemEffect effect in potion.Effects)
            {   
                effect.ExecuteEffect(robotController);
            }

            GameManager.instance.logManager.LogStep($"{robotController.TeamController.Team.teamName} - {robotController.Robot.Name} - Use {potion.Name}");
                
            // Debug.Log($"Skill {skill.Name} launched to {gridController.RobotController.Robot}");
        }
    }
}