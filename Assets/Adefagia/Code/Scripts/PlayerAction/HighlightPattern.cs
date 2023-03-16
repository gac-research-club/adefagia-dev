using System;
using UnityEngine;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.PlayerAction
{
    public class HighlightPattern : MonoBehaviour
    {
        public Vector2[] movementPattern;
        public Vector2[] attackPattern;
        public Vector2[] attackPatternDiamond;
        // public Vector2 lastLoc;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private GameObject actionButton;

        public HighlightPattern()
        {
            /*  8 Directions Movement Pattern
                X X X
                X O X
                X X X
            */
            movementPattern = new Vector2[] { Vector2.right, Vector2.up , Vector2.left, Vector2.down,
            new Vector2(-1,1), new Vector2(-1,-1), new Vector2(1,1), new Vector2(1,-1) };

            /*  4 Front Rows Attack Pattern
                X X X
                X X X
                X X X
                X O X
            */
            attackPattern = new Vector2[] { Vector2.right, Vector2.up, Vector2.left, 
            new Vector2(-1,1), new Vector2(1,1), 
            new Vector2(-1,2), new Vector2(0,2), new Vector2(1,2), 
            new Vector2(-1,3), new Vector2(0,3), new Vector2(1,3), };

            /*  4 Front Rows Attack Pattern
                
                      X 
                    X X X
                  X X O X X
                    X X X
                      X
            */
            attackPatternDiamond = new Vector2[] { Vector2.right, Vector2.up , Vector2.left, Vector2.down,
            new Vector2(-1,1), new Vector2(-1,-1), new Vector2(1,1), new Vector2(1,-1),
            new Vector2(0,2), new Vector2(0,-2), new Vector2(-2,0), new Vector2(2,0) 
            };

        }

        // public Grid[] GetHighlightGrid(Vector2[] pattern, Vector2 lastLoc)
        // {
        //     // Add 8 BasicMovement Pattern : right, up, left, down, + 4 diagonal quads
        //     Vector2[] dirsMovement = pattern;
        //     
        //     var grid = gridManager.GetGridByLocation(lastLoc);
        //     grid.movementGrid = new Grid[dirsMovement.Length];
        //     
        //     for (var i=0; i<dirsMovement.Length; i++)
        //     {
        //         grid.movementGrid[i] = gridManager.GetGridByLocation(lastLoc + dirsMovement[i]);
        //         if (Graph.GridManager.IsGridEmpty(grid.movementGrid[i]))
        //         {
        //             grid.movementGrid[i] = null;
        //         } else 
        //         {
        //             grid.movementGrid[i] = grid.movementGrid[i];
        //         }
        //     }
        //     
        //     return grid.movementGrid;
        // }

        public void SetActiveHighlightMovement(Grid[] grid)
        {
            // for(var i=0; i<grid.Length; i++)
            // {
            //     if(grid[i] != null)
            //     {
            //         foreach (var grids in gridManager._allGridTransform)
            //         {
            //             if(grids.Value.location == grid[i].location)
            //             {
            //                 GameObject highlight = grids.Value._gameObject.transform.GetChild(0).gameObject;
            //                 highlight.SetActive(true);
            //             }
            //         }
            //     }
            // }
        }

        public void SetDisableHighlightMovement(Grid[] grid)
        {
            // for(var i=0; i<grid.Length; i++)
            // {
            //     if(grid[i] != null)
            //     {
            //         foreach (var grids in gridManager._allGridTransform)
            //         {
            //             if(grids.Value.location == grid[i].location)
            //             {
            //                 GameObject highlight = grids.Value._gameObject.transform.GetChild(0).gameObject;
            //                 highlight.SetActive(false);
            //             }
            //         }
            //     }
            // }
        }

        // public void SetActiveHighlight(Transform grid)
        // {
        //     GameObject highlight = grid.GetChild(0).gameObject;
        //     highlight.SetActive(true);
        // }

        // public void SetActiveHighlight(Transform grid)
        // {
        //     GameObject highlight = grid.GetChild(0).gameObject;
        //     highlight.SetActive(true);

        // }

        // public void SetDisableHighlight()
        // {
        //     GameObject highlight = grid.GetChild(0).gameObject;
        //     highlight.SetActive(false);
        // }
    }
}
