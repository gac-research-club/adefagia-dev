using System;
using Adefagia;
using Adefagia.GridSystem;
using Adefagia.SelectObject;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

public class GUIGridManager : MonoBehaviour
{
    private GridManager _gridManager;
    private void Start()
    {
        _gridManager = GameManager.instance.gridManager;
    }

    private void OnGUI()
    {
        var text = "";
        try
        {
            // Show what grid is hover
            var grid = _gridManager.GetGrid();
            text = $"Grid ({grid.X}, {grid.Y})";
        }
        catch (NullReferenceException)
        {
            text = "Null";
        }
        GUI.Box (new Rect (0,0,100,50), text);
    }
}
