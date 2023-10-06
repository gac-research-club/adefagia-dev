using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;

namespace Adefagia.WaveFunctionCollapse
{
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        // private Tilemap outputImage;
        private ValuesManager<TileBase> valueManager;

        // public Tilemap OutputImage => outputImage;

        public TileMapOutput(ValuesManager<TileBase> valueManager)
        {
            // this.outputImage = outputImage;
            this.valueManager = valueManager;
        }

        public int[][] CreateOutput(
            PatternManager manager,
            int[][] outputvalues,
            int width,
            int height
        )
        {
            if (outputvalues.Length == 0)
            {
                return new int[0][];
            }
            // this.outputImage.ClearAllTiles();

            int[][] valueGrid;
            valueGrid = manager.ConvertPatternToValues<TileBase>(outputvalues);

            return valueGrid;
        }
    }
}
