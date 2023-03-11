using UnityEngine;

using Adefagia.SelectObject;
using Adefagia.RobotSystem;

namespace Adefagia.GridSystem
{
    public class Grid : ISelectableObject
    {
        private readonly GridManager _gridManager;
        public Vector2 Location { get;}
        public GridType GridType { get; }
        public GameObject GridGameObject { get; }

        public Grid[] Neighbors { get; set; }
        
        public float Priority { get; set; }

        public bool IsOccupied { get; private set; }
        public bool IsHover { get; private set; }
        public bool IsHighlight { get; set; }
        public bool IsSelect { get; set; }
        public Robot Robot { get; set; }

        public Grid(GridManager gridManager, Vector2 location, GridType gridType)
        {
            _gridManager = gridManager;
            Location = location;
            GridType = gridType;

            GridGameObject = Instantiate();
        }

        private GameObject Instantiate()
        {
            var prefab = _gridManager.GetPrefab(GridType);
            if (prefab == null) return null;
            
            var gridGameObject = Object.Instantiate(prefab, GetLocation(_gridManager.offsetGrid), prefab!.transform.rotation, _gridManager.transform);
            gridGameObject.name = $"Grid ({Location.x},{Location.y})";
            
            // Set reference to GridStatus
            if (gridGameObject.GetComponent<GridStatus>() == null)
            {
                gridGameObject.AddComponent<GridStatus>();
            }
            gridGameObject.GetComponent<GridStatus>().Grid = this;

            return gridGameObject;
        }

        public Vector3 GetLocation(float x = 0, float y = 0, float z = 0)
        {
            return Vec2ToVec3(Location) + new Vector3(x,y,z);
        }
        public Vector3 GetLocation(Vector3 offset) => Vec2ToVec3(Location) + offset;

        public static Vector3 Vec2ToVec3(Vector2 loc)
        {
            return new Vector3(loc.x, 0f, loc.y);
        }

        public void Occupy()
        {
            IsOccupied = true;
        }

        public void Free()
        {
            IsOccupied = false;
        }
        
        public void Selected(bool value)
        {
            IsSelect = value;
        }

        public void Hover(bool value)
        {
            IsHover = value;
        }

        public Grid Right => Neighbors[0];
        public Grid Up => Neighbors[1];
        public Grid Left => Neighbors[2];
        public Grid Down => Neighbors[3];
    }

    public enum GridType {
        Empty,
        Ground,
        Border
    }
}