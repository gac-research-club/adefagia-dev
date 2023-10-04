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
        ValuesManager<TileBase> valuesManager;
        PatternManager manager;

        bool equalWeights = false;

        void Start()
        {
            map = GetComponent<Map>();
            GenerateAllMap();
        }

        void GenerateAllMap()
        {
            InputReader reader = new InputReader(map.tilemapInput);
            var grid = reader.ReadInputToGrid();

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    Debug.Log($"Row : {row} Col : {col} tile name {grid[row][col].Value.name}");
                }
            }

            valuesManager = new ValuesManager<TileBase>(grid);

            StringBuilder builder = null;
            List<string> list = new List<string>();

            Debug.Log($"Row : {grid.Length} Col : {grid[0].Length}");

            for (int row = 0; row < grid.Length; row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < grid[0].Length; col++)
                {
                    builder.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
                    Debug.Log(
                        $"Row : {row} Col : {col} tile name {valuesManager.GetGridValuesIncludingOffset(col, row)}"
                    );
                }
                list.Add(builder.ToString());
            }

            list.Reverse();

            foreach (var item in list)
            {
                Debug.Log(item);
            }

            manager = new PatternManager(1);
            manager.ProcessGrid(valuesManager, equalWeights);

            // Get Possible Neighbours For Each Pattern
            for (int patternIndex = 0; patternIndex < 3; patternIndex++)
            {
                foreach (Direction dire in Enum.GetValues(typeof(Direction)))
                {
                    Debug.Log(
                        $"Pattern Index {patternIndex} --> {dire.ToString()}  : {string.Join(" ", manager.GetPossibleNeighboursForPatternInDirection(patternIndex, dire))}"
                    );
                }
                Debug.Log("----------------------------------------------");
            }
        }
    }
}
