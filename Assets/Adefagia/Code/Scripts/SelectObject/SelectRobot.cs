using UnityEngine;

using Adefagia.RobotSystem;

namespace Adefagia.SelectObject
{
    public class SelectRobot : MonoBehaviour
    {
        private SelectableObject<Robot> robotSelect { get; set; }

        private void Start()
        {
            robotSelect = new SelectableObject<Robot>(GameManager.instance.robotManager);
        }
        
        public Robot GetRobotSelect()
        {
            return robotSelect.GetSelect();
        }
        public void ChangeRobotSelect(Robot robot)
        {
            robotSelect.ChangeSelect(robot);
        }
        public void ChangeRobotHover(Robot robot)
        {
            robotSelect.ChangeHover(robot);
        }
        public void RobotNotHover()
        {
            robotSelect.NotHover();
        }
    }
}