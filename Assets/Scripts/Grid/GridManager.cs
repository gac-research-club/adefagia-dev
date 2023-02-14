using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        // Singleton
        public static GridManager instance;
        
        public int xSize, ySize;
        
        // For Debugging
        public int index;
        public Vector2 nodeLoc;
        // ---

        public GameObject emptyPrefab, borderPrefab;
    
        public static bool doneGenerateGrids;

        // Set of nodes
        private Dictionary<Vector2, Grid> _allGrid;
        private Dictionary<Transform, Grid> _allGridTransform;

        private void Awake()
        {
            _allGrid = new Dictionary<Vector2, Grid>();
            _allGridTransform = new Dictionary<Transform, Grid>();
            
            // StartCoroutine(Generate());
            
            GenerateGrids();
            
            SetNeighbors();
            
            doneGenerateGrids = true;
            
            Singleton();
        
            SetGridStatesByMap();
            // SetGridStatesAllGround();
        
            InstantiateGameObjects();
            
            GenerateBorder();
            
            // Debug.Log(all_nodes.Length);
        }

        void Singleton()
        {
            if (instance.IsUnityNull())
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            // DontDestroyOnLoad(instance);
        }

        private void Update()
        {
        }

        void GenerateGrids()
        {
            // WaitForSeconds wait = new WaitForSeconds(0.01f);

            // Set of nodes

            for (int i = 0, y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    var location = new Vector2(x, y);

                    // Add node
                    Grid grid = new Grid(i, location);
                
                    _allGrid.Add(location, grid);
                    
                    // yield return wait;
                }
            }
        }

        void SetNeighbors()
        {
            for (int  y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    var location = new Vector2(x, y);

                    // Add neighbor
                    var grid = GetGridByLocation(location);

                    // Add 4 : right, up, left, down
                    Vector2[] dirs = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
                    grid.neighbors = new Grid[dirs.Length];

                    for (var i=0; i<dirs.Length; i++)
                    {
                        grid.neighbors[i] = GetGridByLocation(location + dirs[i]);
                    }
                }
            }
        }

        void SetGridStatesByMap()
        {
            var map = ReadFile("Assets/Resources/Map2.txt");

            // Set state of grid
            // [-] empty
            // [o] ground
            for (int i = 0, y = ySize-1; y >= 0; y--)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    try
                    {
                        SetStateByChar(map[i], new Vector2(x,y));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }
                }
            }
        }
        
        void SetGridStatesAllGround()
        {
            foreach (var grid in _allGrid)
            {
                grid.Value.state = State.Ground;
            }
        }

        private void SetStateByChar(char character, Vector2 location)
        {
            var grid = GetGridByLocation(location);
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

        void InstantiateGameObjects()
        {
            foreach (var grid in _allGrid)
            {
                switch (grid.Value.state)
                {
                    case State.Ground:
                        var cube = Instantiate(emptyPrefab, grid.Value.GetLocation(), emptyPrefab.transform.rotation, transform);
                        cube.name = "Cube " + grid.Value.index;
                        
                        grid.Value.SetGameObject(cube);
                        
                        _allGridTransform.Add(cube.transform, grid.Value);
                        
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
            // read from file
            StreamReader reader = new StreamReader(pathFile);
            string map = reader.ReadToEnd();

            // Serialize string
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

