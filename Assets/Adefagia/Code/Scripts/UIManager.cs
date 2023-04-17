using System.Collections;
using Adefagia.BattleMechanism;
using UnityEngine;
using UnityEngine.UI;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas battleCanvas;
        [SerializeField] private Button buttonEndTurn;

        [SerializeField] private Canvas finishCanvas;
        
        
        private void Awake()
        {
            HideCanvasUI(finishCanvas);
            HideBattleUI();
            
            buttonEndTurn.gameObject.SetActive(false);
            StartCoroutine(ShowButtonEndTurn());
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
            finishCanvas.GetComponent<UIFinishController>().ChangeName(teamName);
            ShowCanvasUI(finishCanvas);
        }

        /*-------------------------------------------------------------
         * Enable canvas
         *-------------------------------------------------------------*/
        private void ShowCanvasUI(Canvas canvas)
        {
            canvas.enabled = true;
        }

        /*-------------------------------------------------------------
         * Disable canvas
         *-------------------------------------------------------------*/
        private void HideCanvasUI(Canvas canvas)
        {
            canvas.enabled = false;
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
    }
}
