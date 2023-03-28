using System;
using UnityEngine;

namespace Adefagia.BattleMechanism
{
    public class GUIBattleManager : MonoBehaviour
    {
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

            if (BattleManager.TeamActive != null)
            {
                var text2 = $"Team: \n{BattleManager.TeamActive.Team.teamName}";
                GUI.Box (new Rect (Screen.width - 100,100,100,50), text2);
            }
        }
    }
}