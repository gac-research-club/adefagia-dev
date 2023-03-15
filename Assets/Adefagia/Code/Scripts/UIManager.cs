using TMPro;
using UnityEngine;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        // private GameManager _gameManager;

        public TMP_InputField inputFieldLocation;
        [SerializeField] private GameObject actionButton;

        public int robotId;

        void Start()
        {
            // _gameManager = GameManager.instance;
        }

        public static void ShowCanvas()
        {
            GameManager.instance.uiManager.actionButton.SetActive(true);
        }

        public static void HideCanvas()
        {
            GameManager.instance.uiManager.actionButton.SetActive(false);
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
