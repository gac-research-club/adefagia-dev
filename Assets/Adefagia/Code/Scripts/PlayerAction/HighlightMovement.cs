using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Adefagia.BattleMechanism;
using Adefagia.Collections;
using Adefagia.GridSystem;
using Adefagia.ObstacleSystem;
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
        private List<GameObject> _tempHighlights;
        
        private GameObject _quad, _quadBlock, _quadImpact, _quadBlockImpact;
        private List<string> variantPattern = new List<string>();

        public static event Action<RobotController> AreaHighlight; 
        public static event Action<ObstacleController> AreaObstacleHighlight; 
        public static event Action AreaCleanHighlight; 

        public void Awake()
        {
            _tempHighlights = new List<GameObject>();
            _tempGrids = new List<Grid>();
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

        // [Deprecated]
        // public void CreateHighlight(Grid grid, string pattern, Vector2Int origin)
        // {
        //     if (grid == null) return;
        //
        //     CleanHighlight();
        //
        //     SetQuad();
        //
        //     CreateFromPattern(pattern, 7,7, grid.Location, origin);
        // }

        /*--------------
         *      o
         *    o o o
         *  o o r o o
         --------------*/
        // [Deprecated]
        // public void SetTankRow(TeamController teamActive)
        // {
        //     var grid = teamActive.RobotControllerSelected.Robot.Location;
        //     if (grid == null) return;
        //
        //     CleanHighlight();
        //
        //     SetQuad();
        //
        //     var xGrid = grid.X;
        //     var yGrid = grid.Y;
        //
        //     var whichTeam = teamActive.Team.teamName;
            // if (whichTeam == "DIMOCRAT") // 3 Front Row team biru
            // {
            //     GridHighlight(xGrid + 0, yGrid + 1);
            //     GridHighlight(xGrid + 0, yGrid + 2);
            //     GridHighlight(xGrid + 1, yGrid + 1);
            //     GridHighlight(xGrid - 1, yGrid + 1);
            //     GridHighlight(xGrid + 1, yGrid + 0);
            //     GridHighlight(xGrid + 2, yGrid + 0);
            //     GridHighlight(xGrid - 1, yGrid + 0);
            //     GridHighlight(xGrid - 2, yGrid + 0);
            //
            // }
            // else //highlight kebalik
            // {
            //     GridHighlight(xGrid + 0, yGrid - 1);
            //     GridHighlight(xGrid + 0, yGrid - 2);
            //     GridHighlight(xGrid - 1, yGrid - 1);
            //     GridHighlight(xGrid + 1, yGrid - 1);
            //     GridHighlight(xGrid - 1, yGrid + 0);
            //     GridHighlight(xGrid - 2, yGrid + 0);
            //     GridHighlight(xGrid + 1, yGrid + 0);
            //     GridHighlight(xGrid + 2, yGrid + 0);
            // }
        // }

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
        // [Deprecated]
        // public void ThreeFrontRow(TeamController teamActive)
        // {
        //     var grid = teamActive.RobotControllerSelected.Robot.Location;
        //     if (grid == null) return;
        //
        //     CleanHighlight();
        //
        //     SetQuad();
        //
        //     var xGrid = grid.X;
        //     var yGrid = grid.Y;
        //
        //     var whichTeam = teamActive.Team.teamName;
            // if (whichTeam == "DIMOCRAT") // 3 Front Row team biru
            // {
            //     GridHighlight(xGrid + 0, yGrid + 1);
            //     GridHighlight(xGrid + 0, yGrid + 2);
            //     GridHighlight(xGrid + 0, yGrid + 3);
            // }
            // else //highlight kebalik
            // {
            //     GridHighlight(xGrid + 0, yGrid - 1);
            //     GridHighlight(xGrid + 0, yGrid - 2);
            //     GridHighlight(xGrid + 0, yGrid - 3);
            // }

        // }
        
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

        
        private void AddGridHighlight(Grid grid)
        {
            _tempGrids.Add(grid);
        }
        

        private void CreateHighlightObject(Grid gridRobot, List<Grid> highlightGrids)
        {
            var removedGrid = new List<Grid>();

            var end = gridRobot;
            end.SetFree();
            
            highlightGrids.Add(end);
            
            foreach (var highlightGrid in highlightGrids)
            {
                if (highlightGrid == end) continue;
                
                var aStar = new AStar();
                if (!aStar.PathfindingCustomList(highlightGrids, highlightGrid, end))
                {
                    removedGrid.Add(highlightGrid);
                    continue;
                }
                
                ChangeMaterial(highlightGrid);
                
                GameObject quadDup;
                
                if (highlightGrid.Status != GridStatus.Free)
                {
                    // Debug.Log("Grid Obstacle:" + grid);
                    quadDup = Instantiate(_quadBlock, transform);
                
                }
                else
                {
                    quadDup = Instantiate(_quad, transform);
                }
                
                // Transform
                quadDup.transform.position = GridManager.CellToWorld(highlightGrid);
                quadDup.transform.localScale = GridManager.UpdateScale(quadDup.transform);
                
                _tempHighlights.Add(quadDup);
                // Debug.Log("Create Highlight Object");
                
            }
            
            EliminateGridHighlight(removedGrid);
        }

        private void EliminateGridHighlight(List<Grid> removedGrid)
        {
            // Eliminate grids
            foreach (var grid in removedGrid)
            {
                _tempGrids.Remove(grid);
            }
        }

        private void ChangeMaterial(Grid grid)
        {
            if (BattleManager.battleState == BattleState.AttackRobot ||
                BattleManager.battleState == BattleState.SkillSelectionRobot)
            {
                if (grid.Status == GridStatus.Robot)
                {
                    var robotController = GameManager.instance.gridManager.GetGridController(grid).RobotController;
                    AreaHighlight?.Invoke(robotController);
                }

                if (grid.Status == GridStatus.Obstacle)
                {
                    var obstacleController = GameManager.instance.gridManager.GetGridController(grid).ObstacleController;
                    AreaObstacleHighlight?.Invoke(obstacleController);
                }
            }
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

        public void CleanHighlight()
        {
            
            // invoke on clean highlight
            AreaCleanHighlight?.Invoke();
            
            foreach (var temp in _tempHighlights)
            {
                Destroy(temp);
            }

            _tempHighlights.Clear();
            _tempGrids.Clear();
        }

        private void CreateFromPattern(string pattern, int row, int col, Vector2Int position, Vector2Int origin)
        {
            var gridRobot = GameManager.instance.gridManager.GetGrid(position.x, position.y);
            if (gridRobot == null) return;
            
            string replacement = Regex.Replace(pattern, @"\t|\n|\r", "");
            
            int x = 0, y = row-1;
            foreach (var character in replacement)
            {
                // Debug.Log($"({x},{y}): {character}");
                if (character.Equals('o'))
                {
                    var worldX = position.x + (x - origin.x);
                    var worldY = position.y + (y - origin.y);
                    
                    var grid = GameManager.instance.gridManager.GetGrid(new Vector2Int(worldX, worldY));
                    if (grid != null)
                    {
                        AddGridHighlight(grid);
                    }
                }
                
                x++;
                if (x > col-1)
                {
                    y--;
                    x = 0;
                }
            }
            
            CreateHighlightObject(gridRobot, _tempGrids);
        }
        
        private void OnSkillHappened(GridController gridController)
        {
            var grid = gridController.Grid;
            // Debug.Log(grid);
            // GridImpact(grid.X, grid.Y);
        }

        private GridDirection CheckDirection(Vector2Int position, Vector2Int coordinate)
        {
            var direction = (coordinate - position) / (Heuristic(position, coordinate));
            if (direction == new Vector2Int(1, 0))
            {
                return GridDirection.Right;
            }

            if (direction == new Vector2Int(-1, 0))
            {
                return GridDirection.Left;
            }
            if (direction == new Vector2Int(0, 1))
            {
                return GridDirection.Up;
            }
            return GridDirection.Down;
        }

        private int Heuristic(Vector2Int position, Vector2Int coordinate)
        {
            return coordinate.x - position.x + coordinate.y - position.y;
        }

    }
}

