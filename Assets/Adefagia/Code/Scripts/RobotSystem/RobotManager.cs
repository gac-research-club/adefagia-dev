using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using UnityEngine;


namespace Adefagia.RobotSystem
{
    public class RobotManager : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> robotPrefab;
        [SerializeField] private float speed = 5f;
        
        private TeamController _teamController;

        private void Awake()
        {
            _teamController = GetComponent<TeamController>();

            // Initiate Robots
            SpawnRobot();
            
            _teamController.ChooseRobot(0);
        }

        /*--------------------------------------------------------------------------------------
         * Create a real robot gameObject with selected prefab
         * and replace the dummy gameObject
         *--------------------------------------------------------------------------------------*/
        private void SpawnRobot()
        {

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
                robotController.Robot = new Robot(robotObject.name);
                robotController.Robot.ID = _teamController.TotalRobot-1 - i;
                robotController.Robot.Speed = speed;
                robotController.Robot.healthBar = healthBar;

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
        }

    }
}
