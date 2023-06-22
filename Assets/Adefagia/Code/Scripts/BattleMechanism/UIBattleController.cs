using System;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
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
        [SerializeField] private Button buttonSkill;
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private GameObject listSkill;
        [SerializeField] private GameObject listItem;
        
        private void Start()
        {
            BattleManager.RobotNotHaveSkill += HideSkillButton;
        }

        
        // TODO: disable button Move
        private void Update()
        {
            if (BattleManager.gameState == GameState.Battle)
            {

                if (BattleManager.battleState == BattleState.MoveRobot ||
                    BattleManager.battleState == BattleState.AttackRobot || 
                    BattleManager.battleState == BattleState.SkillRobot ||
                    BattleManager.battleState == BattleState.SkillSelectionRobot ||
                    BattleManager.battleState == BattleState.ItemRobot ||
                    BattleManager.battleState == BattleState.ItemSelectionRobot)
                {
                    ShowButton(cancelButton);
                    if (BattleManager.battleState == BattleState.SkillRobot ||
                        BattleManager.battleState == BattleState.SkillSelectionRobot){
                        ShowButton(listSkill);
                    }
                    if (BattleManager.battleState == BattleState.ItemRobot || BattleManager.battleState == BattleState.ItemSelectionRobot){
                        ShowButton(listItem);
                    }
                }
                else
                {
                    HideButton(cancelButton);
                    HideButton(listSkill);
                    HideButton(listItem);
                }


                var robotSelected = BattleManager.TeamActive.RobotControllerSelected;
                
                // if Robot haven't selected than return
                if (robotSelected == null) return;

                robotNameText.text = robotSelected.Robot.ToString();
                
                if(robotSelected.Robot.CurrentStamina <= 0){
                    // DisableButton(buttonAttack);
                    // DisableButton(buttonMove);
                    DisableButton(buttonSkill);
                }else{
                    
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

                    if (robotSelected.Robot.HasSkill)
                    {
                        DisableButton(buttonSkill);
                    }
                    else
                    {
                        EnableButton(buttonSkill);
                    }
                    
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

        public void HideSkillButton(RobotController robotController)
        {
            if (robotController == null) return;
            
            // Check if robot has weapon
            if (robotController.SkillController == null)
            {
                Debug.Log("Hide button");
                HideButton(buttonSkill);
            }
            else
            {
                ShowButton(buttonSkill);
            }
        }

        private void ShowButton(GameObject buttonList)
        {
            buttonList.SetActive(true);
            var robotSelected = BattleManager.TeamActive.RobotControllerSelected;

                
            // if Robot haven't selected than return
            if (robotSelected == null) return;

            if (BattleManager.battleState == BattleState.SkillRobot || BattleManager.battleState == BattleState.SkillSelectionRobot)
            {          
                for (int i = 0; i < 3 ; i++)
                {
                    Button buttonSkill = buttonList.transform.GetChild(i).GetComponent<Button>();
                    TextMeshProUGUI buttonText = buttonSkill.GetComponentInChildren<TextMeshProUGUI>();

                    Skill _skill = robotSelected.SkillController.ChooseSkill(i);
                    
                    // TODO : Change button text;
                    buttonText.text = _skill.Name;
                }
            }else if (BattleManager.battleState == BattleState.ItemRobot || BattleManager.battleState == BattleState.ItemSelectionRobot)
            {
                for (int i = 0; i < 2 ; i++)
                {
                    Button buttonItem = buttonList.transform.GetChild(i).GetComponent<Button>();
                    TextMeshProUGUI buttonText = buttonItem.GetComponentInChildren<TextMeshProUGUI>();

                    Potion _potion = robotSelected.PotionController.ChoosePotion(i);

                    // TODO : Change button text;
                    buttonText.text = _potion.Name;
                }
            }
        }
        
        private void HideButton(GameObject buttonList)
        {
            buttonList.SetActive(false);
        }

        
    }
}