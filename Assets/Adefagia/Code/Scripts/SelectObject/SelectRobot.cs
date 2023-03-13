using UnityEngine;

using Adefagia.RobotSystem;

namespace Adefagia.SelectObject
{
    public class SelectRobot : MonoBehaviour
    {
        public SelectableObject<Robot> RobotSelect { get; private set; }

        public bool active;
        public Robot robotSelected;
        
        private void Start()
        {
            RobotSelect = new SelectableObject<Robot>(GameManager.instance.robotManager);
        }
        
        public Robot GetRobotSelect()
        {
            return RobotSelect.GetSelect();
        }
    }
}