using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace adefagia.Graph
{
    public class Grid
    {
        private readonly GridManager _gridManager;
        public readonly Vector2 location;
        public GridType GridType { get; }
        public GameObject GridGameObject { get; }

        public Grid[] neighbors;
        
        public float priority;

        public bool Occupied { get; private set; }
        public bool IsHover { get; private set; }
        public bool IsSelect { get; private set; }

        public Grid(GridManager gridManager, Vector2 location, GridType gridType)
        {
            _gridManager = gridManager;
            this.location = location;
            GridType = gridType;

            neighbors = new Grid[4];

            GridGameObject = Instantiate();
        }

        private GameObject Instantiate()
        {
            var prefab = _gridManager.GetPrefab(GridType);
            if (prefab.IsUnityNull()) return null;
            
            var gridGameObject = Object.Instantiate(prefab, GetLocation(), prefab!.transform.rotation, _gridManager.transform);
            gridGameObject.name = $"Grid ({location.x},{location.y})";
            
            // Set reference to GridStatus
            gridGameObject.GetComponent<GridStatus>().Grid = this;

            return gridGameObject;
        }

        public Vector3 GetLocation(float x = 0, float y = 0, float z = 0)
        {
            return Vec2ToVec3(location) + new Vector3(x,y,z);
        }

        public static Vector3 Vec2ToVec3(Vector2 loc)
        {
            return new Vector3(loc.x, 0f, loc.y);
        }

        public void Occupy()
        {
            Occupied = true;
        }
        
        public void Selected(bool value)
        {
            IsSelect = value;
        }

        public void Hover(bool value)
        {
            IsHover = value;
        }

        public Grid Right => neighbors[0];
        public Grid Up => neighbors[1];
        public Grid Left => neighbors[2];
        public Grid Down => neighbors[3];
    }

    public enum GridType {
        Empty,
        Ground,
        Border
    }
}