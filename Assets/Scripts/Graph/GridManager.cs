using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace adefagia.Graph
{
    public class GridManager : MonoBehaviour
    {
        public int xSize, ySize;
        public bool border;
        
        // For Debugging
        // public int index;
        // public Vector2 nodeLoc;
        // ---

        public List<GridPrefab> listGridPrefab;

        public string mapName = "Map";
    
        private bool _doneGenerateGrids;
        
        // List All Grid
        private Grid[,] _listGrid;
        // private List<GameObject> _listGridGameObject;
        private Grid _gridHover, _gridSelect;

        private void Awake()
        {
            GenerateGrids();
            SetNeighbors();
            
            // _doneGenerateGrids = true;
            
            // GenerateBorder();
        }

        private void Start()
        {
            // Debug.Log(GameManager.instance.gridManager.GetPrefab(GridType.Ground)?.name);
            // Debug.Log(GetGridByLocation(new Vector2(4, 0)));
            // Debug.Log(GetGridByLocation(new Vector2(3, 0))?.neighbors[0].GridGameObject.name);
        }

        private void GenerateGrids()
        {
            _listGrid = new Grid[xSize, ySize];
            
            // Map String
            var map = ReadFile("Assets/Resources/"+ mapName +".txt");
            
            // If map empty break generate grid
            if(map == string.Empty || map.Length != xSize*ySize) return;

            // Set all Grid by (x,y)
            for (int i = 0, y = ySize-1; y >= 0 ; y--)
            {
                for (var x = 0; x < xSize; x++, i++)
                {
                    // Add every node
                    // Set location (x,y)
                    // Set type of grid (by map)
                    var grid = new Grid(this, new Vector2(x, y), GetGridTypeByChar(map[i]));

                    // Add into List grid Object
                    _listGrid[x, y] = grid;
                }
            }
        }

        
        [CanBeNull]
        public GameObject GetPrefab(GridType gridType)
        {
            // Check if gridType is equal in each ListGridPrefab
            foreach (var data in listGridPrefab.Where(data => data.dataPrefab.gridType == gridType))
            {
                return data.dataPrefab.prefab;
            }

            Debug.LogWarning(gridType + " must be listed in GridManager", transform);
            return null;
        }

        [CanBeNull]
        public Grid GetGridByLocation(Vector2 location)
        {
            var x = (int) location.x;
            var y = (int) location.y;
            
            // Make sure (x,y) in range of grid Size
            if (x < 0 || x > xSize - 1 || y < 0 || y > ySize - 1)
            {
                // Debug.LogWarning($"({x},{y}) Index is out of range");
                return null;
            }
            
            return _listGrid[x, y];
        }

        public static bool CheckGround(Grid grid)
        {
            return grid.GridType == GridType.Ground;
        }

        private void ChangeHover(Grid grid)
        {
            // First time initialize
            if (_gridHover.IsUnityNull())
            {
                _gridHover = grid;
                _gridHover.Hover(true);
            }
            else
            {
                // different grid disable old grid hover
                if (grid.Equals(_gridHover)) return;
                
                _gridHover.Hover(false);
                _gridHover = grid;
                _gridHover.Hover(true);
            }
        }
        
        private void ChangeSelect(Grid grid)
        {
            // First time initialize
            if (_gridSelect.IsUnityNull())
            {
                _gridSelect = grid;
                _gridSelect.Selected(true);
            }
            else
            {
                // different grid disable old grid hover
                if (grid.Equals(_gridSelect)) return;
                
                _gridSelect.Selected(false);
                _gridSelect = grid;
                _gridSelect.Selected(true);
            }
        }

        public void Hover(GameObject gridGameObject)
        {
            // Debug.Log("Hover");
            var grid = gridGameObject.GetComponent<GridStatus>().Grid;
            ChangeHover(grid);
        }

        public void GridClick(GameObject gridGameObject)
        {
            // Debug.Log("click");
            var grid = gridGameObject.GetComponent<GridStatus>().Grid;
            // grid.Selected();
            ChangeSelect(grid);
        }

        void SetNeighbors()
        {
            // Set all grid neighbors
            foreach (var grid in _listGrid)
            {
                // Add neighbor
                // Add 4 : right, up, left, down
                Vector2[] dirs = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
                grid.neighbors = new Grid[dirs.Length];
                
                for (var i=0; i<dirs.Length; i++)
                {
                    var location = grid.location + dirs[i];
                    var neighbor = GetGridByLocation(location);
                    grid.neighbors[i] = neighbor;
                    
                    // If neighbor is null make it border game object
                    if(grid.neighbors[i].IsUnityNull() && border) InstantiateBorder(location);
                }
            }
            
            if(border) InstantiateBorderCorner();
        }

        private GridType GetGridTypeByChar(char character)
        {
            // Set State by char
            switch (character)
            {
                case '-':
                    return GridType.Empty;
                    
                case 'o':
                    return GridType.Ground;
                
                default:
                    return GridType.Empty;
            }

        }

        void InstantiateBorder(Vector2 location)
        {
            var loc3 = new Vector3(location.x, 0, location.y);
            var borderPrefab = GetPrefab(GridType.Border);
            if(borderPrefab.IsUnityNull()) return;
            Instantiate(borderPrefab, loc3, Quaternion.identity);
        }

        void InstantiateBorderCorner()
        {
            InstantiateBorder(new Vector2(-1,-1));
            InstantiateBorder(new Vector2(xSize,-1));
            InstantiateBorder(new Vector2(-1,ySize));
            InstantiateBorder(new Vector2(xSize,ySize));
        }
        
        public static bool IsGridEmpty(Grid grid)
        {
            // return grid.IsUnityNull() || grid.IsEmpty();
            throw new NotImplementedException();
        }

        /// <summary>
        /// For Debugging in Edit Mode
        /// </summary>
        // private void OnDrawGizmos()
        // {
        //     if (!_doneGenerateGrids) return;
        //
        //     foreach (var grid in _listGrid)
        //     {
        //         Gizmos.color = Color.black;
        //         Gizmos.DrawSphere(grid.GetLocation(), 0.1f);
        //     }
        // }

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
                return Regex.Replace(strIn, @"[^\w-]", "",
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

