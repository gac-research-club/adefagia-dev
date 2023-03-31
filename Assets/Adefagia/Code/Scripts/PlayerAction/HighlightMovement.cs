using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class HighlightMovement : MonoBehaviour
    {
        [SerializeField] private GameObject quadMove, quadAttack;

        private List<GameObject> _tempHighlight;
        private GameObject _quad;

        public void Awake()
        {
            _tempHighlight = new List<GameObject>();
        }

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

        private void GridHighlight(int x, int y)
        {
            var grid = GameManager.instance.gridManager.GetGrid(x, y);
            if (grid == null) return;

            var quadDup = Instantiate(_quad, transform);
            quadDup.transform.position = GridManager.CellToWorld(grid);
            _tempHighlight.Add(quadDup);      
        }
        
        public void CleanHighlight()
        {
            foreach (var temp in _tempHighlight)
            {
                Destroy(temp);
            }
            
            _tempHighlight.Clear();
        }

        public enum TypePattern
        {
            Surround,
            Diamond,
        }
    }
}

