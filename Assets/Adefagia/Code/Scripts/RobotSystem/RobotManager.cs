using System.Collections.Generic;

using UnityEngine;

using Adefagia.SelectObject;

namespace Adefagia.RobotSystem
{
    public class RobotManager : MonoBehaviour
    {
        // public bool doneSpawnRobot;
        public List<RobotTeam> robotTeams;

        public SelectableObject<Robot> Select { get; private set; }

        private void Awake()
        {
            CreateSpawnerGameObject();
            
            Select = new SelectableObject<Robot>(this);
        }

        void CreateSpawnerGameObject()
        {
            foreach (var team in robotTeams)
            {
                var spawner = new Spawner(this, team);
            }
        }

        public Robot GetRobotSelect()
        {
            return Select.GetSelect();
        }

        public void DeleteRobotSelect()
        {
            Select.GetSelect().Selected(false);
            Select.GetSelect().Hover(false);
            Select.GetSelect().ClearGridRange();
            Select.DeleteSelect();
        }

        public bool IsRobotSelect()
        {
            return GetRobotSelect() != null;
        }

        #region UnityEvent
        public void Hover(GameObject robotGameObject)
        {
            Select.ChangeHover(robotGameObject.GetComponent<RobotStatus>().Robot);
        }
        
        public void NotHover()
        {
            Select.NotHover();
        }
        
        public void RobotClick(GameObject robotGameObject)
        {
            // Debug.Log("click");
            var robot = robotGameObject.GetComponent<RobotStatus>().Robot;
            Select.ChangeSelect(robot);
        }
        #endregion
        
    }
}
