using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia.Graph
{
    public class GridManager : MonoBehaviour
    {
        public GameObject ground;

        public int xSize, ySize;
        
        // For Debugging
        public int index;
        public Vector2 nodeLoc;
        // ---

        public GameObject emptyPrefab, borderPrefab;

        public string mapName = "Map";
    
        public static bool doneGenerateGrids;

        // Set of nodes
        private Dictionary<Vector2, Grid> _allGrid;
        public Dictionary<Transform, Grid> _allGridTransform;

        private void Awake()
        {
            _allGrid = new Dictionary<Vector2, Grid>();
            _allGridTransform = new Dictionary<Transform, Grid>();

            GenerateGrids();
            
            SetNeighbors();
            
            doneGenerateGrids = true;

            SetGridStatesByMap();
            // SetGridStatesAllGround();
        
            InstantiateGameObjects();
            
            GenerateBorder();
        }

        void GenerateGrids()
        {
            // Set all Grid by (x,y)
            for (int i = 0, y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    // Add every node
                    Grid grid = new Grid(i, new Vector2(x, y));
                
                    // Add into Dictionary
                    _allGrid[grid.location] = grid;
                }
            }
        }

        void SetNeighbors()
        {
            // Set all grid neighbors
            foreach (var grid in _allGrid.Values)
            {
                // Add neighbor
                // Add 4 : right, up, left, down
                Vector2[] dirs = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
                grid.neighbors = new Grid[dirs.Length];
                
                for (var i=0; i<dirs.Length; i++)
                {
                    grid.neighbors[i] = GetGridByLocation(grid.location + dirs[i]);
                }
            }
            
        }

        void SetGridStatesByMap()
        {
            var map = ReadFile("Assets/Resources/"+ mapName +".txt");
            
            // If Map Empty generate all ground
            if (map.Length == 0)
            {
                SetGridStatesAllGround();
                return;
            }

            // State of grid:
            // [-] empty
            // [o] ground
            
            // From yUp -> yDown
            for (int i = 0, y = ySize-1; y >= 0; y--)
            {
                // From xLeft -> xRight
                for (int x = 0; x < xSize; x++, i++)
                {
                    try
                    {
                        // Set State each grid
                        var grid = GetGridByLocation(new Vector2(x, y));
                        SetStateByChar(map[i], grid);
                    }
                    // catch error if Map char have not same size as AllGrid
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }
                }
            }
        }
        
        void SetGridStatesAllGround()
        {
            // Default State
            foreach (var grid in _allGrid.Values)
            {
                grid.state = State.Ground;
            }
        }

        private void SetStateByChar(char character, Grid grid)
        {
            // Set State by char
            switch (character)
            {
                case '-':
                    grid.state = State.Empty;
                    break;
                    
                case 'o':
                    grid.state = State.Ground;
                    break;
            }
        }

        private void InstantiateGameObjects()
        {
            foreach (var grid in _allGrid.Values)
            {
                // Instantiate gameObject according to State
                switch (grid.state)
                {
                    case State.Ground:
                        ground = Instantiate(emptyPrefab, grid.GetLocation(), emptyPrefab.transform.rotation, transform);
                        ground.name = "Ground " + grid.index;
                        
                        grid.SetGameObject(ground);
                        
                        // Add to Dictionary
                        _allGridTransform[ground.transform] = grid;
                        
                        break;
                }
            }
        }

        void GenerateBorder()
        {
            for (int y = -1; y < ySize+1; y++)
            {
                for (int x = -1; x < xSize+1; x++)
                {
                    if ((x < 0 || y < 0 ) || (x > xSize-1 || y > xSize-1))
                    {
                        Instantiate(borderPrefab, new Vector3(x,0,y), Quaternion.identity, transform);
                    }
                }
            }
        }
        
        public static bool IsGridEmpty(Grid grid)
        {
            if (grid.IsUnityNull()) return true;
            return grid.IsEmpty();
        }

        public Grid GetGridByLocation(Vector2 loc)
        {
            return _allGrid.TryGetValue(loc, out Grid node) ? node : null;
        }

        public Grid GetGridByTransform(Transform iTransform)
        {
            return _allGridTransform.TryGetValue(iTransform, out Grid node) ? node : null;
        }

        private void OnDrawGizmos()
        {
            if (_allGrid.IsUnityNull()) return;
            if (!doneGenerateGrids) return;
    
            foreach (var grid in _allGrid)
            {
                if (!grid.Value.IsUnityNull())
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(grid.Value.GetLocation(), 0.1f);
                
                    Gizmos.color = Color.blue;
                    foreach (var neighbor in grid.Value.neighbors)
                    {
                        if (!neighbor.IsUnityNull())
                        {
                            Gizmos.DrawLine(neighbor.GetLocation(), grid.Value.GetLocation());
                        }
                    }
                
                }
            }
        }

        string ReadFile(string pathFile)
        {
            var map = string.Empty;
            
            try
            {
                // read from file
                StreamReader reader = new StreamReader(pathFile);
                map = reader.ReadToEnd();

            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("File Map not found", this);
            }

            // Serialize string
            // Debug.Log(map.Length);
            return CleanInput(map);
        }
        
        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                    RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException) {
                return String.Empty;
            }
        }
    
    }
}

