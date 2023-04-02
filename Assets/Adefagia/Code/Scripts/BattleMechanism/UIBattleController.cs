using System;
using Adefagia.BattleMechanism;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Adefagia.UI
{
    public class UIBattleController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI robotNameText;
        
        
        [SerializeField] private Button buttonMove;
        [SerializeField] private Button buttonAttack;
        [SerializeField] private Button buttonDeffend;
        [SerializeField] private Button cancelButton;
        
        
        // TODO: disable button Move
        private void Update()
        {
            if (BattleManager.gameState == GameState.Battle)
            {

                if (BattleManager.battleState == BattleState.MoveRobot ||
                    BattleManager.battleState == BattleState.AttackRobot)
                {
                    ShowButton(cancelButton);
                }
                else
                {
                    HideButton(cancelButton);
                }

                var robotSelected = BattleManager.TeamActive.RobotControllerSelected;
                
                // if Robot haven't selected than return
                if (robotSelected == null) return;

                robotNameText.text = robotSelected.Robot.ToString();
                

                // Disable if robot has moved
                if (robotSelected.Robot.HasMove)
                {
                    DisableButton(buttonMove);
                }
                else
                {
                    EnableButton(buttonMove);
                }
                
                // Disable if robot has attacked
                if (robotSelected.Robot.HasAttack)
                {
                    DisableButton(buttonAttack);
                }
                else
                {
                    EnableButton(buttonAttack);
                }

                if(robotSelected.Robot.CurrentStamina <= 0){
                    DisableButton(buttonAttack);
                    DisableButton(buttonMove);
                    DisableButton(buttonDeffend);
                }

            }
        }

        /*-------------------------------------------------
         * Disable button
         *-------------------------------------------------*/
        public void DisableButton(Button button)
        {
            button.interactable = false;
        }

        /*-------------------------------------------------
         * Enable button
         *-------------------------------------------------*/
        public void EnableButton(Button button)
        {
            button.interactable = true;
        }

        private void ShowButton(Button button)
        {
            button.gameObject.SetActive(true);
        }
        
        private void HideButton(Button button)
        {
            button.gameObject.SetActive(false);
        }
        
    }
}