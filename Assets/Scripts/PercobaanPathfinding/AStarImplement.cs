using System;
using System.Collections;
using System.Collections.Generic;
using adefagia.Graph;
using adefagia.PercobaanPathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia
{
    public class AStarImplement : MonoBehaviour
    {

        public Vector2 startLocation;

        public Vector2 endLocation;

        public Material moveMaterial;

        private GridManager _gridManager;

        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
        }

        void Update()
        {
            if (GridManager.doneGenerateGrids)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    var aStar = new AStar();
                    var start = _gridManager.GetGridByLocation(startLocation);
                    var end = _gridManager.GetGridByLocation(endLocation);
                    
                    if(start.IsUnityNull() || end.IsUnityNull())
                    {
                        Debug.Log("Grid not found");
                        return;
                    }

                    aStar.Pathfinding(start, end);
                    aStar.DebugListGrid(aStar.reached);
                    
                    var path = aStar.Traversal(start, end);
                    aStar.DebugListGrid(path);

                    StartCoroutine(SetMaterialMove(path));
                }
            }
        }

        IEnumerator SetMaterialMove(List<Grid> path)
        {
            foreach (var grid in path)
            {
                grid.ChangeMaterial(moveMaterial);

                yield return new WaitForSeconds(0.1f);
            }
            
            foreach (var grid in path)
            {
                grid.ResetMaterial();

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
