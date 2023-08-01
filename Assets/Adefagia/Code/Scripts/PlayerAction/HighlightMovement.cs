using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        
        [SerializeField] private GameObject quadImpact;
        

        private List<Grid> _tempGrids;
        private List<Grid> _tempGridsImpact;

        private List<GameObject> _tempHighlights;
        private List<GameObject> _tempHighlightsImpact;
        private GameObject _quad, _quadBlock, _quadImpact, _quadBlockImpact;


        public static event Action<Grid> RobotOnImpact;
        public static event Action<List<Grid>> RobotOnImpactClear;

        public void Awake()
        {
            _tempHighlights = new List<GameObject>();
            _tempGrids = new List<Grid>();
            
            _tempHighlightsImpact = new List<GameObject>();
            _tempGridsImpact = new List<Grid>();
        }

        private void Start()
        {
            RobotAttack.ThingHappened += OnThingHappened;
            GridManager.SkillHappened += OnSkillHappened;
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

            var pattern = 
                "ooo" +
                "oro" +
                "ooo";
            var origin = new Vector2Int(1, 1);
            CreateFromPattern(pattern, 3,3, grid.Location, origin);
        }

        public void SetSurroundImpact(Grid grid)
        {
            if (grid == null) return;
            
            CleanHighlightImpact();
            
            SetQuadImpact();

            var pattern = 
                "ooo" +
                "oro" +
                "ooo";
            var origin = new Vector2Int(1, 1);
            CreateFromPatternImpact(pattern, 3,3, grid.Location, origin);
        }

        public void CreateHighlight(Grid grid, string pattern, Vector2Int origin)
        {
            if (grid == null) return;

            CleanHighlight();

            SetQuad();

            CreateFromPattern(pattern, 7,7, grid.Location, origin);
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

            var pattern = 
                "  o  " +
                " ooo " +
                "ooroo" +
                " ooo " +
                "  o  ";
            var origin = new Vector2Int(2, 2); // character 'r'
            CreateFromPattern(pattern, 5,5, grid.Location, origin);
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
        
        /*--------------
         *      
         *      o 
         *    o r o 
         *      o 
         *      
         --------------*/
        public void SetSmallDiamondMove(Grid grid)
        {
            if (grid == null) return;

            CleanHighlight();

            SetQuad();

            var pattern = 
                " o " +
                "oro" +
                " o ";
            var origin = new Vector2Int(1, 1);
            CreateFromPattern(pattern, 3,3, grid.Location, origin);
        }

        /*--------------
         *        o
         *        o 
         *    o o r o o
         *        o 
         *        o
         --------------*/
        public void SetCrossMove(Grid grid)
        {
            if (grid == null) return;

            CleanHighlight();

            SetQuad();

            var pattern = 
                "  o  " +
                "  o  " +
                "ooroo" +
                "  o  " +
                "  o  ";
            var origin = new Vector2Int(2, 2); // character 'r'
            CreateFromPattern(pattern, 5,5, grid.Location, origin);
        }

        
        private void GridHighlight(int x, int y)
        {
            var grid = GameManager.instance.gridManager.GetGrid(x, y);
            if (grid == null) return;

            GameObject quadDup;

            if (grid.Status != GridStatus.Free)
            {
                // Debug.Log("Grid Obstacle:" + grid);
                quadDup = Instantiate(_quadBlock, transform);
                
            }
            else
            {
                quadDup = Instantiate(_quad, transform);
            }

            _tempGrids.Add(grid);

            quadDup.transform.position = GridManager.CellToWorld(grid);
            quadDup.transform.localScale = GridManager.UpdateScale(quadDup.transform);

            _tempHighlights.Add(quadDup);
        }
        
        private void GridImpact(int x, int y)
        {
            var grid = GameManager.instance.gridManager.GetGrid(x, y);
            if (grid == null) return;

            GameObject quadDup;

            if (grid.Status != GridStatus.Free)
            {
                // Debug.Log("Grid Obstacle:" + grid);
                quadDup = Instantiate(_quadBlockImpact, transform);
                
                RobotOnImpact?.Invoke(grid);
            }
            else
            {
                quadDup = Instantiate(_quadImpact, transform);
            }

            _tempGridsImpact.Add(grid);
            
            RobotOnImpactClear?.Invoke(_tempGridsImpact);

            quadDup.transform.position = GridManager.CellToWorld(grid);
            quadDup.transform.localScale = GridManager.UpdateScale(quadDup.transform);

            _tempHighlightsImpact.Add(quadDup);
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
            else if (BattleManager.battleState == BattleState.AttackRobot ||
                     BattleManager.battleState == BattleState.SkillSelectionRobot)
            {
                _quad = quadAttack;
                _quadBlock = quadAttackBlock;
            }
            // Skill
            else
            {
                _quad = quadAttack;
                _quadBlock = quadAttackBlock;
            }
        }
        
        private void SetQuadImpact()
        {
            _quadImpact = quadImpact;
            _quadBlockImpact = quadImpact;
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
        
        public bool CheckGridOnHighlightImpact(GridController gridController)
        {
            return _tempGridsImpact.Contains(gridController.Grid);
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
        
        public void CleanHighlightImpact()
        {
            foreach (var temp in _tempHighlightsImpact)
            {
                Destroy(temp);
            }

            _tempHighlightsImpact.Clear();
            _tempGridsImpact.Clear();
        }

        private void CreateFromPattern(string pattern, int row, int col, Vector2Int position, Vector2Int origin)
        {
            string replacement = Regex.Replace(pattern, @"\t|\n|\r", "");
            int x = 0, y = row-1;
            foreach (var character in replacement)
            {
                // Debug.Log($"({x},{y}): {character}");
                if (character.Equals('o'))
                {
                    GridHighlight(position.x + (x-origin.x), position.y + (y-origin.y));
                }
                
                x++;
                if (x > col-1)
                {
                    y--;
                    x = 0;
                }
            }
        }
        
        private void CreateFromPatternImpact(string pattern, int row, int col, Vector2Int position, Vector2Int origin)
        {
            string replacement = Regex.Replace(pattern, @"\t|\n|\r", "");
            int x = 0, y = row-1;
            foreach (var character in replacement)
            {
                // Debug.Log($"({x},{y}): {character}");
                if (character.Equals('o'))
                {
                    GridImpact(position.x + (x-origin.x), position.y + (y-origin.y));
                }
                
                x++;
                if (x > col-1)
                {
                    y--;
                    x = 0;
                }
            }
        }

        
        

        private void OnSkillHappened(GridController gridController)
        {
            var grid = gridController.Grid;
            // Debug.Log(grid);
            // GridImpact(grid.X, grid.Y);
        }

        public enum TypePattern
        {
            Surround,
            Diamond,
        }
    }
}

