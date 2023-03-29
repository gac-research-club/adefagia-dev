using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas battleCanvas;

        private void Awake()
        {
            if (battleCanvas.enabled)
            {
                HideBattleUI();
            }
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
