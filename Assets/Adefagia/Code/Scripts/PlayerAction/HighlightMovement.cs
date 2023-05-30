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
        [Header("Prefab highlight with different color")]
        [SerializeField] private GameObject quadMove, quadAttack;
        
        [Header("Prefab highlight block when grid occupied")]
        [SerializeField] private GameObject quadMoveBlock, quadAttackBlock;

        private List<Grid> _tempGrids;
        private List<GameObject> _tempHighlights;
        private GameObject _quad, _quadBlock;

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

            SetQuad();
            
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

            SetQuad();

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

            SetQuad();

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

            SetQuad();

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

            GameObject quadDup;

            if (grid.Status != GridStatus.Free)
            {
                Debug.Log("Grid Obstacle:" + grid);
                quadDup = Instantiate(_quadBlock, transform);
            }
            else
            {
                quadDup = Instantiate(_quad, transform);
            }

            _tempGrids.Add(grid);

            quadDup.transform.position = GridManager.CellToWorld(grid);

            _tempHighlights.Add(quadDup);
        }
        
        //
        // Set grid
        //
        private void SetQuad()
        {
            if (BattleManager.battleState == BattleState.MoveRobot)
            {
                _quad = quadMove;
                _quadBlock = quadMoveBlock;
            }
            else if (BattleManager.battleState == BattleState.AttackRobot)
            {
                _quad = quadAttack;
                _quadBlock = quadAttackBlock;
            }
        }

        /*----------------------------------------------------------------------
         * Checking grid is on the list of highlight
         *----------------------------------------------------------------------*/
        public bool CheckGridOnHighlight(GridController gridController)
        {
            return _tempGrids.Contains(gridController.Grid);
        }
        public bool CheckGridOnHighlight(Grid grid)
        {
            return _tempGrids.Contains(grid);
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

