using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using Adefagia.SelectObject;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Grid = UnityEngine.Grid;

namespace Adefagia.BattleMechanism
{
    public class PromptHandler : MonoBehaviour
    {
        public TMP_InputField inputField;
        public static TeamController TeamActive { get; set; }
        private Select _select;
        public Button btn_item1;
        public Button btn_item2;

        // This method will be called when the user finishes editing the InputField
        public void OnEndEdit(string editedText)
        {
            editedText.ToLower();
            char[] separators = new char[] { ' ', ',' };

            string[] words = editedText.ToLower().Split(separators);
            Debug.Log(int.Parse(words[1]));
            int x = int.Parse(words[1]);
            int y = int.Parse(words[2]);

            if (words[0] == "move")
            {
                var obj = GameObject.Find($"Grid ({x}, {y})");
                BattleManager.TeamActive.RobotControllerSelected.RobotMovement.Move(
                    robotController: BattleManager.TeamActive.RobotControllerSelected,
                    gridController: obj.GetComponent<GridController>(),
                    speed: BattleManager.TeamActive.RobotControllerSelected.Robot.Speed
                );
            }
            else if (words[0] == "attack")
            {
                var obj = GameObject.Find($"Grid ({x}, {y})");
                BattleManager.TeamActive.RobotControllerSelected.RobotAttack.Attack(
                    robotController: BattleManager.TeamActive.RobotControllerSelected,
                    gridController: obj.GetComponent<GridController>()
                );
            }
            else if (words[0] == "skill")
            {
                var obj = GameObject.Find($"Grid ({x}, {y})");
                BattleManager.TeamActive.RobotControllerSelected.RobotSkill.Skill(
                    robotController: BattleManager.TeamActive.RobotControllerSelected,
                    gridController: obj.GetComponent<GridController>()
                );
            }
            else if (words[0] == "use")
            {
                if (words[1] == "1")
                {
                    btn_item1.onClick.Invoke();
                }else if (words[1] == "2")
                {
                    btn_item2.onClick.Invoke();
                }
            }

            inputField.text = "";
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                inputField.ActivateInputField();
            }
        }
    }
}
