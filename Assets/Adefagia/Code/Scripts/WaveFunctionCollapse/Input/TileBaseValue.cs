using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Adefagia.Helper;

namespace Adefagia.WaveFunctionCollapse
{
    public class TileBaseValue : IValue<TileBase>
    {
        private TileBase tileBase;

        public TileBase Value => this.tileBase;

        public TileBaseValue(TileBase tileBase)
        {
            this.tileBase = tileBase;
        }

        public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
        {
            return x == y;
        }

        public bool Equals(IValue<TileBase> other)
        {
            return other.Value == this.Value;
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.tileBase.GetHashCode();
        }
    }
}
