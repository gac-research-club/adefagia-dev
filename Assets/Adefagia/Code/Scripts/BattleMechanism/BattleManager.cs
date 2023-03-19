using System.Collections;
using UnityEngine;

namespace Adefagia.BattleMechanism
{
    public class BattleManager : MonoBehaviour
    {
        // Before Battle start
        public static GameState gameState = GameState.Initialize;

        public static PreparationState preparationState = PreparationState.Nothing;
            
        [SerializeField] private TeamController teamA, teamB;
        
        public TeamController TeamActive { get; set; }
        public TeamController NextTeam { get; set; }

        private void Awake()
        {
            StartCoroutine(PreparationBattle());
        }

        private IEnumerator PreparationBattle()
        {
            // Wait until GameState is Preparation
            while (gameState != GameState.Preparation)
            {
                yield return null;
            }

            ChangePreparationState(PreparationState.SelectTeam);
            // Selecting Team to start first
            if (Random.Range(0, 2) == 0)
            {
                TeamActive = teamA;
                NextTeam = teamB;
            }
            else
            {
                TeamActive = teamB;
                NextTeam = teamA;
            }
            
            ChangePreparationState(PreparationState.ChooseRobot);
        }

        public static void ChangeGameState(GameState state)
        {
            gameState = state;
        }
        
        public static void ChangePreparationState(PreparationState state)
        {
            if (gameState == GameState.Preparation)
            {
                preparationState = state;
            }
        }
    }
    

    public enum GameState
    {
        Initialize,
        Preparation,
    }

    public enum PreparationState
    {
        Nothing,
        SelectTeam,
        ChooseRobot,
        DeployRobot,
    }
}

