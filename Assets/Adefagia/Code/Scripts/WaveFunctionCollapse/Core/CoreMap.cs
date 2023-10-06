using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.WaveFunctionCollapse
{
    public class CoreMap
    {
        OutputGrid outputGrid;
        PatternManager patternManager;
        private int maxIterations = 0;

        public CoreMap(
            int outputWidth,
            int outputHeight,
            int maxIterations,
            PatternManager patternManage
        )
        {
            this.outputGrid = new OutputGrid(
                outputWidth,
                outputHeight,
                patternManage.GetNumberOfPatterns()
            );
            this.patternManager = patternManage;
            this.maxIterations = maxIterations;
        }

        public int[][] CreateOutputGrid()
        {
            int iteration = 0;
            while (iteration < this.maxIterations)
            {
                CoreSolver solver = new CoreSolver(this.outputGrid, this.patternManager);
                int innerIteration = 1000;
                while (!solver.CheckForConflicts() && !solver.CheckIfSolved())
                {
                    Vector2Int position = solver.GetLowestEntropyCell();
                    solver.CollapseCell(position);
                    solver.Propagate();
                    innerIteration--;
                    if (innerIteration <= 0)
                    {
                        Debug.Log("Propagation is taking too long");
                        return new int[0][];
                    }
                }
                if (solver.CheckForConflicts())
                {
                    Debug.Log("\n Conflict Occured. Iteration: " + iteration);
                    iteration++;
                    outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(this.outputGrid, this.patternManager);
                }
                else
                {
                    Debug.Log("Solved on: " + iteration);
                    this.outputGrid.PrintResultsToConsol();
                    break;
                }
            }
            if (iteration >= this.maxIterations)
            {
                Debug.Log("Could not solve the tilemap");
            }
            return outputGrid.GetSolvedOutputGrid();
        }
    }
}
