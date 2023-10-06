using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.WaveFunctionCollapse
{
    public class NeighboursStrategySize1Default : IFindNeighbourStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(
            PatternDataResults patternDataResults
        )
        {
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();
            FindNeighboursForEachPattern(patternDataResults, result);
            return result;
        }

        private void FindNeighboursForEachPattern(
            PatternDataResults patternFinderResult,
            Dictionary<int, PatternNeighbours> result
        )
        {
            /* Case Example
             
             [0][0][0][0][0]
             [0][0][1][0][0]
             [0][1][1][1][0]
             [0][0][1][0][0]
             [0][0][1][0][0]
             
                 0,0
                 UP -> 1
                 DOWN -> 0
                 RIGHT -> 1
                 LEFT -> 0
 
             Return : <Direction, HashSet<int>> [(UP, 1) (DOWN, 0) (RIGHT, 1) (LEFT, 0)]
             */

            for (int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                for (int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    /*
                        Example Case:

                        X,Y ==> (0,0)
                        Return : <Direction, HashSet<int>> [(UP, 1) (DOWN, 0) (RIGHT, 1) (LEFT, 0)]

                    */
                    PatternNeighbours neighbours = PatternFinder.CheckNeighboursInEachDirection(
                        col,
                        row,
                        patternFinderResult
                    );

                    /*
                        Example Case :

                        PatternIndex : GetIndexAt(col, row)
                        
                        Dictionary<int, PatternNeighbours> [PatternIndex] => <Direction, HashSet<int>>
                        
                    */
                    PatternFinder.AddNeighboursToDictionary(
                        result,
                        patternFinderResult.GetIndexAt(col, row),
                        neighbours
                    );
                }
            }
        }
    }
}
