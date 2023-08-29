using System;
using Adefagia.BattleMechanism;
using UnityEngine;

namespace Adefagia.UI
{
    public class GUIBattleManager : MonoBehaviour
    {
        private void OnGUIBackup()
        {
            var robotActive = "";
            try
            {
                robotActive = "Active: " + BattleManager.TeamActive.Robot;
            }
            catch (NullReferenceException)
            {
                robotActive = "Empty";
            }

            var observe = "";
            try
            {
                var robot = GameManager.instance.gridManager.GetGridController().RobotController.Robot;
                observe = $"Hover : {robot}\n" +
                        $"Health : {robot.CurrentHealth}\n" +
                        $"Stamina : {robot.CurrentStamina}";
            }
            catch (NullReferenceException)
            {
                observe = "Empty";
            }

            GUI.Box(new Rect(0, Screen.height - 100, 100, 50), robotActive);
            GUI.Box(new Rect(0, Screen.height - 50, 100, 50), observe);

            //-------------------------------------------------------------------------------------------------
            var text = BattleManager.gameState.ToString();
            GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), text);

            var textPrepare = "";
            if (BattleManager.preparationState == PreparationState.Nothing)
            {
                textPrepare = BattleManager.battleState.ToString();
            }
            else if (BattleManager.battleState == BattleState.Nothing)
            {
                textPrepare = BattleManager.preparationState.ToString();
            }

            GUI.Box(new Rect(Screen.width - 100, 50, 100, 50), textPrepare);

            if (BattleManager.TeamActive != null)
            {
                var text2 = $"Team: \n{BattleManager.TeamActive.Team.teamName}";
                GUI.Box(new Rect(Screen.width - 100, 100, 100, 50), text2);
            }

            var timerText = $"Timer :{BattleManager.currentTime.ToString("0")}";
            GUI.Box(new Rect(Screen.width - 100, 150, 100, 50), timerText);


        }
    }
}