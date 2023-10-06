using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.WaveFunctionCollapse
{
    public interface IFindNeighbourStrategy
    {
        Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternDataResults);
    }
}
