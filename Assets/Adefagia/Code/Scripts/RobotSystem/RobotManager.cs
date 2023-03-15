using System.Collections.Generic;

using UnityEngine;

using Adefagia.SelectObject;
using Adefagia.GridSystem;

namespace Adefagia.RobotSystem
{
    public class RobotManager : MonoBehaviour
    {
        public Vector3 offsetRobotPosition;
        // public bool doneSpawnRobot;
        public List<RobotTeam> robotTeams;
        public GridManager GridManager => GameManager.instance.gridManager;

        private void Awake()
        {
            CreateSpawnerGameObject();
        }

        /*---------------------------------------------------------------------
         * Create spawner each Team
         *---------------------------------------------------------------------*/
        void CreateSpawnerGameObject()
        {
            foreach (var team in robotTeams)
            {
                var spawner = new Spawner(this, team);
            }
        }

        // public void DeleteRobotSelect()
        // {
        //     RobotSelect.GetSelect().Selected(false);
        //     RobotSelect.GetSelect().Hover(false);
        //     RobotSelect.GetSelect().ClearGridRange();
        //     RobotSelect.DeleteSelect();
        // }

    }
}
