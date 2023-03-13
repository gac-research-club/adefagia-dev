using System.Collections.Generic;
using UnityEngine;

using Adefagia.RobotSystem;

namespace Adefagia.SelectObject
{
    public class SelectRobotManager : MonoBehaviour
    {

        public int index;
        public List<GameObject> selectRobots;

        private void Start()
        {
            selectRobots = new List<GameObject>();
            
            CreateRobotSelect(2);
        }

        public void CreateRobotSelect(int size = 1)
        {
            if (size < 1) return;
            for (int i = 0; i < size; i++)
            {
                var selectRobot = new GameObject
                {
                    transform = { parent = transform },
                    name = "SelectObject",
                };
                selectRobot.AddComponent<SelectRobot>();
                selectRobots.Add(selectRobot);
            }
        }
        
        #region UnityEvent
        public void Hover(GameObject robotGameObject)
        {
            Robot robot = robotGameObject.GetComponent<RobotStatus>()?.Robot;
            if (robot == null) return;
            selectRobots[index].GetComponent<SelectRobot>().RobotSelect.ChangeHover(robot);
        }
        
        public void NotHover()
        {
            selectRobots[index].GetComponent<SelectRobot>().RobotSelect.NotHover();
        }
        
        public void RobotClick(GameObject robotGameObject)
        {
            // Debug.Log("click");
            var robot = robotGameObject.GetComponent<RobotStatus>().Robot;
            selectRobots[index].GetComponent<SelectRobot>().RobotSelect.ChangeSelect(robot);
        }
        #endregion
    }
}