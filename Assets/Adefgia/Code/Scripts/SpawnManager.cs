using System;
using System.Collections;
using System.Collections.Generic;
using adefagia.Robot;
using UnityEngine;
using Random = UnityEngine.Random;

namespace adefagia
{
    public class SpawnManager : MonoBehaviour
    {
        public Spawner spawnerA;
        public Spawner spawnerB;

        public Team teamTurn;

        private Robot.Robot _robot;

        private void Start()
        {
            // Random 50:50
            teamTurn = (Random.value < 0.5f) ? Team.TeamA : Team.TeamB;
        }

        public Robot.Robot GetRobot()
        {
            return _robot;
        }
        public void SelectRobot(Robot.Robot robot)
        {
            _robot = robot;
        }
    }
    
    public enum Team
    {
        TeamA,
        TeamB
    }
}
