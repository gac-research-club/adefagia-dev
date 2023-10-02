using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class TileContainer
    {
        public TileBase Tile { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public TileContainer(TileBase tile, int x, int y)
        {
            this.Tile = tile;
            this.X = x;
            this.Y = y;
        }
    }
}
