using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adefagia.BattleMechanism;
using Adefagia.UI;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas battleCanvas;
        public UIBattleController uiBattleController;
        
        [SerializeField] private Button buttonEndTurn;
        [SerializeField] private Canvas finishCanvas;
        [SerializeField] private GameObject deployRobotCanvas;

        private void Awake()
        {
            // Set into gameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.uiManager = this;
            }
            
            HideCanvasUI(finishCanvas);
            HideBattleUI();
            
            buttonEndTurn.gameObject.SetActive(false);
            StartCoroutine(ShowButtonEndTurn());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Show character select canvas
                BattleManager.ChangePreparationState(PreparationState.DeploySelect);
                deployRobotCanvas.SetActive(true);
            }
        }

        private IEnumerator ShowButtonEndTurn()
        {
            while (BattleManager.gameState != GameState.Battle)
            {
                yield return null;
            }

            buttonEndTurn.gameObject.SetActive(true);
        }

        
        public void ShowBattleUI()
        {
            ShowCanvasUI(battleCanvas);
        }
        public void HideBattleUI()
        {
            HideCanvasUI(battleCanvas);
        }

        public void ShowFinishUI(string teamName)
        {
            Dictionary<string, List<float>> statistic = GameManager.instance.logManager.GetDamageCalculation();
            List<Robot> listRobot = GameManager.instance.logManager.GetListRobot();
            
            finishCanvas.GetComponent<UIFinishController>().ChangeName(teamName);

            int index = 0;
            foreach (string key in statistic.Keys){
                float totalDamage = statistic[key].Sum();
                finishCanvas.GetComponent<UIFinishController>().ChangeTotalDamage(totalDamage, key, index);
                index += 1;
            }

            string txtRobotList = "";
            foreach (Robot robot in listRobot){
                txtRobotList += robot.Name + "\n";
            }

            finishCanvas.GetComponent<UIFinishController>().ChangeRobotDead(txtRobotList);

            ShowCanvasUI(finishCanvas);
        }

        /*-------------------------------------------------------------
         * Enable canvas
         *-------------------------------------------------------------*/
        private void ShowCanvasUI(Canvas canvas)
        {
            canvas.gameObject.SetActive(true);
        }

        /*-------------------------------------------------------------
         * Disable canvas
         *-------------------------------------------------------------*/
        private void HideCanvasUI(Canvas canvas)
        {
            canvas.gameObject.SetActive(false);
        }

        /*-------------------------------------------------------------
         * Enable healthbars
         *-------------------------------------------------------------*/
        public void EnableHealthBars(bool isDeployed)
        {
            if(isDeployed)
            {
                foreach (var healthBar in BattleManager.healthBars)
                {
                    healthBar.SetActive(true);
                }
            }
        }
        
        // Change Character Select team name text
        public void ChangeTextTeam(string teamName)
        {
            deployRobotCanvas.GetComponent<DeployIndexing>().textTeamActive.text = teamName;
        }
        
        // Hide character select canvas
        public void HideCharacterSelectCanvas()
        {
            deployRobotCanvas.SetActive(false);
            BattleManager.ChangePreparationState(PreparationState.DeployRobot);
        }

        public void ShowCharacterSelectCanvas()
        {
            deployRobotCanvas.SetActive(true);
            BattleManager.ChangePreparationState(PreparationState.DeploySelect);
        }

        public void DisableButtonSelect(int index)
        {
            var deployIndex = deployRobotCanvas.GetComponent<DeployIndexing>();
            var hudRobot = deployIndex.hudRobots[index];
            var button = deployIndex.GetButton(hudRobot);
            
            if (button == null) return;
            button.interactable = false;
        }
        
        public void ResetButtonSelect()
        {
            var deployIndex = deployRobotCanvas.GetComponent<DeployIndexing>();
            foreach (var hudRobot in deployIndex.hudRobots)
            {
                var button = deployIndex.GetButton(hudRobot);
                
                if (button == null) return;
                button.interactable = true;
            }
        }
    }
}
