using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.SelectObject;
using Adefagia.Collections;

namespace Adefagia.RobotSystem
{
    
    public class Robot : ISelectableObject
    {
        public int Id { get; }
        public GameObject RobotGameObject { get; }
        public Grid Grid { get; private set; }

        private readonly Spawner _spawner;
        private RobotManager _robotManager;
        
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float Damage { get; set; }
        
        public bool IsHover { get; private set; }
        public bool IsSelect { get; private set; }
        
        public OutlineScriptableObject OutlineStyle { get; set; }

        private List<Grid> _gridRange;
        private Vector2[] _dirs;

        public Robot(Spawner spawner, RobotPrefab prefab)
        {
            _spawner = spawner;

            Id = prefab.id;
            
            Grid = spawner.RobotManager.GridManager.GetGrid(prefab.location, true);

            RobotGameObject = Instantiate(prefab.gameObject);

            CurrentHealth = prefab.status.healthPoint;
            Damage = prefab.status.attackDamage;
            
            _gridRange = new List<Grid>();
            _dirs = new[]
            {
                Vector2.right, Vector2.up, Vector2.left, Vector2.down,
                Vector2.down + Vector2.left, Vector2.down + Vector2.right,
                Vector2.up + Vector2.left, Vector2.up + Vector2.right
            };
        }

        private GameObject Instantiate(GameObject prefab)
        {
            if (prefab == null) return null;

            if (Grid != null)
            {
                if (GridManager.CheckGround(Grid) && !Grid.IsOccupied)
                {
                    Debug.Log("Instantiate Robot");
                    var robot = Object.Instantiate(prefab, 
                        Grid.GetLocation(_spawner.RobotManager.offsetRobotPosition),
                        prefab.transform.rotation, _spawner.SpawnerGameObject.transform);
                    robot.name = "Robot " + Id;

                    robot.GetComponent<RobotStatus>().Robot = this;
                    robot.GetComponent<RobotMovement>().Robot = this;
                    
                    // Grid Occupied
                    Grid.Occupy();
                    Grid.Robot = this;

                    return robot;
                }
            }

            Debug.LogWarning($"Robot ID:{Id} Must be place on Ground Grid & not Occupied");
            return null;
        }

        public void Attack(Robot robot)
        {
            robot.CurrentHealth -= Damage;
        }
        
        public void Move(Grid grid)
        {
            Grid = grid;
            if (Grid != null) Grid.Robot = this;
        }
        
        public void SetGridRange()
        {
            var dirs = _dirs.Select(dir => Grid.Location + dir).ToList();

            var bfs = new BFS();
            
            bfs.BFSArea(Grid, dirs);
            // aStar.BFSLine(AStar.BFSLineType.Right, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Up, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Left, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Down, Grid);

            _gridRange = bfs.Reached;
            // aStar.DebugListRobot(aStar.Robots);
        }
        
        public bool TakeDamage(float dmg)
        {
            CurrentHealth -= Damage;
            if (CurrentHealth <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public void ClearGridRange()
        {
            _gridRange.Clear();
        }

        public void ResetDefaultGrid()
        {
            foreach (var grid in _gridRange)
            {
                grid.IsHighlight = false;
                grid.Hover(false);
                grid.Selected(false);
            }
        }

        public bool IsInGridRange(Grid grid)
        {
            return _gridRange.Contains(grid);
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