using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.WaveFunctionCollapse
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}
