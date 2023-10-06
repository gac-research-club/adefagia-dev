using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Adefagia.WaveFunctionCollapse
{
    public interface IOutputCreator<T>
    {
        // T OutputImage { get; }
        int[][] CreateOutput(PatternManager manager, int[][] outputvalues, int width, int height);
    }
}
