using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using Adefagia.Inventory;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using Random = UnityEngine.Random;

namespace Adefagia.BattleMechanism
{
    public class BattleManager : MonoBehaviour
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
        private int countRound = 1;
        private Potion potionChoosed = null;

        private bool moveFinish;

        // public static Logging GameManager.instance.logManager;
        
        public static event Action<RobotController> RobotNotHaveSkill;
        public static event Action<Robot> SelectRobot;
        public static event Action<string> ChangeTurn; 

        private void Awake()
        {
            // Set into battleManager
            if (GameManager.instance != null)
            {
                GameManager.instance.battleManager = this;
            }
            
            healthBars = new List<GameObject>();
            // GameManager.instance.logManager = new Logging();

            /* Team A deploying Area
             *  #  ........ 9,9
             * 0,6 ........  #
             *----------------------*/
            teamA.SetPreparationArea(0, 6, 9, 9);


            /* Team B deploying Area
             *  #  ........ 9,3
             * 0,0 ........  #
             *----------------------*/
            teamB.SetPreparationArea(0, 0, 9, 3);

            highlightMovement = GetComponent<HighlightMovement>();

            // Initialize current Time
            currentTime = startingTime;

            StartCoroutine(PreparationBattle());
        }

        private void Start()
        {
            // GridManager.GridHover += Test;
            // HighlightMovement.RobotOnImpact += OnRobotImpacted; [deprecated]
            // HighlightMovement.RobotOnImpactClear += OnRobotClearImpacted; [deprecated]
        }

        private void Update()
        {
            #region Preparation

            if (gameState == GameState.Preparation &&
                preparationState == PreparationState.DeploySelect)
            {
                // Change Team Activate if has deployed all the robot
                if (TeamActive.IsHasFinishDeploy())
                {
                    // Reset team active robot selected
                    TeamActive.ResetRobotSelected();
                    TeamActive.SetPreparationArea(0, 0, 9, 9); // full area

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
                        
                        
                        GameManager.instance.uiManager.HideCharacterSelectCanvas();
                        GameManager.instance.uiManager.ShowBattleUI();
                        
                        // Enable healthbars when both teams deployed
                        GameManager.instance.uiManager.EnableHealthBars(TeamActive.IsHasFinishDeploy());
                    }
                }
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
                    battleState == BattleState.SkillSelectionRobot ||
                    battleState == BattleState.ItemRobot ||
                    battleState == BattleState.ItemSelectionRobot)
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

                    GameManager.instance.uiManager.uiBattleController.timer.text = (int)currentTime + " Second";

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
            
            UpdateSlider();
            
            if (gameState == GameState.Battle)
            {
                // Hide PlayerActionHUD
                // GameManager.instance.uiManager.HideBattleUI();
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

            GameManager.instance.logManager.LogStep($"{teamController.Team.teamName} is Winning");

            // reset timer
            currentTime = -1;

            GameManager.instance.uiManager.ShowFinishUI(teamController.Team.teamName);
        }
        
        // public void Test(GridController gridController)
        // {
        //     // Click on the grid highlighted
        //     if (highlightMovement.CheckGridOnHighlight(gridController))
        //     {
        //         // run AStar Pathfinding
        //
        //         highlightMovement.SetSurroundImpact(gridController.Grid);
        //     }
        //     
        //     // localHighlight.SetSurroundImpact(gridController.Grid);
        // }

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

        private void UpdateSlider()
        {
            for (int i = 0; i < 3; i++)
            {
                var slider = GameManager.instance.uiManager.uiBattleController.healthBarSliders[i];
                var robot = TeamActive.GetRobotController(i);

                if (robot == null)
                {
                    slider.maxValue = 1;
                    slider.value = 0;
                }
                else
                {
                    slider.maxValue = robot.Robot.MaxHealth;
                    slider.value = robot.Robot.CurrentHealth;
                }
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

                GameManager.instance.logManager.LogStep($"{TeamActive.Team.teamName} - {TeamActive.RobotController.Robot} - Deploy to {TeamActive.GridController.Grid}", LogManager.LogText.Info);

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
                    
                    if (TeamActive.RobotControllerSelected){

                        var robot = TeamActive.RobotControllerSelected.Robot;
                        
                        // Update healthbar slider
                        SelectRobot?.Invoke(robot);
                        
                        // Hide skill button
                        RobotNotHaveSkill?.Invoke(TeamActive.RobotControllerSelected);
                    }   
                    
                    // Show or Hide Battle UI
                    // if (TeamActive.RobotControllerSelected != null)
                    // {
                    //     GameManager.instance.uiManager.ShowBattleUI();
                    // }
                    // else
                    // {
                    //     GameManager.instance.uiManager.HideBattleUI();
                    // }
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

                    UpdateSlider();

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
                    if (highlightMovement.CheckGridOnHighlight(gridController))
                    {
                        TeamActive.RobotControllerSelected.RobotSkill.Skill(
                            robotController: TeamActive.RobotControllerSelected,
                            gridController: gridController
                        );
                    }

                    UpdateSlider();
                    
                    // change to selecting state
                    ChangeBattleState(BattleState.SelectRobot);

                    // Clear highlight
                    highlightMovement.CleanHighlight();
                    highlightMovement.DestroyAllHighlight();
                }


                /*---------------------------------------------------------------
                 * Item Robot
                 *---------------------------------------------------------------*/
                if (battleState == BattleState.ItemSelectionRobot)
                {
                    
                    // Click on the grid highlighted
                    TeamActive.RobotControllerSelected.RobotItem.Item(
                        robotController: TeamActive.RobotControllerSelected,
                        potion: potionChoosed
                    );

                    UpdateSlider();
                    
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

            // highlight grid movement  by weapon type pattern
            if(TeamActive.RobotControllerSelected.Robot != null){
                Robot robot = TeamActive.RobotControllerSelected.Robot;
            
                if(robot.TypePattern == TypePattern.Cross){
                    highlightMovement.SetSmallDiamondMove(robot.Location);
                }else if(robot.TypePattern == TypePattern.SmallDiamond){
                    highlightMovement.SetCrossMove(robot.Location);
                }else{
                    highlightMovement.SetSurroundMove(robot.Location);
                };
            }
        
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

            if(TeamActive.RobotControllerSelected.Robot != null){

                // highlight grid attack  by weapon type pattern
                Robot robot = TeamActive.RobotControllerSelected.Robot;
                
                if (robot.TypePattern == TypePattern.Cross)
                {
                    highlightMovement.SetSmallDiamondMove(robot.Location);
                }
                else if (robot.TypePattern == TypePattern.SmallDiamond)
                {
                    highlightMovement.SetCrossMove(robot.Location);
                }
                else
                {
                    highlightMovement.SetSurroundMove(robot.Location);
                }
            }

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Attack");
        }

        public void SkillButtonClick()
        {
            highlightMovement.CleanHighlight();
            
            // change to defend robot
            // ChangeBattleState(BattleState.SkillRobot);
            

            // means the robot is considered to move
            // TeamActive.RobotControllerSelected.Robot.HasSkill = true;

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} List Skill Active");
        }

        public void ItemButtonClick()
        {
            // change to defend robot
            ChangeBattleState(BattleState.ItemRobot);

            // means the robot is considered to move
            // TeamActive.RobotControllerSelected.Robot.HasSkill = true;

            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} List Skill Active");
        }

        public void SkillChildButtonClick(int indexSkill)
        {
            // change to skill selection robot
            ChangeBattleState(BattleState.SkillSelectionRobot);
            
            // highlight grid attack  by weapon type pattern
            Robot robot = TeamActive.RobotControllerSelected.Robot;
            Skill skill = TeamActive.RobotControllerSelected.GetSkill(indexSkill);
            
            // Set skill
            robot.SkillSelected = skill;

            // Pattern skill highlight
            if (skill.PatternAttack == TypePattern.Cross)
            {
                highlightMovement.SetSmallDiamondMove(robot.Location);
            } else if (skill.PatternAttack == TypePattern.SmallDiamond)
            {
                highlightMovement.SetCrossMove(robot.Location);
            } else if (skill.PatternAttack == TypePattern.Diamond)
            {
                highlightMovement.SetDiamondSurroundMove(robot.Location);
            } else {
                highlightMovement.SetSurroundMove(robot.Location);
            }

            // means the robot is considered to move
            // TeamActive.RobotControllerSelected.Robot.HasSkill = true;
            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Skill Active");
        }

        public void ItemChildButtonClick(int item)
        {
            // Get Potion from RobotSelected
            RobotController robotController = TeamActive.RobotControllerSelected;
            PotionController potionController = robotController.PotionController;
            
            Potion potion = potionController.ChoosePotion(item);
            potionChoosed = potion;

            potion.HasUsed = true;

            // change to skill selection robot
            ChangeBattleState(BattleState.ItemSelectionRobot);
 
            // means the robot is considered to move
            Debug.Log($"{TeamActive.RobotControllerSelected.Robot} Item Active");
        }

        public void EndTurnButtonClick()
        {
            // starting with initial starting time
            currentTime = startingTime;

            TeamActive.ResetRobotSelected();
            TeamActive.HasRound = true;
            
            // Logging
            GameManager.instance.logManager.LogStep($"{TeamActive.Team.teamName} - End Turn", LogManager.LogText.Info);
            
            if(NextTeam.HasRound && TeamActive.HasRound){
                ChangeRound();
            }

            ChangeTeam();

            ChangeTurn?.Invoke(TeamActive.Team.teamName);

            // change to select robot
            ChangeBattleState(BattleState.SelectRobot);
        }

        public void ChangeRound(){
            countRound++;
            
            TeamActive.IncreaseRobotStamina();
            NextTeam.IncreaseRobotStamina();

            TeamActive.ResetBuffRobot();
            NextTeam.ResetBuffRobot();
            
            // Logging
            GameManager.instance.logManager.LogStep($"Round {countRound} !!", LogManager.LogText.Common);
            
            NextTeam.HasRound = false;
            TeamActive.HasRound = false;
        }

        public void CancelButtonClick()
        {
            ChangeBattleState(BattleState.SelectRobot);

            highlightMovement.CleanHighlight();
        }

        public void ChangeRobotSelect(int index)
        {
            TeamActive.RobotControllerSelected = TeamActive.GetRobotController(index);
            
            // Hide skill button
            RobotNotHaveSkill?.Invoke(TeamActive.RobotControllerSelected);
        }
        
        // Bot button
        public void MoveBotButtonClick()
        {
            
            MoveButtonClick();
            
            Invoke("WaitMove", 1);
        }
        
        public void AttackBotButtonClick()
        {
            
            AttackButtonClick();
            
            Invoke("WaitAttack", 1);
        }

        public void AutoBotClick()
        {
            
            AutoChangeIndex();
            
            Invoke("MoveBotButtonClick", 1f);

            Invoke("AttackBotButtonClick", 4f);
        }

        public void AutoChangeIndex()
        {
            // Select Robot
            if (battleState == BattleState.SelectRobot)
            {
                TeamActive.RobotControllerSelected = TeamActive.robotControllers[TeamActive.count];

                if (TeamActive.RobotControllerSelected){

                    var robot = TeamActive.RobotControllerSelected.Robot;
                        
                    // Update healthbar slider
                    SelectRobot?.Invoke(robot);
                        
                    // Hide skill button
                    RobotNotHaveSkill?.Invoke(TeamActive.RobotControllerSelected);
                }
                
                TeamActive.IncrementCount();
            }
        }

        private void WaitMove()
        {
            if (battleState == BattleState.MoveRobot)
            {
                // run AStar Pathfinding
                TeamActive.RobotControllerSelected.RobotMovement.MoveBot(
                    robotController: TeamActive.RobotControllerSelected,
                    speed: TeamActive.RobotControllerSelected.Robot.Speed
                );

                // change to selecting state
                ChangeBattleState(BattleState.SelectRobot);
                
                highlightMovement.CleanHighlight();
            }

        }
        
        private void WaitAttack()
        {
            if (battleState == BattleState.AttackRobot)
            {
                
                TeamActive.RobotControllerSelected.RobotAttack.AttackBot(
                    robotController: TeamActive.RobotControllerSelected
                );

                UpdateSlider();

                // change to selecting state
                ChangeBattleState(BattleState.SelectRobot);

                // Clear highlight
                highlightMovement.CleanHighlight();
            }
        }

        #endregion

        // public void OnRobotImpacted(Grid grid)
        // {
        //     var gridCtrl = GameManager.instance.gridManager.GetGridController(grid);
        //
        //     if (gridCtrl.Grid.Status != GridStatus.Free)
        //     {
        //         _gridImpacts[grid] = gridCtrl;
        //     }
        // }

        // public void OnRobotClearImpacted(List<Grid> tempGridImpact)
        // {
        //     var listGrid = _gridImpacts.Keys.ToList();
        //     for (int i = listGrid.Count-1; i >= 0; i--)
        //     {
        //         if (!tempGridImpact.Contains(listGrid[i]))
        //         {
        //             _gridImpacts.Remove(listGrid[i]);
        //         }
        //     }
        // }
    }


    public enum GameState
    {
        Initialize,
        Preparation,
        Battle,
        Finish
    }

    public enum PreparationState
    {
        Nothing,
        SelectTeam,
        DeployRobot,
        DeploySelect,
    }

    public enum BattleState
    {
        Nothing,
        SelectRobot,
        MoveRobot,
        AttackRobot,
        SkillRobot,
        SkillSelectionRobot,
        ItemRobot,
        ItemSelectionRobot
    }
}