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
        public static GameState gameState               = GameState.Initialize;
        public static PreparationState preparationState = PreparationState.Nothing;
        public static BattleState battleState           = BattleState.Nothing;
            
        [SerializeField] private TeamController teamA, teamB;
        
        public static TeamController TeamActive { get; set; }
        public static TeamController NextTeam   { get; set; }

        public static float currentTime = 10f;
        float startingTime = 10f;

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

            if (gameState == GameState.Preparation && 
                preparationState == PreparationState.DeployRobot)
            {
                
                // Change Team Activate if has deployed all the robot
                if (TeamActive.IsHasFinishDeploy())
                {
                    // Reset team active robot selected
                    TeamActive.ResetRobotSelected();
                    TeamActive.SetPreparationArea(0,0,9,9); // full area
                    
                    // Change team Active
                    ChangeTeam();
                    
                    // If 2 team has finishing deploy
                    if (TeamActive.IsHasFinishDeploy())
                    {
                        /* State:
                         * Preparation => Nothing
                         * ---------------------
                         * Game => Battle
                         * Battle => SelectRobot
                         */
                        ChangePreparationState(PreparationState.Nothing);
                        ChangeGameState(GameState.Battle);
                        ChangeBattleState(BattleState.SelectRobot);
                    }
                }
                
                /*---------------------------------------------------------------
                 * Move robot location & position
                 *---------------------------------------------------------------*/
                
                // if have been deployed, cannot change location and position
                if (TeamActive.IsHasDeployed(TeamActive.Robot)) return;
                
                // Get grid controller active
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

            if (gameState == GameState.Battle)
            {
                if (battleState == BattleState.SelectRobot)
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

                if (battleState == BattleState.MoveRobot)
                {
                    // Exit to select another Robot
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        ChangeBattleState(BattleState.SelectRobot);
                    }
                }
                
            }

            if(TeamActive.IsHasFinishDeploy())
            {
                currentTime -= 1 * Time.deltaTime;
                if(currentTime<0)
                {
                    currentTime = 0;
                }
                
                if(currentTime == 0)
                {
                    EndTurnButtonClick();
                    currentTime = startingTime;
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

            ChangeGameState(GameState.Preparation);
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
            if (gameState == GameState.Battle)
            {
                battleState = state;
            }
        }
        
        #endregion

        #region UnityEvent
        public void OnMouseClick()
        {
            if (gameState == GameState.Preparation &&
                preparationState == PreparationState.DeployRobot)
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

            if (gameState == GameState.Battle)
            {
                // Select Robot
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
                
                // Move Robot
                if (battleState == BattleState.MoveRobot)
                {
                    // Get current grid click
                    var gridController = GameManager.instance.gridManager.GetGridController();

                    // run AStar Pathfinding
                    TeamActive.RobotControllerSelected.RobotMovement.Move(
                        robotController: TeamActive.RobotControllerSelected, 
                        gridController : gridController,
                        delayMove      : TeamActive.RobotControllerSelected.Robot.DelayMove
                        );

                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);
                }
                
                // Attack Robot
                if (battleState == BattleState.AttackRobot)
                {
                    // Get current grid click
                    var gridController = GameManager.instance.gridManager.GetGridController();
                    
                    // TODO: robot attack
                    TeamActive.RobotControllerSelected.RobotAttack.Attack(
                        robotController: TeamActive.RobotControllerSelected,
                        gridController: gridController
                        );
                    
                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);
                }
            }
        }

        
        /*----------------------------------------------------------------------
         * Click move button in UI step
         *----------------------------------------------------------------------*/
        public void MoveButtonClick()
        {
            // change to move robot
            ChangeBattleState(BattleState.MoveRobot);
            
            // Run Function Move from RobotMovement.cs

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Move");
        }
        
        /*----------------------------------------------------------------------
         * Click attack button in UI step
         *----------------------------------------------------------------------*/
        public void AttackButtonClick()
        {
            // change to move robot
            ChangeBattleState(BattleState.AttackRobot);
            
            // Run Function Attack from RobotAttack.cs
            
            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Attack");
        }
        
        public void DefendButtonClick()
        {
            // change to defend robot
            ChangeBattleState(BattleState.DefendRobot);
            
            // means the robot is considered to move
            TeamActive.RobotControllerSelected.Robot.HasDefend = true;
            
            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Defend");
        }

        public void EndTurnButtonClick()
        {
            if(currentTime != 0)
            {
                currentTime = startingTime;
            }
            TeamActive.ResetRobotSelected();
            
            
            ChangeTeam();
            
            // change to select robot
            ChangeBattleState(BattleState.SelectRobot);
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

            var text2 = "";
            try
            {
                var robot = GameManager.instance.gridManager.GetGridController().RobotController.Robot;
                text2 = $"Hover: {robot}\n" +
                       $"Health: {robot.CurrentHealth}";
            }catch (NullReferenceException)
            {
                text2 = "Empty";
            }
            
            GUI.Box (new Rect (0,Screen.height - 100,100,50), text);
            GUI.Box (new Rect (0,Screen.height - 50,100,50), text2);
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
        AttackRobot,
        DefendRobot,
    }
}

