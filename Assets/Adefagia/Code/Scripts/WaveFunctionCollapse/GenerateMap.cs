using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Adefagia.MapBase;

namespace Adefagia.WaveFunctionCollapse
{
    public class GenerateMap : MonoBehaviour
    {
        public Map map;

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
        }
    }
}
