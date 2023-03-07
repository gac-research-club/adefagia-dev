using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public TMP_InputField inputFieldLocation;

        public int robotId;
        
        void Start()
        {
            _gameManager = GameManager.instance;
        }

        public void ButtonMoveClicked()
        {
            // Debug.Log(inputFieldLocation.text);
            
            var s = inputFieldLocation.text.Split(",");
            var endLocation = new Vector2();
            endLocation.x = float.Parse(s[0]);
            endLocation.y = float.Parse(s[1]);

            // var robot = _gameManager.spawnManager.GetRobot();
            // var robot = _gameManager.robotManager.spawner.GetRobotById(robotId);
            // if (robot.IsUnityNull()) return;
            
            // robot.Move(endLocation);
        }
    }
}
