using System;
using System.Text;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.Robot
{
    public class RobotMovement : MonoBehaviour
    {
        
        public Grid gridBerdiri;

        // Update is called once per frame
        void Update()
        {
            if (!Spawner.doneSpawn) return;

            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeGridBerdiri(gridBerdiri.Right);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ChangeGridBerdiri(gridBerdiri.Up);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ChangeGridBerdiri(gridBerdiri.Left);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ChangeGridBerdiri(gridBerdiri.Down);
            }
        
            MoveToGrid(gridBerdiri);
        }

        void ChangeGridBerdiri(Grid grid)
        {
            if (GridManager.IsGridEmpty(grid)) return;
        
            gridBerdiri = grid;
        }

        void MoveToGrid(Grid grid)
        {
            transform.position = grid.GetLocation(y: 0.5f);
        }
        
        private void OnGUI()
        {
        
            if (!GridManager.doneGenerateGrids) return;
            if (gridBerdiri.IsUnityNull()) return;
        
            var textLeft = $"Berdiri {gridBerdiri.location.x}, {gridBerdiri.location.y}"; 
            GUI.Box (new Rect (10,Screen.height-10-50,100,50), textLeft);
        
            var text = "";
        
            StringBuilder textTopRight = new StringBuilder();
            textTopRight.Append("Neighboors: ");
            DebugNeighborPosition(textTopRight, "Kanan", gridBerdiri.Right);
            DebugNeighborPosition(textTopRight, "Atas", gridBerdiri.Up);
            DebugNeighborPosition(textTopRight, "Kiri", gridBerdiri.Left);
            DebugNeighborPosition(textTopRight, "Bawah", gridBerdiri.Down);
        
            text = $"Node {gridBerdiri.index} = ({gridBerdiri.location.x},{gridBerdiri.location.y})";

            // Make a background box
            GUI.Box(new Rect(10, 10, 100, 50), text);
            GUI.Box (new Rect (Screen.width - 10 - 100,10,100,100), textTopRight.ToString());
        
        }

        private void DebugNeighborPosition(StringBuilder sb, string position, Graph.Grid grid)
        {
            try
            {
                sb.Append($"\n{position}: {grid.state}");
            }
            catch (NullReferenceException)
            {
                sb.Append($"\n{position}: Null");
            }
        }
    }
}