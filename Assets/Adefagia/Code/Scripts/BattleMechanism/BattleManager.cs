using System;
using System.Collections;
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
        
        public static TeamController TeamActive { get; set; }
        public static TeamController NextTeam { get; set; }

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
            
            // StartCoroutine(PreparationBattle());
            PreparationBattle();
        }

        private void Update()
        {
            
            #region Preparation

            if (gameState == GameState.Preparation && 
                preparationState == PreparationState.DeployRobot)
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
                
                /*---------------------------------------------------------------
                 * Move robot location & position
                 *---------------------------------------------------------------*/
                
                // if have been deployed, cannot change location and position
                if (TeamActive.IsHasDeployed(TeamActive.Robot)) return;
                
                TeamActive.GridController = GameManager.instance.gridManager.GetGridController();

                try
                {
                    var gridHover = TeamActive.GridController.Grid;

                    // gridHover only in team active Area
                    // & if grid has been occupied by other robot
                    if (!TeamActive.IsGridInPreparationArea(gridHover) || Grid.IsOccupied(gridHover))
                    {
                        throw new NullReferenceException();
                    }

                    // Set grid reference to robot
                    TeamActive.RobotController.MovePosition(gridHover);
                    TeamActive.Robot.ChangeLocation(gridHover);
                }
                catch (NullReferenceException)
                {
                    // Reset position
                    TeamActive.RobotController.ResetPosition();
                    
                    // make grid controller null
                    TeamActive.GridController = null;
                }
            }

            #endregion

            #region Battle

            if (gameState == GameState.Battle &&
                battleState == BattleState.SelectRobot)
            {
                // get grid controller
                var gridController = GameManager.instance.gridManager.GetGridController();

                // set robot from gridController
                try
                {
                    if (!TeamActive.Contains(gridController.RobotController))
                    {
                        throw new NullReferenceException();
                    }
                    
                    // select robot by grid
                    TeamActive.ChooseRobot(gridController.RobotController);
                }
                catch (NullReferenceException)
                {
                    TeamActive.ChooseRobot(null);
                }
            }

            #endregion
        }

        private void PreparationBattle()
        {
            // Wait until GameState is Preparation
            // while (gameState != GameState.Preparation)
            // {
            //     yield return null;
            // }

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
                /*-----------------------------------------------------------------
                 * Deploy Robot
                 *-----------------------------------------------------------------*/

                // Robot has deploy or grid is empty
                if (TeamActive.IsHasDeployed(TeamActive.Robot) 
                    || TeamActive.GridController == null) return;

                TeamActive.DeployRobot();
                
                // set robot to grid
                TeamActive.GridController.RobotController = TeamActive.RobotController;
                
                // Occupied the grid
                TeamActive.Robot.Location.SetOccupied();
                
                // change to the next robot index
                TeamActive.IncrementIndex();
                TeamActive.ChooseRobot();
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

        
        /*----------------------------------------------------------------------
         * Move robot to clicked grid
         *----------------------------------------------------------------------*/
        public void MoveClick()
        {
            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Move");
            
            // TODO: State cannot change
            ChangeBattleState(BattleState.MoveRobot);
        }
        
        #endregion
        
        private void OnGUI()
        {
            var text = "";
            try
            {
                text = "Active: " + TeamActive.Robot;
            }
            catch (NullReferenceException)
            {
                text = "Empty";
            }
            
            GUI.Box (new Rect (0,Screen.height - 50,100,50), text);
        }
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
        MoveRobot,
    }
}

