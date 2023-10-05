using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Adefagia.MapBase;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class GenerateMap : MonoBehaviour
    {
        public Map map;
        private int[][] _positionObstacle;
        public int[][] PositionObstacle => _positionObstacle;
        ValuesManager<TileBase> valuesManager;
        PatternManager manager;

        public int outputSize = 10;
        public int patternSize = 2;
        public int testingLoop = 10000;
        public bool equalWeights = false;

        [Header("Select Strategy : 1. Size Default, 2. Size More")]
        public int strategyNumber = 2;
        List<string> strategyName = new List<string>();

        CoreMap core;

        void Awake()
        {
            strategyName.Add("neighboursstrategysize1default");
            strategyName.Add("neighboursstrategysize2andmore");

            map = GetComponent<Map>();
            GenerateAllTile();
        }

        void GenerateAllTile()
        {
            InputReader reader = new InputReader(map.tilemapInput);
            var grid = reader.ReadInputToGrid();

            valuesManager = new ValuesManager<TileBase>(grid);

            manager = new PatternManager(patternSize);
            manager.ProcessGrid(valuesManager, equalWeights, strategyName[strategyNumber - 1]);

            core = new CoreMap(outputSize, outputSize, testingLoop, manager);
            CreateMap();
        }

        public void CreateMap()
        {
            TileMapOutput output = new TileMapOutput(valuesManager);
            var result = core.CreateOutputGrid();

            StringBuilder builder = null;
            List<string> list = new List<string>();

            _positionObstacle = output.CreateOutput(manager, result, outputSize, outputSize);
        }
    }
}



/* Here For Testing */

// StringBuilder builder = null;
// List<string> list = new List<string>();

// Debug.Log($"Row : {grid.Length} Col : {grid[0].Length}");

// for (int row = 0; row < grid.Length; row++)
// {
//     builder = new StringBuilder();
//     for (int col = 0; col < grid[0].Length; col++)
//     {
//         builder.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
//         Debug.Log(
//             $"Row : {row} Col : {col} tile name {valuesManager.GetGridValuesIncludingOffset(col, row)}"
//         );
//     }
//     list.Add(builder.ToString());
// }

// list.Reverse();

// foreach (var item in list)
// {
//     Debug.Log(item);
// }


// for (int row = 0; row < grid.Length; row++)
// {
//     for (int col = 0; col < grid[0].Length; col++)
//     {
//         Debug.Log($"Row : {row} Col : {col} tile name {grid[row][col].Value.name}");
//     }
// }

// Get Possible Neighbours For Each Pattern
// for (int patternIndex = 0; patternIndex < 3; patternIndex++)
// {
//     foreach (Direction dire in Enum.GetValues(typeof(Direction)))
//     {
//         Debug.Log(
//             $"Pattern Index {patternIndex} --> {dire.ToString()}  : {string.Join(" ", manager.GetPossibleNeighboursForPatternInDirection(patternIndex, dire))}"
//         );
//     }
//     Debug.Log("----------------------------------------------");
// }



// for (int row = 0; row < result.Length; row++)
// {
//     builder = new StringBuilder();
//     for (int col = 0; col < result[0].Length; col++)
//     {
//         builder.Append(result[col][row] + " ");
//     }
//     list.Add(builder.ToString());
// }

// foreach (var item in list)
// {
//     Debug.Log(item);
// }
