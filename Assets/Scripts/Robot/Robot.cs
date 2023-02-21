using adefagia.Collections;
using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.Robot
{
    
    public class Robot : ISelectableObject
    {
        public int Id { get; }
        public GameObject RobotGameObject { get; }
        public Grid Grid { get; private set; }

        private readonly Spawner _spawner;
        private RobotManager _robotManager;
        
        public bool IsHover { get; private set; }
        public bool IsSelect { get; private set; }
        
        public Robot(Spawner spawner, RobotPrefab prefab)
        {
            _spawner = spawner;

            Id = prefab.id;
            Grid = GameManager.instance.gridManager.GetGridByLocation(prefab.location);
            RobotGameObject = Instantiate(prefab.gameObject);
        }

        private GameObject Instantiate(GameObject prefab)
        {
            if (prefab.IsUnityNull()) return null;

            if (Grid != null)
            {
                if (GridManager.CheckGround(Grid) && !Grid.IsOccupied)
                {
                    Debug.Log("Instantiate Robot");
                    var robot = Object.Instantiate(prefab, Grid.GetLocation(), prefab.transform.rotation, _spawner.SpawnerGameObject.transform);
                    robot.name = "Robot " + Id;

                    robot.GetComponent<RobotStatus>().Robot = this;
                    robot.GetComponent<RobotMovement>().Robot = this;
                    
                    // Grid Occupied
                    Grid.Occupy();

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

        public void Move(Grid grid)
        {
            Grid = grid;
        }

    }
}