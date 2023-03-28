using System;
using TMPro;
using UnityEngine;

namespace Adefagia
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void Awake()
        {
            if (canvas.enabled)
            {
                HideBattleUI();
            }
        }

        public void ShowBattleUI()
        {
            canvas.enabled = true;
        }

        public void HideBattleUI()
        {
            canvas.enabled = false;
        }
    }
}
