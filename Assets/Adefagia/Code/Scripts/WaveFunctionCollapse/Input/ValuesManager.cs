using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class ValuesManager<T>
    {
        int[][] _grid;

        Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0;

        public ValuesManager(IValue<T>[][] gridOfValue)
        {
            CreateGridOfIndices(gridOfValue);
        }

        private void CreateGridOfIndices(IValue<T>[][] gridOfValues)
        {
            _grid = MyCollectionExtension.CreateJaggedArray<int[][]>(
                gridOfValues.Length,
                gridOfValues[0].Length
            );

            for (int row = 0; row < gridOfValues[0].Length; row++)
            {
                for (int col = 0; col < gridOfValues[0].Length; col++)
                {
                    SetIndexToGridPosition(gridOfValues, row, col);
                }
            }
        }

        private void SetIndexToGridPosition(IValue<T>[][] gridOfValue, int row, int col)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValue[row][col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(
                    x => x.Value.Equals(gridOfValue[row][col])
                );
                _grid[row][col] = key.Key;
            }
            else
            {
                _grid[row][col] = index;
                valueIndexDictionary.Add(_grid[row][col], gridOfValue[row][col]);
                index++;
            }
        }

        public int GetGridValue(int row, int col)
        {
            if (row >= _grid[0].Length || col >= _grid.Length || row < 0 || col < 0)
            {
                throw new System.IndexOutOfRangeException(
                    $"Grid doesn't contain x : {row} y : {col}"
                );
            }
            return _grid[col][row];
        }

        public IValue<T> GetValueFromIndex(int index)
        {
            if (valueIndexDictionary.ContainsKey(index))
            {
                return valueIndexDictionary[index];
            }

            throw new System.Exception($"No Index {index} in valueDictionary");
        }

        public int GetGridValuesIncludingOffset(int x, int y)
        {
            int yMax = _grid.Length;
            int xMax = _grid[0].Length;
            if (x < 0 && y < 0)
            {
                return GetGridValue(xMax + x, yMax + y);
            }
            if (x < 0 && y >= yMax)
            {
                return GetGridValue(xMax + x, y - yMax);
            }
            if (x >= xMax && y < 0)
            {
                return GetGridValue(x - xMax, yMax + y);
            }
            if (x >= xMax && y >= yMax)
            {
                return GetGridValue(x - xMax, y - yMax);
            }

            if (x < 0)
            {
                return GetGridValue(xMax + x, y);
            }
            if (x >= xMax)
            {
                return GetGridValue(x - xMax, y);
            }
            if (y < 0)
            {
                return GetGridValue(x, yMax + y);
            }

            if (y >= yMax)
            {
                return GetGridValue(x, y - yMax);
            }

            /*
                Use Case

                |0 |1 |1 |     |2,1 |2,1 |2,2 |
                |0 |1 |1 | ==> |1,0 |1,1 |1,2 |
                |0 |0 |0 |     |0,0 |0,1 |0,2 |
                
                PatternSize : 3 x 3
                Case : -1 , -1
                Process : GetGridValue((3 + -1), (3 + -1))
                Result : 2, 2
                Value : 1
            */

            return GetGridValue(x, y);
        }

        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[][] arrayToReturn = MyCollectionExtension.CreateJaggedArray<int[][]>(
                patternSize,
                patternSize
            );

            /*
                Case Example
                -1, -1 -- PatternSize = 2

                [-1, 0][ 0,0]    [0][0]
                [-1,-1][-1,0] => [1][0]

            */
            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    arrayToReturn[row][col] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }

            return arrayToReturn;
        }

        public Vector2 GetGridSize()
        {
            if (_grid == null)
            {
                return Vector2.zero;
            }
            return new Vector2(_grid[0].Length, _grid.Length);
        }
    }
}
