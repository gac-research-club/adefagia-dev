using System;
using System.Collections;
using Adefagia.BattleMechanism;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas battleCanvas;
        [SerializeField] private Button buttonEndTurn;
        

        private void Awake()
        {
            if (battleCanvas.enabled)
            {
                HideBattleUI();
            }

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

        /*-------------------------------------------------------------
         * Enable canvas battleUI
         *-------------------------------------------------------------*/
        public void ShowBattleUI()
        {
            battleCanvas.enabled = true;
        }

        /*-------------------------------------------------------------
         * Disable canvas battleUI
         *-------------------------------------------------------------*/
        public void HideBattleUI()
        {
            battleCanvas.enabled = false;
        }
    }
}
