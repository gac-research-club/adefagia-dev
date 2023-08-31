    using System;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
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
        
        [SerializeField] private GameObject listSkill;
        [SerializeField] private GameObject listItem;

        [SerializeField] private GameObject gridInfo;

        public Text timer;

        [SerializeField] private List<GameObject> robotSelectPanels;
        [SerializeField] private List<GameObject> robotNotSelectPanels;
        

        public List<Slider> healthBarSliders;

        private void Start()
        {
            // BattleManager.RobotNotHaveSkill += HideSkillButton;
            ShowSelectUI(null);
        }

        private void OnEnable()
        {
            BattleManager.RobotNotHaveSkill += HideItemButton;
            GridManager.GridHoverInfo += OnGridInfo;
            BattleManager.SelectRobotUI += ShowSelectUI;
        }

        private void OnDisable()
        {
            BattleManager.RobotNotHaveSkill -= HideItemButton;
            GridManager.GridHoverInfo -= OnGridInfo;
            // RobotSkill.LaunchSkillController += HideSkillButton;
            BattleManager.SelectRobotUI -= ShowSelectUI;
        }

        private void ShowSelectUI(RobotController robotController)
        {
            if (robotController == null)
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
            }
            
            foreach (var robotSelectPanel in robotSelectPanels)
            {
                ShowUI(robotSelectPanel);
            }
            
            foreach (var robotNotSelectPanel in robotNotSelectPanels)  
            {
                HideUI(robotNotSelectPanel);
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

        // public void HideSkillButton(RobotController robotController)
        // {
        //     if (robotController == null) return;
        //     
        //     // Check if robot has weapon
        //     if (robotController.SkillController == null)
        //     {
        //         // Debug.Log("Hide button");
        //         HideUI(listSkill);
        //     }
        //     else
        //     {
        //         Robot robot = robotController.Robot;
        //         // 3 skill
        //         for (int i = 0; i < 3 ; i++)
        //         {
        //             Button buttonSkill = listSkill.transform.GetChild(i).GetComponent<Button>();
        //             if(buttonSkill == null) return;
        //             
        //             Text buttonText = buttonSkill.GetComponentInChildren<Text>();
        //
        //             Skill _skill = robotController.SkillController.ChooseSkill(i);
        //             
        //             // TODO : Change button text;
        //             buttonText.text = _skill.Name;
        //
        //             if(robot.CurrentStamina < _skill.StaminaRequirement){
        //                 buttonSkill.interactable = false;
        //             }else{
        //                 buttonSkill.interactable = true;
        //             }
        //         }
        //         ShowUI(listSkill);
        //     }
        // }

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
                    Button buttonSkill = listItem.transform.GetChild(i).GetComponent<Button>();
                    Text buttonText = buttonSkill.GetComponentInChildren<Text>();

                    Potion _potion = robotController.PotionController.ChoosePotion(i);
                    if(_potion == null) continue;
                    
                    // TODO : Change button text;
                    buttonText.text = _potion.Name;

                    if(_potion.HasUsed){
                        buttonSkill.interactable = false;
                    }else{
                        buttonSkill.interactable = true;
                    }
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
        }
    }
}