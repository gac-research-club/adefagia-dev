﻿using System;
using System.Collections.Generic;
using Adefagia.Collections;
using Adefagia.GridSystem;
using UnityEngine;
using Adefagia.RobotSystem;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.BattleMechanism
{
    public class TeamController : MonoBehaviour
    {
        [SerializeField] private Team team;
        
        // Must be not SerializedField
        // But for debugging, this is important to show in inspector
        [SerializeField] private List<RobotController> robotControllers;
        [SerializeField] private RobotController robotControllerActive;
        [SerializeField] private RobotController robotControllerSelect;
        [SerializeField] private List<int> _robotDeployed;

        // Bound Area
        private Vector2 _startArea, _endArea;
        
        // Index Robot
        private int _index;

        #region Properties
        public Team Team => team;
        public int TotalRobot => robotControllers.Count;
        public Robot Robot => robotControllerActive.Robot;
        public RobotController RobotController => robotControllerActive;
        public GridController GridController { get; set; }
        
        // Robot Selected in Battle State
        public RobotController RobotControllerSelected { 
            get => robotControllerSelect;
            set => robotControllerSelect = value;
        }
        #endregion

        private void Update()
        {
            if (BattleManager.preparationState == PreparationState.DeployRobot)
            {
                SelectingRobot();
            }
        }
        
        // Area Selecting while preparation mode
        public void SetPreparationArea(int ax, int ay, int bx, int by)
        {
            _startArea = new Vector2(ax, ay);
            _endArea   = new Vector2(bx, by);
        }
        public bool IsGridInPreparationArea(Grid grid)
        {
            
            return (grid.X >= _startArea.x &&
                    grid.Y >= _startArea.y &&
                    grid.X <= _endArea.x   &&
                    grid.Y <= _endArea.y     );
        }

        // TODO: Team controller can change what robot is selected by UI user
        private void SelectingRobot()
        {
            // Only for the team active
            var teamActive = GameManager.instance.battleManager.TeamActive;
            if (this != teamActive) return;

            var last = RobotController;
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!IsHasDeployed(Robot))
                {
                    last.ResetPosition();
                }
                ChooseRobot(0);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!IsHasDeployed(Robot))
                {
                    last.ResetPosition();
                }
                ChooseRobot(1);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!IsHasDeployed(Robot))
                {
                    last.ResetPosition();
                }
                ChooseRobot(2);
            }
        }

        public void ChooseRobot()
        {
            robotControllerActive = robotControllers[_index];
        }
        public void ChooseRobot(int index)
        {
            _index = index;
            robotControllerActive = robotControllers[_index];
        }
        public void ChooseRobot(RobotController robotController)
        {
            robotControllerActive = robotController;
        }

        public void IncrementIndex()
        {
            while (true)
            {
                if (!_robotDeployed.Contains(_index) || _robotDeployed.Count == TotalRobot)
                {
                    return;
                }

                _index++;
                if (_index >= TotalRobot)
                {
                    _index = 0;
                }
            }
        }

        public void DeployRobot()
        {
            _robotDeployed.Add(_index);
        }

        public bool IsHasDeployed(Robot robot)
        {
            return _robotDeployed.Contains(robot.ID);
        }

        public bool IsHasFinishDeploy()
        {
            return _robotDeployed.Count == TotalRobot;
        }

        public bool Contains(RobotController robotController)
        {
            return robotControllers.Contains(robotController);
        }
        
        
        /*-----------------------------------------------------------------------
         * Change Team Robot Controllers
         *----------------------------------------------------------------------*/
        public void ChangeRobotController(List<RobotController> newRobotControllers)
        {
            robotControllers = newRobotControllers;
        }
        
        /*-----------------------------------------------------------------------
         * get reference from dummy gameObject
         *----------------------------------------------------------------------*/
        public GameObject GetRobotGameObject(int index)
        {
            // Out of range index is returning null
            if (index >= TotalRobot || index < 0) return null;

            return robotControllers[index].gameObject;
        }
    }
}