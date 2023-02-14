using System;
using System.Collections;
using System.Collections.Generic;
using Playground.Grid;
using Unity.VisualScripting;
using UnityEngine;
using State = Playground.Grid.State;

namespace Playground
{
    public class RobotMovement : MonoBehaviour
    {
        
        public Grid.Grid gridBerdiri;

        // Update is called once per frame
        void Update()
        {
            if (!Spawner.doneSpawn) return;

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (gridBerdiri.Right.IsUnityNull()) return;
                if (gridBerdiri.Right.state.Equals(State.Empty)) return;
            
                gridBerdiri = gridBerdiri.Right;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (gridBerdiri.Up.IsUnityNull()) return;
                if (gridBerdiri.Up.state.Equals(State.Empty)) return;
            
                gridBerdiri = gridBerdiri.Up;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (gridBerdiri.Left.IsUnityNull()) return;
                if (gridBerdiri.Left.state.Equals(State.Empty)) return;
            
                gridBerdiri = gridBerdiri.Left;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (gridBerdiri.Down.IsUnityNull()) return;
                if (gridBerdiri.Down.state.Equals(State.Empty)) return;
            
                gridBerdiri = gridBerdiri.Down;
            }

            transform.position = gridBerdiri.GetLocation();
        }
        
        private void OnGUI()
        {
        
            if (!GridManager.doneGenerateNodes) return;
            if (gridBerdiri.IsUnityNull()) return;
        
            var textLeft = $"Berdiri {gridBerdiri.location.x}, {gridBerdiri.location.y}"; 
            GUI.Box (new Rect (10,Screen.height-10-50,100,50), textLeft);
        
            var text = "";
            var textTopRight = "Neighboors: ";
        
            text = $"Node {gridBerdiri.index} = ({gridBerdiri.location.x},{gridBerdiri.location.y})";
            foreach (var neighboor in gridBerdiri.neighboors)
            {
                try
                {
                    // textTopRight += "\n- " + neighboor.index;
                    textTopRight += "\n- " + neighboor.state;
                }
                catch (NullReferenceException)
                {
                    textTopRight += "\n- Null";
                }
            }
        
            // Make a background box
            GUI.Box(new Rect(10, 10, 100, 50), text);
            GUI.Box (new Rect (Screen.width - 10 - 100,10,100,100), textTopRight);
        
        }
    }
    
    
}
