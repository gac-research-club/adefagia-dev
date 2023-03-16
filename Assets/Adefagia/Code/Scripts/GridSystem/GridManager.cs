using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

using Adefagia.SelectObject;
using UnityEngine.Serialization;

namespace Adefagia.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public int xSize, ySize;
        public bool border;
        public Transform borderParent;
        
        public bool emptyMap;
        
        // For Debugging
        // public int index;
        // public Vector2 nodeLoc;
        // ---
        
        public List<GridScriptableObject> listGridPrefab;
        public Vector3 offsetGridPosition;

        public string mapName = "Map";
    
        private bool _doneGenerateGrids;
        
        // List All Grid
        private Grid[,] _listGrid;

        private const GridType DefaultTypeGrid = GridType.Ground;

        public SelectableObject<Grid> GridSelected { get; private set; }

        private void Awake()
        {
            // Map String
            var map = ReadFile("Assets/Adefagia/Resources/"+ mapName +".txt");

            if (emptyMap)
            {
                GenerateDefaultGrids();
            }
            else if(ValidateMap(map))
            {
                GenerateGrids(map);
            }
            else
            {
                Debug.LogWarning("Ukuran Map tidak sesuai dengan grid Size");
                Debug.LogWarning("**Generate Default Map**");
                GenerateDefaultGrids();
            }
            
            SetNeighbors();

            // _doneGenerateGrids = true;
            GridSelected = new SelectableObject<Grid>(this);
        }

        public void Start()
        {
            // GetGrid(3, 4, true);
            // GameManager.instance.gridManager.GetGrid(3,4, true);
        }

        /*----------------------------------------------------------------------------------
         * GenerateGrids(string map)
         *  - string map // isi string dari text map. Bisa memakai fungsi ReadFile(namaMap)
         * 
         * GenerateGrids(Vector2 size, string map)
         * params :
         *  - vector2 size // Vector untuk ukuran x dan y, misal new Vector2(x, y)
         * 
         * GenerateGrids(int x, int y, string map)
         * params :
         *  - int x // untuk ukuran x
         *  - int y // untuk ukuran y
         *
         * Generate map grid secara procedural sesuai ukurang x dan y.
         * tipe Grid ditentukan dari urutan character string map.
         *---------------------------------------------------------------------------------*/
        private void GenerateGrids(int x, int y, string map)
        {
            _listGrid = new Grid[x, y];

            // Set all Grid by (x,y)
            // xi = x index
            // yi = y index
            for (int i = 0, yi = y-1; yi >= 0 ; yi--)
            {
                for (var xi = 0; xi < x; xi++, i++)
                {
                    // Add every node
                    // Set location (xi,yi)
                    // Set type of grid (by map)
                    var grid = new Grid(this, new Vector2(xi, yi), GetGridTypeByChar(map[i]));

                    // Add into List grid Object
                    _listGrid[xi, yi] = grid;
                }
            }
        }
        private void GenerateGrids(Vector2 gridSize, string map) => GenerateGrids((int)gridSize.x, (int)gridSize.y, map);
        private void GenerateGrids(string map) => GenerateGrids(xSize, ySize, map);


        /*----------------------------------------------------------------------------------
         * GenerateDefaultGrids()
         * GenerateDefaultGrids(Vector2 size)
         * params :
         *  - vector2 size // Vector untuk ukuran x dan y, misal new Vector2(x, y)
         * 
         * GenerateDefaultGrids(int x, int y)
         * params :
         *  - int x // untuk ukuran x
         *  - int y // untuk ukuran y
         *
         * Generate map grid secara procedural sesuai ukurang x dan y dengan DefaultTypeGrid
         *---------------------------------------------------------------------------------*/
        private void GenerateDefaultGrids(int x, int y)
        {
            _listGrid = new Grid[x, y];

            // Set all Grid by (x,y)
            // xi = x index
            // yi = y index
            for (int yi = y-1; yi >= 0 ; yi--)
            {
                for (int xi = 0; xi < x; xi++)
                {
                    // Add every node
                    // Set location (xi,yi)
                    // Set type of grid (by map)
                    var grid = new Grid(this, new Vector2(xi, yi), DefaultTypeGrid);

                    // Add into List grid Object
                    _listGrid[xi, yi] = grid;
                }
            }
        }
        private void GenerateDefaultGrids(Vector2 gridSize) => GenerateDefaultGrids((int)gridSize.x, (int)gridSize.y);
        private void GenerateDefaultGrids() => GenerateDefaultGrids(xSize, ySize);


        /*----------------------------------------------------------------------------------
         * SetNeighbors()
         *
         * Menyambungkan 1 grid dengan 4 grid di sebelahnya,
         * yaitu: Kanan, Atas, Kiri, Bawah.
         * Dan jika border pada inspector diset true,
         * maka neighbor yang null akan menjadi borderObject.
         *---------------------------------------------------------------------------------*/
        void SetNeighbors()
        {
            // Set all grid neighbors
            foreach (var grid in _listGrid)
            {
                // Add neighbor
                // Add 4 : right, up, left, down
                Vector2[] dirs = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
                grid.Neighbors = new Grid[dirs.Length];
                
                for (var i=0; i<dirs.Length; i++)
                {
                    var location = grid.Location + dirs[i];
                    var neighbor = GetGrid(location);
                    grid.Neighbors[i] = neighbor;
                    
                    // If neighbor is null make it border game object
                    if(grid.Neighbors[i] == null && border) InstantiateBorder(location);
                }
            }
            
            if(border) InstantiateBorderCorner();
        }
        
        public GameObject GetPrefab(GridType gridType)
        {
            // Check if gridType is equal in each ListGridPrefab
            foreach (var data in listGridPrefab.Where(data => data.gridPrefab.dataPrefab.gridType == gridType))
            {
                return data.gridPrefab.dataPrefab.prefab;
            }

            Debug.LogWarning(gridType + " must be listed in GridManager", transform);
            return null;
        }

        
        /*-----------------------------------------------------------------------------------------------
         * GetGrid() // return grid yang sedang terselect
         *
         * GetGrid(int x, int y, bool debugMessage = false)
         * params:
         *  - int x // index grid x
         *  - int y // index grid y
         *  - bool debugMessage // optional for Debug.Log GetGrid Result
         * 
         * GetGrid(Vector2 location, bool debugMessage = false)
         * - vector XYindex // Location where x and y grid
         *
         * Fungsi untuk Mengambil Grid
         *-----------------------------------------------------------------------------------------------*/
        public Grid GetGrid(int x, int y, bool debugMessage = false)
        {
            // Make sure (x,y) in range of grid Size
            if (x < 0 || x > xSize - 1 || y < 0 || y > ySize - 1)
            {
                if (debugMessage)
                {
                    Debug.LogWarning($"Grid ({x},{y}) Index is out of range");
                }
                return null;
            }
            
            var grid = _listGrid[x, y];
            if (debugMessage)
            {
                Debug.Log($"Grid ({grid.Location.x},{grid.Location.y})");
            }

            return grid;
        }
        public Grid GetGrid(Vector2 location, bool debugMessage = false)
        {
            return GetGrid((int) location.x, (int) location.y, debugMessage);
        }

        public static Grid GetGrid()
        {
            return GameManager.instance.gridManager.GetGridSelected();
        }
        public Grid GetGridSelected()
        {
            return GridSelected.GetSelect();
        }
        
        #region UnityEvent
        public void MouseHover(GameObject gridGameObject)
        {
            // Debug.Log("Hover");
            var grid = gridGameObject.GetComponent<GridStatus>().Grid;
            
            if(grid == null) return;
            
            GridSelected.ChangeHover(grid);
        }

        public void MouseNotHover()
        {
            GridSelected.NotHover();
        }

        public void GridClick(GameObject gridGameObject)
        {
            // Debug.Log("click");
            var grid = gridGameObject.GetComponent<GridStatus>().Grid;

            // Only select on specific grid
            if (!grid.IsOccupied && grid.IsHover)
            {
                GridSelected.ChangeSelect(grid);
                UIManager.HideCanvas();
            }
        }
        #endregion

        
        public static bool CheckGround(Grid grid)
        {
            return grid.GridType == GridType.Ground;
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

        private void InstantiateBorder(Vector2 location)
        {
            var loc3 = new Vector3(location.x, 0, location.y);
            var borderPrefab = GetPrefab(GridType.Border);
            if(borderPrefab == null) return;
            Instantiate(borderPrefab, loc3, Quaternion.identity, borderParent);
        }
        private void InstantiateBorderCorner()
        {
            InstantiateBorder(new Vector2(-1,-1));
            InstantiateBorder(new Vector2(xSize,-1));
            InstantiateBorder(new Vector2(-1,ySize));
            InstantiateBorder(new Vector2(xSize,ySize));
        }
        
        // public static bool IsGridEmpty(Grid grid)
        // {
        //     // return grid.IsUnityNull() || grid.IsEmpty();
        //     throw new NotImplementedException();
        // }

        /*----------------------------------------------------------------------------------
         * ValidateMap(string mapString)
         * params:
         *  - string mapString // text dari Map
         *
         * Return true if map sesuai sama ukuran grid.
         *---------------------------------------------------------------------------------*/
        private bool ValidateMap(string mapString)
        {
            return mapString.Length == (xSize * ySize);
        }

        /*----------------------------------------------------------------------------------
         * ReadFile(string pathFile)
         * params:
         *  - string pathFile // dimulai dari folder Asset. Misal: "Assets/Map/Map2.txt"
         *
         * Read File from directory and return the content text
         * and turn into one line.
         *---------------------------------------------------------------------------------*/
        private string ReadFile(string pathFile)
        {
            var map = "";

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
            catch (DirectoryNotFoundException)
            {
                Debug.LogWarning("File Map not found", this);
            }

            // Serialize string
            // Debug.Log(map.Length);
            return CleanInput(map);
        }
        
        
        /*----------------------------------------------------------------------------------
         * ---------------------------------------------------------------------------------
         * CleanInput(string strIn)
         * params:
         *  - string strIn // text
         *
         * Replacing some character in the text to empty string
         * character:
         *  - Selain huruf & angka
         *  - Selain (-)
         * ---------------------------------------------------------------------------------
         *---------------------------------------------------------------------------------*/
        private string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try {
                return Regex.Replace(strIn, @"[^\w-]", "", RegexOptions.None, TimeSpan.FromSeconds(1));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException) {
                return "";
            }
        }
    
    }
}

