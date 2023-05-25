using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using Grid = Adefagia.GridSystem.Grid;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class HighlightMovement : MonoBehaviour
    {
        [SerializeField] private GameObject quadMove, quadAttack;

        private List<Grid> _tempGrids;
        private List<GameObject> _tempHighlights;
        private GameObject _quad;

        public void Awake()
        {
            _tempHighlights = new List<GameObject>();
            _tempGrids = new List<Grid>();
        }

        private void Start()
        {
            RobotAttack.ThingHappened += OnThingHappened;
        }

        public void OnThingHappened(RobotController robotController)
        {
            var gridController = robotController.GridController;
            Debug.Log($"{gridController.Grid} Highlight attack");
        }

        /*--------------
         *    o o o
         *    o r o
         *    o o o
         --------------*/
        public void SetSurroundMove(Grid grid)
        {
            if (grid == null) return;

            CleanHighlight();

            if (BattleManager.battleState == BattleState.MoveRobot)
            {
                _quad = quadMove;
            }
            else if (BattleManager.battleState == BattleState.AttackRobot)
            {
                _quad = quadAttack;
            }
            else
            {
                return;
            }
            var xGrid = grid.X;
            var yGrid = grid.Y;

            GridHighlight(xGrid + 0, yGrid + 1);
            GridHighlight(xGrid + 0, yGrid - 1);
            GridHighlight(xGrid - 1, yGrid + 0);
            GridHighlight(xGrid - 1, yGrid - 1);
            GridHighlight(xGrid - 1, yGrid + 1);
            GridHighlight(xGrid + 1, yGrid + 0);
            GridHighlight(xGrid + 1, yGrid - 1);
            GridHighlight(xGrid + 1, yGrid + 1);
        }

        /*--------------
         *      o
         *    o o o
         *  o o r o o
         --------------*/
        public void SetTankRow(TeamController teamActive)
        {
            var grid = teamActive.RobotControllerSelected.Robot.Location;
            if (grid == null) return;

            CleanHighlight();

            if (BattleManager.battleState == BattleState.MoveRobot)
            {
                _quad = quadMove;
            }
            else if (BattleManager.battleState == BattleState.AttackRobot)
            {
                _quad = quadAttack;
            }
            else
            {
                return;
            }

            var xGrid = grid.X;
            var yGrid = grid.Y;

            var whichTeam = teamActive.Team.teamName;
            if (whichTeam == "DIMOCRAT") // 3 Front Row team biru
            {
                GridHighlight(xGrid + 0, yGrid + 1);
                GridHighlight(xGrid + 0, yGrid + 2);
                GridHighlight(xGrid + 1, yGrid + 1);
                GridHighlight(xGrid - 1, yGrid + 1);
                GridHighlight(xGrid + 1, yGrid + 0);
                GridHighlight(xGrid + 2, yGrid + 0);
                GridHighlight(xGrid - 1, yGrid + 0);
                GridHighlight(xGrid - 2, yGrid + 0);

            }
            else //highlight kebalik
            {
                GridHighlight(xGrid + 0, yGrid - 1);
                GridHighlight(xGrid + 0, yGrid - 2);
                GridHighlight(xGrid - 1, yGrid - 1);
                GridHighlight(xGrid + 1, yGrid - 1);
                GridHighlight(xGrid - 1, yGrid + 0);
                GridHighlight(xGrid - 2, yGrid + 0);
                GridHighlight(xGrid + 1, yGrid + 0);
                GridHighlight(xGrid + 2, yGrid + 0);
            }
        }

        /*--------------
         *      o
         *    o o o
         *  o o r o o
         *    o o o
         *      o
         --------------*/
        public void SetDiamondSurroundMove(Grid grid)
        {
            if (grid == null) return;

            CleanHighlight();

            if (BattleManager.battleState == BattleState.MoveRobot)
            {
                _quad = quadMove;
            }
            else if (BattleManager.battleState == BattleState.AttackRobot)
            {
                _quad = quadAttack;
            }
            else
            {
                return;
            }

            var xGrid = grid.X;
            var yGrid = grid.Y;

            GridHighlight(xGrid + 0, yGrid + 1);
            GridHighlight(xGrid + 0, yGrid - 1);
            GridHighlight(xGrid + 1, yGrid + 0);
            GridHighlight(xGrid - 1, yGrid + 0);
            GridHighlight(xGrid + 1, yGrid + 1);
            GridHighlight(xGrid + 1, yGrid - 1);
            GridHighlight(xGrid - 1, yGrid - 1);
            GridHighlight(xGrid - 1, yGrid + 1);
            GridHighlight(xGrid + 2, yGrid + 0);
            GridHighlight(xGrid - 2, yGrid + 0);
            GridHighlight(xGrid + 0, yGrid + 2);
            GridHighlight(xGrid + 0, yGrid - 2);
        }

        /*--------------
         *      o
         *      o
         *      o
         *      r
         --------------*/
        public void ThreeFrontRow(TeamController teamActive)
        {
            var grid = teamActive.RobotControllerSelected.Robot.Location;
            if (grid == null) return;

            CleanHighlight();

            if (BattleManager.battleState == BattleState.MoveRobot)
            {
                _quad = quadMove;
            }
            else if (BattleManager.battleState == BattleState.AttackRobot)
            {
                _quad = quadAttack;
            }
            else
            {
                return;
            }

            var xGrid = grid.X;
            var yGrid = grid.Y;

            var whichTeam = teamActive.Team.teamName;
            if (whichTeam == "DIMOCRAT") // 3 Front Row team biru
            {
                GridHighlight(xGrid + 0, yGrid + 1);
                GridHighlight(xGrid + 0, yGrid + 2);
                GridHighlight(xGrid + 0, yGrid + 3);
            }
            else //highlight kebalik
            {
                GridHighlight(xGrid + 0, yGrid - 1);
                GridHighlight(xGrid + 0, yGrid - 2);
                GridHighlight(xGrid + 0, yGrid - 3);
            }

        }

        private void GridHighlight(int x, int y)
        {
            var grid = GameManager.instance.gridManager.GetGrid(x, y);
            if (grid == null) return;

            _tempGrids.Add(grid);

            var quadDup = Instantiate(_quad, transform);
            quadDup.transform.position = GridManager.CellToWorld(grid);

            _tempHighlights.Add(quadDup);
        }

        /*----------------------------------------------------------------------
         * Checking grid is on the list of highlight
         *----------------------------------------------------------------------*/
        public bool CheckGridOnHighlight(GridController gridController)
        {
            return _tempGrids.Contains(gridController.Grid);
        }

        public void CleanHighlight()
        {
            foreach (var temp in _tempHighlights)
            {
                Destroy(temp);
            }

            _tempHighlights.Clear();
            _tempGrids.Clear();
        }

        public enum TypePattern
        {
            Surround,
            Diamond,
        }
    }
}

