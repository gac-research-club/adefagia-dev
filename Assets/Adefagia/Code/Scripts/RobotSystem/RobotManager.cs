using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using UnityEngine;


namespace Adefagia.RobotSystem
{
    public class RobotManager : MonoBehaviour
    {
        public GameObject robotPrefab;

        private TeamController _teamController;

        private void Awake()
        {
            _teamController = GetComponent<TeamController>();

            StartCoroutine(SpawnRobot());
            
        }

        private IEnumerator SpawnRobot()
        {

            while (BattleManager.gameState != GameState.Initialize)
            {
                yield return null;
            }

            List<RobotController> newRobotControllers = new List<RobotController>();
            for (int i = _teamController.team.robotControllers.Count-1; i >= 0 ; i--)
            {
                var dummy = _teamController.team.robotControllers[i].gameObject;
                
                var robotObject = Instantiate(robotPrefab, transform);
                robotObject.name = "Robot " + i;
                robotObject.transform.position = dummy.transform.position;

                newRobotControllers.Add(robotObject.AddComponent<RobotController>());
                
                Destroy(dummy);
            }

            _teamController.team.robotControllers = newRobotControllers;

            // Create gameObject of robot
            // gridObject.transform.position = new Vector3(xi * gridLength, 0, yi * gridLength);
            // gridObject.name = $"Grid ({xi}, {yi})";
            //         
            // // Add Grid Controller
            // var gridController = gridObject.AddComponent<GridController>();
            // gridController.Grid = new Grid(xi, yi);
            //
            // // Add into List grid Object
            // _listGrid[xi, yi] = gridController.Grid;
        }

    }
}
