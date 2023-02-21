using System;
using System.Collections.Generic;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia.Robot
{
    public class RobotManager : MonoBehaviour
    {
        public bool doneSpawnRobot;
        public List<RobotTeam> robotTeams;

        private Robot _robotSelect, _robotHover;

        private void Awake()
        {
            CreateSpawnerGameObject();
        }

        void CreateSpawnerGameObject()
        {
            foreach (var team in robotTeams)
            {
                var spawner = new Spawner(this, team);
            }
        }
    }
}
