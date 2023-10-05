using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;

        int _patternSize = -1;

        IFindNeighbourStrategy strategy;

        // Pattern Size parameter
        public PatternManager(int patternSize)
        {
            _patternSize = patternSize;
        }

        public void ProcessGrid<T>(
            ValuesManager<T> valuesManager,
            bool equalWeights,
            string strategyName = "neighboursstrategysize2andmore"
        )
        {
            /*
                NeighbourStrategyFactory load all existing
                strategies with the IFindNeighbors interface

                Currently Just Two
                - neighboursstrategysize1default
                - neighboursstrategysize2andmore,
                
            */
            NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
            strategy = strategyFactory.CreateInstance(
                strategyName == null ? _patternSize + "" : strategyName
            );

            /*
                Do Create Pattern
                ValuesManager ==> initialize the indexed grid
                strategy ==> what strategy we use for Adjency Rules
                equalWeights ==> ???
            */
            CreatePatterns(valuesManager, strategy, equalWeights);
        }

        private void CreatePatterns<T>(
            ValuesManager<T> valuesManager,
            IFindNeighbourStrategy strategy,
            bool equalWeights
        )
        {
            // A combination object of PatternIndicesGrid (Matrix Map where the Pattern is located)
            // and also PatternIndexDictionary (Dictionary Index of PatternData)
            PatternDataResults patternFinderResult = PatternFinder.GetPatternDataFromGrid(
                valuesManager,
                _patternSize,
                equalWeights
            );

            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;
            GetPatternNeighbours(patternFinderResult, strategy);
        }

        private void GetPatternNeighbours(
            PatternDataResults patternFinderResult,
            IFindNeighbourStrategy strategy
        )
        {
            patternPossibleNeighboursDictionary =
                PatternFinder.FindPossibleNeighboursForAllPatterns(strategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighboursForPatternInDirection(
            int patternIndex,
            Direction dir
        )
        {
            return patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }

        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }

        public int GetNumberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }

        internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth = outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;
            int valueGridWidth = patternOutputWidth + (_patternSize - 1);
            int valueGridHeight = patternOutputHeight + (_patternSize - 1);
            int[][] valueGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(
                valueGridHeight,
                valueGridWidth
            );

            for (int row = 0; row < patternOutputHeight; row++)
            {
                for (int col = 0; col < patternOutputWidth; col++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;
                    GetPatternValues(
                        patternOutputWidth,
                        patternOutputHeight,
                        valueGrid,
                        row,
                        col,
                        pattern
                    );
                }
            }

            return valueGrid;
        }

        private void GetPatternValues(
            int patternOutputWidth,
            int patternOutputHeight,
            int[][] valueGrid,
            int row,
            int col,
            Pattern pattern
        )
        {
            if (row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for (int row_1 = 0; row_1 < _patternSize; row_1++)
                {
                    for (int col_1 = 0; col_1 < _patternSize; col_1++)
                    {
                        valueGrid[row + row_1][col + col_1] = pattern.GetGridValue(col_1, row_1);
                    }
                }
            }
            else if (row == patternOutputHeight - 1)
            {
                for (int row_1 = 0; row_1 < _patternSize; row_1++)
                {
                    valueGrid[row + row_1][col] = pattern.GetGridValue(0, row_1);
                }
            }
            else if (col == patternOutputWidth - 1)
            {
                for (int col_1 = 0; col_1 < _patternSize; col_1++)
                {
                    valueGrid[row][col + col_1] = pattern.GetGridValue(col_1, 0);
                }
            }
            else
            {
                valueGrid[row][col] = pattern.GetGridValue(0, 0);
            }
        }
    }
}
