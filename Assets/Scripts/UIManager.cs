using TMPro;
using UnityEngine;

namespace adefagia
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public TMP_InputField inputFieldLocation;
        
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
            
            _gameManager.robotManager.spawner.GetRobot().Move(endLocation);
        }
    }
}
