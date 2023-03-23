using System;
using UnityEngine;

namespace Adefagia.BattleMechanism
{
    public class GUIBattleManager : MonoBehaviour
    {
        private BattleManager _battleManager;
        private void Start()
        {
            _battleManager = GetComponent<BattleManager>();
        }

        private void OnGUI()
        {
            var text = BattleManager.gameState.ToString();
            GUI.Box (new Rect (Screen.width - 100,0,100,50), text);

            var textPrepare = "";
            if (BattleManager.preparationState == PreparationState.Nothing)
            {
                textPrepare = BattleManager.battleState.ToString();
            } 
            else if (BattleManager.battleState == BattleState.Nothing)
            {
                textPrepare = BattleManager.preparationState.ToString();
            }
            
            GUI.Box (new Rect (Screen.width - 100,50,100,50), textPrepare);
            
            var text2 = $"Team: \n{_battleManager.TeamActive.Team.teamName}";
            GUI.Box (new Rect (Screen.width - 100,100,100,50), text2);
        }
    }
}