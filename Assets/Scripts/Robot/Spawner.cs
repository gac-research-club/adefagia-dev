using System;
using System.Collections;
using System.Collections.Generic;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace adefagia.Robot
{
    public class Spawner
    {
        public GameObject SpawnerGameObject { get; }
        
        private RobotTeam RobotTeam { get; }
        public RobotManager RobotManager { get; }

        public int totalTurn;
        
        public int turnId;

        public bool active;

        [Space(5f)]
        
        private int _turn;

        // private List<int> _robotsId;
        private Dictionary<int, Robot> _robots;

        private GridManager _gridManager;

        public Spawner(RobotManager robotManager, RobotTeam team)
        {
            RobotTeam = team;
            RobotManager = robotManager;
            
            SpawnerGameObject = Instantiate();
            
            SpawnRobot();
        }
        
        private GameObject Instantiate()
        {
            var spawner = new GameObject
            {
                transform =
                {
                    parent = RobotManager.transform
                },
                name = "Spawner " + RobotTeam.name 
            };

            return spawner;
        }

        private void SpawnRobot()
        {
            _robots = new Dictionary<int, Robot>();
            
            foreach (var robotPrefab in RobotTeam.dataRobot)
            {
                try
                {
                    var robot = new Robot(this, robotPrefab);
                    _robots.Add(robotPrefab.id, robot);
                }
                catch (ArgumentException)
                {
                    Debug.LogWarning($"Robot ID:{robotPrefab.id} Already Available");
                }
                
            }
        }

        public Robot GetRobotById(int id)
        {
            return _robots[id];
        }
        
        // public Robot GetRobotByTurn()
        // {
        //     if (totalTurn < 1) return null;
        //     
        //     turnId = _robotsId[_turn];
        //     return GetRobotById(_robotsId[_turn]);
        // }

        // public void FinishTurn()
        // {
        //     GetRobotByTurn().Deactivate();
        //     _turn++;
        //     if (_turn > totalTurn-1)
        //     {
        //         _turn = 0;
        //     }
        //     GetRobotByTurn().Activate();
        // }
    }
}