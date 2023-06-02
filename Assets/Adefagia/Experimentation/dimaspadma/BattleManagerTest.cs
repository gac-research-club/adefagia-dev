using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using Random = UnityEngine.Random;

namespace Adefagia.BattleMechanism
{
    public class BattleManagerTest : MonoBehaviour
    {
        [SerializeField] private TeamController teamA, teamB;
        [SerializeField] private float startingTime = 10f;
        public static List<GameObject> healthBars;

        // Before Battle start
        public static GameState gameState = GameState.Initialize;
        public static PreparationState preparationState = PreparationState.Nothing;
        public static BattleState battleState = BattleState.Nothing;

        public static HighlightMovement highlightMovement;

        public static TeamController TeamActive { get; set; }
        public static TeamController NextTeam { get; set; }

        public static float currentTime = -1;
        private int skillChoosed = 0;

        private void Awake()
        {
            healthBars = new List<GameObject>();

            highlightMovement = GetComponent<HighlightMovement>();

            // Initialize current Time
            currentTime = startingTime;

            StartCoroutine(PreparationBattle());
        }

        private void Update()
        {
            #region Preparation

            if (gameState == GameState.Preparation &&
                preparationState == PreparationState.DeploySelect)
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

                // Enable healthbars when both teams deployed
                GameManager.instance.uiManager.EnableHealthBars(TeamActive.IsHasFinishDeploy());
            }

            // Deploying robot
            if (gameState == GameState.Preparation && 
                preparationState == PreparationState.DeployRobot)
            {

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
                if (TeamActive.TotalRobot == 0)
                {
                    TeamWin(NextTeam);
                }

                if (NextTeam.TotalRobot == 0)
                {
                    TeamWin(TeamActive);
                }

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

                if (battleState == BattleState.MoveRobot ||
                    battleState == BattleState.AttackRobot ||
                    battleState == BattleState.SkillRobot ||
                    battleState == BattleState.SkillSelectionRobot)
                {
                    // Exit to select another Robot
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CancelButtonClick();
                    }
                }

                if (TeamActive.IsHasFinishDeploy())
                {
                    currentTime -= 1 * Time.deltaTime; // seconds

                    if (currentTime <= 0)
                    {
                        EndTurnButtonClick();
                    }
                }

            }

            #endregion
        }

        // First Round
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
            
            // Edit text team name
            ChangePreparationState(PreparationState.DeploySelect);
            GameManager.instance.uiManager.ChangeTextTeam(TeamActive.Team.teamName);
            
            // ChangePreparationState(PreparationState.DeployRobot);
        }

        private void ChangeTeam()
        {
            // Swap via destruction
            (TeamActive, NextTeam) = (NextTeam, TeamActive);
            
            if (gameState == GameState.Battle)
            {
                // Hide PlayerActionHUD
                GameManager.instance.uiManager.HideBattleUI();

                TeamActive.IncreaseRobotStamina();
                highlightMovement.CleanHighlight();
            }
            else
            {
                // Edit text team name
                GameManager.instance.uiManager.ChangeTextTeam(TeamActive.Team.teamName);
                // Reset character select
                // ChangePreparationState(PreparationState.DeploySelect);
                GameManager.instance.uiManager.ResetButtonSelect();
                GameManager.instance.uiManager.ShowCharacterSelectCanvas();
            }
        }

        public void TeamWin(TeamController teamController)
        {
            Debug.Log($"Team {teamController.Team.teamName} is Winning");


            ChangeBattleState(BattleState.Nothing);
            ChangeGameState(GameState.Finish);

            // reset timer
            currentTime = -1;

            GameManager.instance.uiManager.ShowFinishUI(teamController.Team.teamName);
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
                
                // Grid not obstacle
                if (TeamActive.GridController.Grid.Status == GridStatus.Obstacle)
                {
                    return;
                }

                TeamActive.DeployRobot();

                // set robot to grid
                TeamActive.GridController.RobotController = TeamActive.RobotController;

                // set grid to robot
                TeamActive.GridController.RobotController.GridController = TeamActive.GridController;

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

                /*---------------------------------------------------------------
                 * Move Robot
                 *---------------------------------------------------------------*/
                if (battleState == BattleState.MoveRobot)
                {
                    // Get current grid click
                    var gridController = GameManager.instance.gridManager.GetGridController();

                    // Click on the grid highlighted
                    if (highlightMovement.CheckGridOnHighlight(gridController))
                    {
                        // run AStar Pathfinding
                        TeamActive.RobotControllerSelected.RobotMovement.Move(
                            robotController: TeamActive.RobotControllerSelected,
                            gridController: gridController,
                            speed: TeamActive.RobotControllerSelected.Robot.Speed
                            );
                    }

                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);

                    // Clear highlight
                    highlightMovement.CleanHighlight();
                }


                /*---------------------------------------------------------------
                 * Attack Robot
                 *---------------------------------------------------------------*/
                if (battleState == BattleState.AttackRobot)
                {
                    // Get current grid click
                    var gridController = GameManager.instance.gridManager.GetGridController();

                    // Click on the grid highlighted
                    if (highlightMovement.CheckGridOnHighlight(gridController))
                    {
                        TeamActive.RobotControllerSelected.RobotAttack.Attack(
                            robotController: TeamActive.RobotControllerSelected,
                            gridController: gridController
                        );
                    }

                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);

                    // Clear highlight
                    highlightMovement.CleanHighlight();
                }


                /*---------------------------------------------------------------
                 * Skill Robot
                 *---------------------------------------------------------------*/
                if (battleState == BattleState.SkillSelectionRobot)
                {
                    // Get current grid click
                    var gridController = GameManager.instance.gridManager.GetGridController();

                    // Click on the grid highlighted
                    TeamActive.RobotControllerSelected.RobotSkill.Skill(
                        robotController: TeamActive.RobotControllerSelected,
                        gridController: gridController,
                        skillChoosed: skillChoosed
                    );
                    
                    if (highlightMovement.CheckGridOnHighlight(gridController))
                    {
                    }

                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);

                    // Clear highlight
                    highlightMovement.CleanHighlight();
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

            // highlight grid movement
            //highlightMovement.SetSurroundMove(TeamActive.RobotControllerSelected.Robot.Location);
            //highlightMovement.ThreeFrontRow(TeamActive);
            highlightMovement.SetDiamondSurroundMove(TeamActive.RobotControllerSelected.Robot.Location);
            //highlightMovement.SetTankRow(TeamActive);

            // Running Function Move from RobotMovement.cs

            // Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Move");
        }

        /*----------------------------------------------------------------------
         * Click attack button in UI step
         *----------------------------------------------------------------------*/
        public void AttackButtonClick()
        {
            // change to move robot
            ChangeBattleState(BattleState.AttackRobot);

            // highlight grid movement
            highlightMovement.SetSurroundMove(TeamActive.RobotControllerSelected.Robot.Location);
            //highlightMovement.ThreeFrontRow(TeamActive);
            //highlightMovement.SetDiamondSurroundMove(TeamActive.RobotControllerSelected.Robot.Location);
            //highlightMovement.SetTankRow(TeamActive);

            // Running Function Attack from RobotAttack.cs
            // Debug.Log("17.6");
            // Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Attack");
        }

        public void SkillButtonClick()
        {
            // change to defend robot
            ChangeBattleState(BattleState.SkillRobot);

            // means the robot is considered to move
            TeamActive.RobotControllerSelected.Robot.HasSkill = true;

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} List Skill Active");
        }

        public void SkillChildButtonClick(int skill)
        {

            // change to skill selection robot
            ChangeBattleState(BattleState.SkillSelectionRobot);
            skillChoosed = skill;
            // means the robot is considered to move
            TeamActive.RobotControllerSelected.Robot.HasSkill = true;

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Skill Active");
        }

        public void EndTurnButtonClick()
        {
            // starting with initial starting time
            currentTime = startingTime;

            TeamActive.ResetRobotSelected();

            ChangeTeam();

            // change to select robot
            ChangeBattleState(BattleState.SelectRobot);
        }

        public void CancelButtonClick()
        {
            ChangeBattleState(BattleState.SelectRobot);

            highlightMovement.CleanHighlight();
        }

        #endregion
    }
}
