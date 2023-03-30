using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using UnityEngine;

namespace Adefagia.Highlight{
    public class HighlightMovement : MonoBehaviour
    {

        [SerializeField] public GameObject quadMove;


        public void SetSurroundMove(Grid grid)
        {
            if(grid != null){
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
        }


        private void GridHighlight(int x, int y){
            var grid = GameManager.instance.gridManager.GetGrid(x, y);
            if(grid != null){
                GameObject quadDup = GameObject.Instantiate(quadMove);
                quadDup.transform.position = GameManager.instance.gridManager.CellToWorld(grid); 
            }
        }
    }
}

