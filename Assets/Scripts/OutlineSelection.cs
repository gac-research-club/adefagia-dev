using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace adefagia
{
    public class OutlineSelection : MonoBehaviour
    {
        public Vector2 lastLoc;

        private Transform highlight;
        private Transform selection;
        private RaycastHit raycastHit;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private Color color;
        
        [SerializeField] private PlayerAction.MoveAction moveAction;
        [SerializeField] private PlayerAction.AttackHighlight attackHighlight;
        [SerializeField] private Graph.GridManager gridManager;
        [SerializeField] private GameObject actionButton;

        private void Start()
        {
            gameInput.OnInteractAction += GameInput_OnInteractAction;
        }

        private void GameInput_OnInteractAction(object sender, System.EventArgs e)
        {
            // Selection
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
                actionButton.SetActive(true);

                // ShowHighlight(highlightPattern.movementPattern);
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject() && selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    actionButton.SetActive(false);

                    moveAction.MoveButtonOnDisable();
                    attackHighlight.AttackButtonOnDisable();
                }
            }
        }

        void Update()
        {
            Highlight();
        }

        private void ShowHighlight(Vector2[] pattern)
        {
            // Add 8 BasicMovement Pattern : right, up, left, down, + 4 diagonal quads
            Vector2[] dirsMovement = pattern;
            
            var grid = gridManager.GetGridByLocation(lastLoc);
            grid.movementGrid = new Graph.Grid[dirsMovement.Length];

            for (var i=0; i<dirsMovement.Length; i++)
            {
                grid.movementGrid[i] = gridManager.GetGridByLocation(lastLoc + dirsMovement[i]);
                if (Graph.GridManager.IsGridEmpty(grid.movementGrid[i]))
                {
                    grid.movementGrid[i] = null;
                } else 
                {
                    grid.movementGrid[i] = grid.movementGrid[i];
                }

                if(grid.movementGrid[i] != null)
                {
                    foreach (var grids in gridManager._allGridTransform)
                    {
                        if(grids.Value.location == grid.movementGrid[i].location)
                        {
                                SetHighlightMovement(grids.Value._gameObject.transform, selection);
                        }
                    }
                }
            }
        }

        public void SetHighlightMovement(Transform grid, Transform selection)
        {
            GameObject highlight = grid.GetChild(0).gameObject;

            if(!selection.IsUnityNull())
            {
                highlight.SetActive(true);
            } else 
            {
                highlight.SetActive(false);
            }
        }

        private void Highlight()
        {
            // Highlight
            if (highlight != null)
            {
                highlight.gameObject.GetComponent<Outline>().enabled = false;
                highlight = null;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
            {
                highlight = raycastHit.transform;
                if (highlight.CompareTag("Selectable") && highlight != selection)
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        highlight.gameObject.GetComponent<Outline>().OutlineColor = color;
                        highlight.gameObject.GetComponent<Outline>().OutlineWidth = 8.0f;
                    }
                }
                else
                {
                    highlight = null;
                }
            }
        }
    }
}
