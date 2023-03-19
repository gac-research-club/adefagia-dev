using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using UnityEngine;

public class GUIGridManager : MonoBehaviour
{
    private GridManager _gridManager;
    private void Start()
    {
        _gridManager = GetComponent<GridManager>();
    }

    private void OnGUI()
    {
        var text = "";
        try
        {
            var grid = _gridManager.GridHover.Grid;
            text = $"Grid ({grid.X}, {grid.Y})";
        }
        catch (NullReferenceException)
        {
            text = "Null";
        }
        GUI.Box (new Rect (0,0,100,50), text);
    }
}
