using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.Inventory;
using UnityEngine;


namespace Adefagia.RobotSystem
{
    public class RobotManager : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> robotPrefab;
        [SerializeField] private float speed = 5f;
        
        private TeamController _teamController;

        private List<RobotStat> robotSelected;
        private static int count = 0;

        private TeamManager teamManager;
        private Team teamSelected;

        private void Start()
        {
            _teamController = GetComponent<TeamController>();

            // Initiate Robots
            teamManager = GameManager.instance.gameObject.GetComponent<TeamManager>();
            
            // Change team name
            teamSelected = new Team(teamManager.teamA.teamName);
            
            // Use robot A first
            robotSelected = teamManager.robotsA;

            SpawnRobot();
            
            _teamController.ChooseRobot(0);
            
        }

        /*--------------------------------------------------------------------------------------
         * Create a real robot gameObject with selected prefab
         * and replace the dummy gameObject
         *--------------------------------------------------------------------------------------*/
        private void SpawnRobot()
        {

            if (count > 0)
            {
                // after robotA, change to robotB
                robotSelected = teamManager.robotsB;
                
                // Change team name
                teamSelected = new Team(teamManager.teamB.teamName);
            }
            
            // Change team name
            _teamController.Team = teamSelected;

            List<RobotController> newRobotControllers = new List<RobotController>();
            
            for (int i = _teamController.TotalRobot-1; i >= 0 ; i--)
            {
                var dummy = _teamController.GetRobotGameObject(i);
                
                // If dummy is null then skip to next loop
                if(dummy == null) continue;
             
                // Create a real robot gameObject
                var robotObject = Instantiate(robotPrefab[i], transform);
                robotObject.name = "Robot " + i;
                robotObject.transform.position = dummy.transform.position;

                // Add RobotController to attach on robot gameObject
                var robotController = robotObject.AddComponent<RobotController>();
                
                // Add SkillController to attach on robot
                var skillController = robotObject.AddComponent<SkillController>();

                // Add Potion Controller to attach on robot
                var potionController = robotObject.AddComponent<PotionController>();
                
                // Find healthBar GameObject
                var healthBarObject = GameObject.Find($"Robot {i}/Canvas/HealthBar");
                healthBarObject.name = "HBar Robot " + i;

                // add healthBar GameObject to healthBars List
                BattleManager.healthBars.Add(healthBarObject);

                // GetComponent<HealthBar> to input healthBar stat
                var healthBar = healthBarObject.GetComponent<HealthBar>();

                // Set robot the parent of teamController
                robotController.SetTeam(_teamController);
                
                // TODO: Make each robot dynamic edited by user
                
                // Manual input robot stat
                
                // Get robot from teamManager
                var robot = robotSelected[i];

                if (robot == null) return;
                robotController.Robot = new Robot(
                    _teamController.Team.teamName + "-" + robotObject.name,
                    robot.maxHealth,
                    robot.maxStamina,
                    robot.damage);
                
                robotController.Robot.ID = _teamController.TotalRobot-1 - i;
                robotController.Robot.Speed = speed;
                
                // If robot hasn't used weapon set to default
                if (robot.weaponId != null)
                {
                    robotController.Robot.TypePattern = robot.weaponId.TypePattern;
                    
                    // Skill
                    Skill skill1 = robot.weaponId.WeaponSkill[0];
                    Skill skill2 = robot.weaponId.WeaponSkill[1];
                
                    // Ultimate Skill
                    Skill skill3 = robot.weaponId.WeaponSkill[2];
                    
                    // set skill
                    skillController.Skills.Add(skill1); 
                    skillController.Skills.Add(skill2); 
                    skillController.Skills.Add(skill3);
                    
                    robotController.SetSkill(skillController);
                }
                else
                {
                    Debug.Log("Not have weapon");
                    
                    robotController.Robot.TypePattern = TypePattern.Surround;
                }

                robotController.Robot.healthBar = healthBar;
                
                // Add Potion
                Potion item1 = new Potion(robot.buffItem1.ItemName, robot.buffItem1.Effects);
                Potion item2 = new Potion(robot.buffItem2.ItemName, robot.buffItem2.Effects);

                potionController.Potions.Add(item1);
                potionController.Potions.Add(item2);

                robotController.SetPotion(potionController);

                // Manual input HealthBar stat
                healthBar.health = robotController.Robot.MaxHealth;
                healthBar.maxHealth = robotController.Robot.MaxHealth;
                healthBar.damage = robotController.Robot.Damage;
                

                // Edit name
                // robotController.Robot.Name = robotObject.name;
                newRobotControllers.Add(robotController);
                
                // Delete the dummy gameObject
                Destroy(dummy);
            }

            _teamController.ChangeRobotController(newRobotControllers);

            count++;
        }

    }
}
