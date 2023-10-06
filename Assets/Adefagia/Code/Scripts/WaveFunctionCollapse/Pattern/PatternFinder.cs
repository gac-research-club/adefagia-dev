using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public static class PatternFinder
    {
        public static PatternDataResults GetPatternDataFromGrid<T>(
            ValuesManager<T> valuesManager,
            int patternSize,
            bool equalWeights
        )
        {
            Dictionary<string, PatternData> patternHashcodeDictionary =
                new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary =
                new Dictionary<int, PatternData>();
            Vector2 sizeOfGrid = valuesManager.GetGridSize();

            int patternGridSizeX = 0;
            int patternGridSizeY = 0;
            int rowMin = -1,
                colMin = -1,
                rowMax = -1,
                colMax = -1;
            if (patternSize < 3)
            {
                /* Use Case

                    SizeGrid : 3 x 3
                    PatternSize : 1

                    PatternGridSizeX : 3 + 3 - 1 => 5
                    PatternGridSizeY : 3 + 3 - 1 => 5
                    RowMax = 5 - 1 => 4;
                    colMax = 5 - 1 => 4;
                    
                    Example Output  Estimation
                    [x][x][x][x][x]
                    [x][*][*][*][x]
                    [x][*][*][*][x]
                    [x][*][*][*][x]
                    [x][x][x][x][x]

                    ------------------------
                    
                    SizeGrid : 4 x 4
                    PatternSize : 2

                    PatternGridSizeX : 4 + 3 - 1 => 6
                    PatternGridSizeY : 4 + 3 - 1 => 6
                    RowMax = 6 - 1 => 5;
                    colMax = 6 - 1 => 5;
                    
                    Example Output  Estimation
                    [x][x][x][x][x][x]
                    [x][*][*][*][*][x]
                    [x][*][*][*][*][x]
                    [x][*][*][*][*][x]
                    [x][*][*][*][*][x]
                    [x][x][x][x][x][x]
                */

                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeY = (int)sizeOfGrid.y + 3 - patternSize;
                rowMax = patternGridSizeY - 1;
                colMax = patternGridSizeX - 1;
            }
            else
            {
                /* Use Case
                
                    SizeGrid : 5 x 5
                    PatternSize : 3

                    PatternGridSizeX : 5 + 3 - 1 => 7
                    PatternGridSizeY : 5 + 3 - 1 => 7
                    
                    RowMin = 1 - 3 => -2;
                    colMin = 1 - 3 => -2;

                    RowMax = 5 => 5;
                    colMax = 5 => 5;
                    
                    Example Output  Estimation
                    
                    [x][x][x][x][x][x][x]
                    [x][*][*][*][*][*][x]
                    [x][*][*][*][*][*][x]
                    [x][*][*][*][*][*][x]
                    [x][*][*][*][*][*][x]
                    [x][*][*][*][*][*][x]
                    [x][x][x][x][x][x][x]
        
                    
                    SizeGrid : 6 x 6
                    PatternSize : 3

                    PatternGridSizeX : 6 + 3 - 1 => 8
                    PatternGridSizeY : 6 + 3 - 1 => 8
                    
                    RowMin = 1 - 3 => -2;
                    colMin = 1 - 3 => -2;

                    RowMax = 5 => 6;
                    colMax = 5 => 6;
                    

                    [x][x][x][x][x][x][x][x]
                    [x][*][*][*][*][*][*][x]
                    [x][*][*][*][*][*][*][x]
                    [x][*][*][*][*][*][*][x]
                    [x][*][*][*][*][*][*][x]
                    [x][*][*][*][*][*][*][x]
                    [x][*][*][*][*][*][*][x]
                    [x][x][x][x][x][x][x][x]
                */

                patternGridSizeX = (int)sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = (int)sizeOfGrid.y + patternSize - 1;
                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;
                rowMax = (int)sizeOfGrid.y;
                colMax = (int)sizeOfGrid.x;
            }

            // PatternIndicesGrid is a Map Matrix of all the patterns that will be generated later,
            // for example with PatternSize 2x2 we have to know the position of each of these patterns.
            int[][] patternIndicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(
                patternGridSizeY,
                patternGridSizeX
            );

            // we loop with offset -1 / +1 to get patterns. At the same time we have to account for pattern size.
            // If pattern is of size 2 we will be reaching x+1 and y+1 to check all 4 values. Need visual aid.

            int totalFrequency = 0,
                patternIndex = 0;
            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = colMin; col < colMax; col++)
                {
                    /*
                        Get Grid Matrix from ValuesManager
                        that return Pattern at Col,Row based patternSize
                    
                        Return Matrix Grid Pattern Of int[][]

                        -1, -1 -- patternSize = 2

                        [0, -1][ 0,0] => [0][0]
                        [-1,-1][-1,0]    [x][0]
                    */

                    int[][] gridValues = valuesManager.GetPatternValuesFromGridAt(
                        col,
                        row,
                        patternSize
                    );

                    // Calculate HashCode ??
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if (patternHashcodeDictionary.ContainsKey(hashValue) == false)
                    {
                        // The Pattern made of GridValues and have a patternIndex for index
                        // HashValue is used to make each pattern unique for each type of the same type
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;

                        AddNewPattern(
                            patternHashcodeDictionary,
                            patternIndexDictionary,
                            hashValue,
                            pattern
                        );
                    }
                    else
                    {
                        if (equalWeights == false)
                        {
                            // Increase Frequency of the same type Pattern
                            patternIndexDictionary[
                                patternHashcodeDictionary[hashValue].Pattern.Index
                            ].AddToFrequency();
                        }
                    }

                    totalFrequency++;

                    //
                    if (patternSize < 3)
                    {
                        /* Case Example
                            SizeGrid : 4 x 4
                            PatternSize : 2
                            
                            RowMax = 6 - 1 => 5;
                            colMax = 6 - 1 => 5;

                            Example Output  Estimation
                            
                            [x][x][x][x][x][x]
                            [x][*][*][*][*][x]
                            [x][*][*][*][*][x]
                            [x][*][*][*][*][x]
                            [x][v][v][*][*][x]
                            [x][x][x][x][x][x]

                            -1, -1
                            PatternIndicesGrid[0][0] ==> PatternIndex [0]

                            -1, 0
                            PatternIndicesGrid[0][0] ==> PatternIndex [1]
                        */
                        patternIndicesGrid[row + 1][col + 1] = patternHashcodeDictionary[hashValue]
                            .Pattern
                            .Index;
                    }
                    else
                    {
                        patternIndicesGrid[row + patternSize - 1][col + patternSize - 1] =
                            patternHashcodeDictionary[hashValue].Pattern.Index;
                    }
                }
            }

            // Calculate All Frequency of Each Pattern
            CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);

            return new PatternDataResults(patternIndicesGrid, patternIndexDictionary);
        }

        private static void CalculateRelativeFrequency(
            Dictionary<int, PatternData> patternIndexDictionary,
            int totalFrequency
        )
        {
            foreach (var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }

        private static void AddNewPattern(
            Dictionary<string, PatternData> patternHashcodeDictionary,
            Dictionary<int, PatternData> patternIndexDictionary,
            string hashValue,
            Pattern pattern
        )
        {
            PatternData data = new PatternData(pattern);
            patternHashcodeDictionary.Add(hashValue, data);
            patternIndexDictionary.Add(pattern.Index, data);
        }

        public static Dictionary<int, PatternNeighbours> FindPossibleNeighboursForAllPatterns(
            IFindNeighbourStrategy patternFinder,
            PatternDataResults patternDataResults
        )
        {
            return patternFinder.FindNeighbours(patternDataResults);
        }

        public static PatternNeighbours CheckNeighboursInEachDirection(
            int x,
            int y,
            PatternDataResults patternDataResults
        )
        {
            PatternNeighbours neighbours = new PatternNeighbours();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResults.GetNeighbourInDirection(x, y, dir);
                if (possiblePatternIndex >= 0)
                {
                    neighbours.AddPatternToDictionary(dir, possiblePatternIndex);
                }
            }

            return neighbours;
        }

        public static void AddNeighboursToDictionary(
            Dictionary<int, PatternNeighbours> dictionary,
            int patternIndex,
            PatternNeighbours neighbours
        )
        {
            if (dictionary.ContainsKey(patternIndex) == false)
            {
                dictionary.Add(patternIndex, neighbours);
            }
            dictionary[patternIndex].AddNeighbours(neighbours);
        }
    }
}
