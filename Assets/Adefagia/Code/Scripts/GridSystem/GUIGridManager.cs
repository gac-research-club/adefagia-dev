using System;
using Adefagia.GridSystem;
using UnityEngine;

namespace Adefagia.UI
{
    public class GUIGridManager : MonoBehaviour
    {
        private GridManager _gridManager;
        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
        }

        private void OnGUIBackup()
        {
            var text = "";
            try
            {
                // Show what grid is hover
                var grid = _gridManager.GetGrid();
                text = $"Grid ({grid.X}, {grid.Y})\n" +
                       $"{grid.Status.ToString()}";
            }
            catch (NullReferenceException)
            {
                text = "Null";
            }
            GUI.Box (new Rect (0,0,100,50), text);
        }
    }

}