using System;
using System.Collections;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using Random = UnityEngine.Random;

namespace Adefagia.BattleMechanism
{
    public class BattleManager : MonoBehaviour
    {
        // Before Battle start
        public static GameState gameState = GameState.Initialize;

        public static PreparationState preparationState = PreparationState.Nothing;
        public static BattleState battleState = BattleState.Nothing;
            
        [SerializeField] private TeamController teamA, teamB;
        
        public TeamController TeamActive { get; set; }
        public TeamController NextTeam { get; set; }

        private void Awake()
        {
            /* Team A deploying Area
             *  #  ........ 9,9
             * 0,6 ........  #
             *----------------------*/
            teamA.SetPreparationArea(0,6,9,9);
            
            
            /* Team B deploying Area
             *  #  ........ 9,3
             * 0,0 ........  #
             *----------------------*/
            teamB.SetPreparationArea(0,0,9,3);
            
            StartCoroutine(PreparationBattle());
        }

        private void Update()
        {
            
            #region Preparation

            if (preparationState == PreparationState.DeployRobot)
            {
                
                // Change Team Activate if has deployed all the robot
                if (TeamActive.IsHasFinishDeploy())
                {
                    ChangeTeam();
                    
                    // If 2 team has finishing deploy
                    if (TeamActive.IsHasFinishDeploy())
                    {
                        ChangePreparationState(PreparationState.Nothing);
                        ChangeBattleState(BattleState.SelectRobot);
                        ChangeGameState(GameState.Battle);
                    }
                }
                
                // Move robot location & position
                var gridHover = GameManager.instance.gridManager.GetGrid();
                
                if(gridHover == null) return;

                // gridHover only in team active Area
                if (!TeamActive.IsInPreparationArea(gridHover)) return;
                
                // if grid has been occupied by other robot
                if(Grid.IsOccupied(gridHover)) return;
                
                // if have been deployed, cannot change location and position
                if (TeamActive.IsHasDeployed(TeamActive.Robot)) return;
                
                TeamActive.Robot.ChangeLocation(gridHover);
                TeamActive.RobotController.MovePosition(gridHover);

                // Change gridController
                var gridController = GameManager.instance.gridManager.GetGridController();
                if (gridController == null) return;
                TeamActive.GridController = gridController;
            }

            #endregion

            #region Battle

            if (battleState == BattleState.SelectRobot)
            {
                var gridController = GameManager.instance.gridManager.GetGridController();
                
                if(gridController == null) return;
                
                // select robot by grid
                if (TeamActive.Contains(gridController.RobotController))
                {
                    TeamActive.ChooseRobot(gridController.RobotController);
                }
                else
                {
                    TeamActive.ChooseRobot(null);
                }
                
            }

            #endregion
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

            ChangePreparationState(PreparationState.DeployRobot);
        }

        private void ChangeTeam()
        {
            // Swap via destruction
            (TeamActive, NextTeam) = (NextTeam, TeamActive);
        }

        #region ChangeState
        
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
        public static void ChangeBattleState(BattleState state)
        {
            if (gameState == GameState.Preparation)
            {
                battleState = state;
            }
        }
        
        #endregion

        #region UnityEvent
        public void OnMouseClick()
        {
            if (preparationState == PreparationState.DeployRobot)
            {
                if (TeamActive.IsHasDeployed(TeamActive.Robot)) return;
                TeamActive.DeployRobot();
                
                // Occupied the grid
                TeamActive.Robot.Location.SetOccupied();
                TeamActive.RobotController.GridController = TeamActive.GridController;
                TeamActive.GridController.RobotController = TeamActive.RobotController;
            }

            if (battleState == BattleState.SelectRobot)
            {
                TeamActive.RobotControllerSelected = TeamActive.RobotController;
                
                // Show or Hide Battle UI
                if (TeamActive.RobotControllerSelected != null)
                {
                    GameManager.instance.uiManager.ShowBattleUI();
                }
                else
                {
                    GameManager.instance.uiManager.HideBattleUI();
                }
            }
        }

        public void MoveClick()
        {
            Debug.Log($"Move {TeamActive.RobotControllerSelected.Robot}");
        }
        
        #endregion
    }
    

    public enum GameState
    {
        Initialize,
        Preparation,
        Battle
    }

    public enum PreparationState
    {
        Nothing,
        SelectTeam,
        DeployRobot,
    }

    public enum BattleState
    {
        Nothing,
        SelectRobot,
    }
}

