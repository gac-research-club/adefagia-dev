using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.Collections;
using Adefagia.SelectObject;
using Adefagia.GridSystem;
using Adefagia.MapBase;
using Random = UnityEngine.Random;

namespace Adefagia.ObstacleSystem
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField]
        private int gridSizeX,
            gridSizeY;

        [Header("ObstacleElement ScriptableObject")]
        [SerializeField]
        private List<ObstacleElement> listObstacleObjects;

        [Header("Offset for Obstacle Position")]
        [SerializeField]
        private Vector3 offset;

        private Dictionary<int, ObstacleElement> _obstacleElements;

        private Select _select;
        private GridManager _gridManager;

        // List All Grid
        private ObstacleController[,] _listObstacle;

        private void Start()
        {
            StartCoroutine(InitializeObstacleManager());
            _select = GetComponent<Select>();
        }

        /*----------------------------------------------------------------------
         * Initialize Obstacle, their Neighbor
         *----------------------------------------------------------------------*/
        private IEnumerator InitializeObstacleManager()
        {
            // Wait until GameState is Initialize
            while (BattleManager.gameState != GameState.Initialize)
            {
                yield return null;
            }

            _gridManager = GameManager.instance.gridManager;
            // Init gridElements
            if (MapLoader.successLoad)
            {
                GenerateMapObstacles();
            }
            else
            {
                CreateObstacleElements();

                // Generate Grids
                GenerateObstacles();
            }

            BattleManager.ChangeGameState(GameState.Preparation);
        }

        private void CreateObstacleElements()
        {
            _obstacleElements = new Dictionary<int, ObstacleElement>();

            var duplicateCount = 0;
            for (var index = 0; index < listObstacleObjects.Count; index++)
            {
                try
                {
                    if (_obstacleElements.ContainsKey(index))
                        throw new DictionaryDuplicate();
                    _obstacleElements.Add(index, listObstacleObjects[index]);
                }
                catch (DictionaryDuplicate)
                {
                    duplicateCount++;
                }
            }

            // duplicate error message
            if (duplicateCount > 0)
                Debug.LogWarning($"Duplicate {duplicateCount} Item.");
        }

        /*----------------------------------------------------------------------------------
         * Generate map grid secara procedural sesuai ukurang x dan y.
         * tipe Grid ditentukan dari urutan character string map.
         *---------------------------------------------------------------------------------*/
        private void GenerateObstacles(int x, int y)
        {
            _listObstacle = new ObstacleController[x, y];

            int numPoints = 12; // change this to the number of points you want to generate
            List<Vector2Int> points = new List<Vector2Int>();

            while (points.Count < numPoints)
            {
                int _x = UnityEngine.Random.Range(0, 10); // change the range as needed
                int _y = UnityEngine.Random.Range(0, 10); // change the range as needed

                Vector2Int point = new Vector2Int(_x, _y);

                if (!points.Contains(point))
                {
                    points.Add(point);
                }
            }

            // Set all random Grid by (x,y)

            foreach (Vector2Int point in points)
            {
                var index = Random.Range(0, listObstacleObjects.Count);

                CreateObstacleObject(_obstacleElements[index], point);
            }
        }

        private void GenerateObstacles(Vector2 gridSize) =>
            GenerateObstacles((int)gridSize.x, (int)gridSize.y);

        private void GenerateObstacles() => GenerateObstacles(gridSizeX, gridSizeY);

        void CreateObstacleObject(ObstacleElement obstacleElement, Vector2Int point)
        {
            _listObstacle = new ObstacleController[gridSizeX, gridSizeY];

            // Create gameObject of grid
            var obstacleObject = Instantiate(obstacleElement.Prefab, transform);
            obstacleObject.transform.position =
                new Vector3(point.x * GridManager.GridLength, 0f, point.y * GridManager.GridLength)
                + offset;
            obstacleObject.name = $"Obstacle ({point.x}, {point.y})";

            // Add Obstacle Controller
            var obstacleController = obstacleObject.AddComponent<ObstacleController>();
            obstacleController.ObstacleElement = obstacleElement;
            obstacleController.Obstacle = new Obstacle(point.x, point.y);
            obstacleController.Grid = _gridManager.GetGrid(point.x, point.y);
            obstacleController.Grid.SetObstacle();

            // Ref to grid
            _gridManager.GetGridController(obstacleController.Grid).ObstacleController =
                obstacleController;

            // Add into List grid Object
            _listObstacle[point.x, point.y] = obstacleController;
        }

        private void GenerateMapObstacles()
        {
            int[][] map = _gridManager.generateMap.PositionObstacle;
            for (int row = 0; row < map.Length - 1; row++)
            {
                for (int col = 0; col < map[0].Length - 1; col++)
                {
                    var obstacle = GetObstacleElement(map[row][col]);
                    Vector2Int position = new Vector2Int(col, row);
                    if (obstacle)
                    {
                        CreateObstacleObject(obstacle, position);
                    }
                }
            }
        }

        ObstacleElement GetObstacleElement(int index)
        {
            // foreach (var obstacle in listObstacleObjects)
            // {
            //     if (obstacle.TileType == tileType)
            //         return obstacle;
            // }

            if (index > 0)
            {
                int positionObs = index - 1;
                return listObstacleObjects[positionObs];
            }

            return null;
        }

        /*----------------------------------------------------------------------------------
         * Menyambungkan 1 grid dengan 4 grid di sebelahnya,
         * yaitu: Kanan, Atas, Kiri, Bawah.
         * Dan jika border pada inspector diset true,
         * maka neighbor yang null akan menjadi borderObject.
         *---------------------------------------------------------------------------------*/

        /*-----------------------------------------------------------------------------------------------
         * Fungsi untuk Mengambil Obstacle
         *-----------------------------------------------------------------------------------------------*/
        public ObstacleController GetObstacle(int x, int y, bool debugMessage = false)
        {
            try
            {
                return _listObstacle[x, y];
            }
            catch (IndexOutOfRangeException error)
            {
                if (debugMessage)
                    Debug.LogWarning(error.Message);
                return null;
            }
        }

        public ObstacleController GetObstacle(Vector2 location, bool debugMessage = false)
        {
            return GetObstacle((int)location.x, (int)location.y, debugMessage);
        }

        // Get Vector3 by Obstacle
        public static Vector3 CellToWorld(Obstacle obstacle)
        {
            return new Vector3(obstacle.X, 0, obstacle.Y);
        }

        // Obstacle hover
        public Obstacle GetObstacle()
        {
            return GetObstacleSelect()?.Obstacle;
        }

        public ObstacleController GetObstacleSelect()
        {
            try
            {
                return _select.GetObjectHit<ObstacleController>();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
