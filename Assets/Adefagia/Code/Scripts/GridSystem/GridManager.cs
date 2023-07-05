using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Adefagia.BattleMechanism;
using Adefagia.Collections;
using Adefagia.SelectObject;

namespace Adefagia.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject gridQuad;
        [SerializeField] private GameObject gridQuadSelect;

        [SerializeField] private float gridLength = 1;
        [SerializeField] private int gridSizeX, gridSizeY;

        [SerializeField] private List<GridElement> listGridPrefab;

        [SerializeField] private Vector3 offset;

        private Dictionary<GridType, GridElement> _gridElements;

        private Select _select;

        public static float GridLength;
        
        // Grid state
        public GridController gridSelect;
        public GridController gridTemp;
        public GridController gridLast;

        // List All Grid
        private Grid[,] _listGrid;

        public static bool DoneGenerate = false;
        public static event Action<GridController> GridHover;
        public static event Action<GridController> GridHoverInfo;

        public static event Action<GridController> SkillHappened; 

        private void Awake()
        {

            GridLength = gridLength;
            
            // Set into gameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.gridManager = this;
            }
            
            StartCoroutine(InitializeGridManager());
            _select = GetComponent<Select>();
        }

        private void Update()
        {
            if (BattleManager.gameState == GameState.Battle)
            {
                var robotControllerSelected = BattleManager.TeamActive.RobotControllerSelected;
                if (robotControllerSelected != null)
                {
                    gridQuadSelect.transform.position = CellToWorld(robotControllerSelected.Robot.Location);
                }
                else
                {
                    // Kept away from keep sent
                    gridQuadSelect.transform.position = new Vector3(99, 99, 99);
                }
            }
        }

        /*----------------------------------------------------------------------
         * Initialize Grid, their Neighbor 
         *----------------------------------------------------------------------*/
        private IEnumerator InitializeGridManager()
        {
            // Wait until GameState is Initialize
            while (BattleManager.gameState != GameState.Initialize)
            {
                yield return null;
            }

            // Init gridElements
            CreateGridElements();

            // Generate Grids
            GenerateGrids();

            // Set grid neighbor 
            SetNeighbors();

            // BattleManager.ChangeGameState(GameState.Preparation);
            
            // Finish Generate
            DoneGenerate = true;
        }

        private void CreateGridElements()
        {
            _gridElements = new Dictionary<GridType, GridElement>();
            var duplicateCount = 0;
            foreach (var gridElement in listGridPrefab)
            {
                try
                {
                    if (_gridElements.ContainsKey(gridElement.gridType)) throw new DictionaryDuplicate();
                    _gridElements.Add(gridElement.gridType, gridElement);
                }
                catch (DictionaryDuplicate)
                {
                    duplicateCount++;
                }
            }

            // duplicate error message
            if (duplicateCount > 0) Debug.LogWarning($"Duplicate {duplicateCount} Item.");
        }

        /*----------------------------------------------------------------------------------
         * Generate map grid secara procedural sesuai ukurang x dan y.
         * tipe Grid ditentukan dari urutan character string map.
         *---------------------------------------------------------------------------------*/
        private void GenerateGrids(int x, int y)
        {
            _listGrid = new Grid[x, y];

            // Set all Grid by (x,y)
            // xi = x index
            // yi = y index
            for (int yi = y - 1; yi >= 0; yi--)
            {
                for (var xi = 0; xi < x; xi++)
                {
                    var prefab = _gridElements[GridType.Ground].prefab;
                    
                    // Create gameObject of grid
                    var gridObject = Instantiate(prefab, transform);

                    gridObject.transform.localScale = UpdateScale(gridObject.transform);
                    
                    gridObject.transform.position = new Vector3(xi * gridLength, 0, yi * gridLength) + offset;
                    gridObject.name = $"Grid ({xi}, {yi})";

                    // Add Grid Controller
                    var gridController = gridObject.AddComponent<GridController>();
                    gridController.Grid = new Grid(xi, yi);

                    // Add into List grid Object
                    _listGrid[xi, yi] = gridController.Grid;
                }
            }
        }
        private void GenerateGrids(Vector2 gridSize) => GenerateGrids((int)gridSize.x, (int)gridSize.y);
        private void GenerateGrids() => GenerateGrids(gridSizeX, gridSizeY);


        /*----------------------------------------------------------------------------------
         * Menyambungkan 1 grid dengan 4 grid di sebelahnya,
         * yaitu: Kanan, Atas, Kiri, Bawah.
         * Dan jika border pada inspector diset true,
         * maka neighbor yang null akan menjadi borderObject.
         *---------------------------------------------------------------------------------*/
        void SetNeighbors()
        {
            // Access all grid
            for (int yi = 0; yi < gridSizeY; yi++)
            {
                for (var xi = 0; xi < gridSizeX; xi++)
                {
                    _listGrid[xi, yi].AddNeighbor(GridDirection.Right, GetGrid(xi + 1, yi));
                    _listGrid[xi, yi].AddNeighbor(GridDirection.Up, GetGrid(xi, yi + 1));
                    _listGrid[xi, yi].AddNeighbor(GridDirection.Left, GetGrid(xi - 1, yi));
                    _listGrid[xi, yi].AddNeighbor(GridDirection.Down, GetGrid(xi, yi - 1));
                }
            }
        }

        /*-----------------------------------------------------------------------------------------------
         * Fungsi untuk Mengambil Grid
         *-----------------------------------------------------------------------------------------------*/
        public Grid GetGrid(int x, int y, bool debugMessage = false)
        {
            try
            {
                return _listGrid[x, y];
            }
            catch (IndexOutOfRangeException error)
            {
                if (debugMessage) Debug.LogWarning(error.Message);
                return null;
            }
        }
        public Grid GetGrid(Vector2Int location, bool debugMessage = false)
        {
            return GetGrid(location.x, location.y, debugMessage);
        }

        // Get Vector3 by Grid
        public static Vector3 CellToWorld(Grid grid)
        {
            return new Vector3(grid.X * GridLength, 0, grid.Y * GridLength);
        }

        // Grid hover 
        public Grid GetGrid()
        {
            return GetGridController()?.Grid;
        }

        public GridController GetGridController()
        {
            try
            {
                return _select.GetObjectHit<GridController>();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public GridController GetGridController(Grid grid)
        {
            // with find object
            var obj = GameObject.Find(grid.ToString());

            return obj.GetComponent<GridController>();
        }

        #region UnityEvent

        public void OnMouseHover(GameObject objectHit)
        {
            // Move grid Quad
            if (objectHit == null)
            {
                gridQuad.transform.position = new Vector3(99, 99, 99);
                return;
            }

            gridSelect = GetGridController();

            if (gridSelect != gridTemp)
            {
                gridLast = gridTemp;
                gridTemp = gridSelect;
                
                GridHoverInfo?.Invoke(gridSelect);
                
                // Debug.Log("Current: " + gridSelect);
                // if (gridSelect != null && BattleManager.battleState == BattleState.AttackRobot)
                // {
                //     GridHover?.Invoke(gridSelect);
                // }
            }

            if (BattleManager.battleState == BattleState.SkillSelectionRobot)
            {
                SkillHappened?.Invoke(GetGridController());
                GridHover?.Invoke(gridSelect);
            }

            // var _grid = GetGrid(); 
            // if(_grid.Status == GridStatus.Obstacle){
            //     gridQuad.transform.position = new Vector3(99, 99, 99);
            //     return;
            // }

            gridQuad.transform.position = objectHit.transform.position - offset;
        }

        #endregion


        public static Vector3 UpdateScale(Transform original)
        {
            var defaultScale = original.localScale;
            
            var result = new Vector3(
                defaultScale.x * GridLength,
                defaultScale.y * GridLength, 
                defaultScale.z * GridLength);
            
            return result;
        }
        
        private void OnDrawGizmos()
        {
            var center = (gridSizeX * gridLength + gridSizeY * gridLength) * 0.5f;
            Gizmos.DrawWireCube(transform.position + new Vector3(center * 0.5f - 0.5f, 0, center * 0.5f - 0.5f), new Vector3(center, 1, center));
        }
    }
}

