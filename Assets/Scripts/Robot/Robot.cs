using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.Robot
{
    
    public class Robot
    {
        public int Id { get; }
        public GameObject RobotGameObject { get; }
        public Vector2 location;
        
        private Grid _gridIdle;

        private RobotMovement _robotMovement;

        private bool _active;

        private readonly Spawner _spawner;
        private RobotManager _robotManager;
        
        public bool IsHover { get; private set; }
        public bool IsSelect { get; private set; }
        
        public Robot(Spawner spawner, RobotPrefab prefab)
        {
            Id = prefab.id;
            location = prefab.location;
            _spawner = spawner;

            RobotGameObject = Instantiate(prefab.gameObject);
        }

        private GameObject Instantiate(GameObject prefab)
        {
            if (prefab.IsUnityNull()) return null;
            
            var grid = GameManager.instance.gridManager.GetGridByLocation(location);
            
            if (grid != null)
            {
                if (GridManager.CheckGround(grid) && !grid.Occupied)
                {
                    Debug.Log("Instantiate Robot");
                    var robot = Object.Instantiate(prefab, grid.GetLocation(), prefab.transform.rotation, _spawner.SpawnerGameObject.transform);
                    robot.name = "Robot " + Id;

                    robot.GetComponent<RobotStatus>().Robot = this;
                    
                    // Grid Occupied
                    grid.Occupy();

                    return robot;
                }
            }

            Debug.LogWarning($"Robot ID:{Id} Must be place on Ground Grid & not Occupied");
            return null;
        }
        
        public void Selected(bool value)
        {
            IsSelect = value;
        }
        
        public void Hover(bool value)
        {
            IsHover = value;
        }

    }
}