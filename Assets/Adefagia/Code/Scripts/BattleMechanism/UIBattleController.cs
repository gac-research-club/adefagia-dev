﻿using System;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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

        [SerializeField] private GameObject gridInfo;
        [SerializeField] private GameObject robotStat;
        [SerializeField] private GameObject currentRobotStat;

        public Text timer;

        [SerializeField] private List<GameObject> robotSelectPanels;
        [SerializeField] private List<GameObject> robotNotSelectPanels;
        

        public List<Slider> healthBarSliders;
        public Slider healthSlider;
        public Slider staminaSlider;

        private void Start()
        {
            BattleManager.RobotNotHaveSkill += HideSkillButton;
            BattleManager.RobotNotHaveSkill += HideItemButton;
            GridManager.GridHoverInfo += OnGridInfo;
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
                    ShowUI(cancelButton);
                    ShowUI(listSkill);
                    ShowUI(listItem);
                    
                    // if (BattleManager.battleState == BattleState.SkillRobot ||
                    //     BattleManager.battleState == BattleState.SkillSelectionRobot){
                    // }
                    // if (BattleManager.battleState == BattleState.ItemRobot || BattleManager.battleState == BattleState.ItemSelectionRobot){
                    //     ShowUI(listItem);
                    // }
                }
                else
                {
                    HideUI(cancelButton);
                    // HideButton(listSkill);
                    // HideUI(listItem);
                }


                RobotController robotSelected = BattleManager.TeamActive.RobotControllerSelected;
                
                // if Robot haven't selected than return
                if (robotSelected == null)
                {
                    foreach (var robotSelectPanel in robotSelectPanels)
                    {
                        HideUI(robotSelectPanel);
                    }
                    
                    foreach (var robotNotSelectPanel in robotNotSelectPanels)
                    {
                        ShowUI(robotNotSelectPanel);
                    }
                    return;
                }else{
                    currentRobotStat.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = $"{robotSelected.Robot.Damage}";
                    currentRobotStat.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = $"{robotSelected.Robot.Defend}";
                }
                
                foreach (var robotSelectPanel in robotSelectPanels)
                {
                    ShowUI(robotSelectPanel);
                }
                    
                foreach (var robotNotSelectPanel in robotNotSelectPanels)
                {
                    HideUI(robotNotSelectPanel);
                }

                // robotNameText.text = robotSelected.Robot.ToString();
                
                if(robotSelected.Robot.CurrentStamina <= 0){
                    // DisableButton(buttonAttack);
                    // DisableButton(buttonMove);
                    //DisableButton(buttonSkill);
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

                    // if (robotSelected.Robot.HasSkill)
                    // {
                    //     DisableButton(buttonSkill);
                    // }
                    // else
                    // {
                    //     EnableButton(buttonSkill);
                    // }
                    
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

        private void ShowUI(Button button)
        {
            button.gameObject.SetActive(true);
        }
        
        private void HideUI(Button button)
        {
            button.gameObject.SetActive(false);
        }

        public void HideSkillButton(RobotController robotController)
        {
            if (robotController == null) return;
            
            // Check if robot has weapon
            if (robotController.SkillController == null)
            {
                // Debug.Log("Hide button");
                HideUI(listSkill);
            }
            else
            {
                // 2 skill
                for (int i = 0; i < 2 ; i++)
                {
                    var buttonSkill = listSkill.transform.GetChild(i).GetComponent<Button>();
                    if(buttonSkill == null) return;
                    
                    var buttonText = buttonSkill.GetComponentInChildren<Text>();

                    Skill _skill = robotController.SkillController.ChooseSkill(i);
                    
                    // TODO : Change button text;
                    buttonText.text = _skill.Name;
                }
                ShowUI(listSkill);
            }
        }

        private void ShowUI(GameObject buttonList)
        {
            buttonList.SetActive(true);
        }

        public void HideItemButton(RobotController robotController)
        {
            if (robotController == null) return;
            
            // Check if robot has weapon
            if (robotController.PotionController == null)
            {
                Debug.Log("Hide potion");
                HideUI(listItem);
            }
            else
            {
                // 2 skill
                for (int i = 0; i < 2 ; i++)
                {
                    var buttonSkill = listItem.transform.GetChild(i).GetComponent<Button>();
                    var buttonText = buttonSkill.GetComponentInChildren<Text>();

                    Potion _potion = robotController.PotionController.ChoosePotion(i);
                    if(_potion == null) continue;
                    
                    // TODO : Change button text;
                    buttonText.text = _potion.Name;
                }
                ShowUI(listItem);
            }
        }
        
        private void HideUI(GameObject buttonList)
        {
            buttonList.SetActive(false);
        }

        private void OnGridInfo(GridController gridController)
        {
            if(gridController == null) return;
            
            // Title
            gridInfo.transform.GetChild(1).GetComponent<Text>().text = gridController.Grid.Status.ToString();
            
            // Description
            gridInfo.transform.GetChild(2).GetComponent<Text>().text = "Ini adalah grid";
            
            if(gridController.RobotController == null){
                
                robotStat.SetActive(false); 
                return;

            }else{
                
                robotStat.SetActive(true);

                robotStat.transform.GetChild(0).GetComponent<Text>().text = gridController.RobotController.Robot.Name;
                robotStat.transform.GetChild(2).GetComponent<Text>().text = "Ini adalah robot";
                
                robotStat.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = $"{gridController.RobotController.Robot.CurrentHealth}/{gridController.RobotController.Robot.MaxHealth}";
                robotStat.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = $"{gridController.RobotController.Robot.CurrentStamina}/{gridController.RobotController.Robot.MaxStamina}";
                robotStat.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = $"{gridController.RobotController.Robot.Damage}";
                robotStat.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = $"{gridController.RobotController.Robot.Defend}";
            }
        }
    }
}