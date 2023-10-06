using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Adefagia.MapBase
{
    public class Map : MonoBehaviour
    {
        [SerializeField]
        public List<MapTile> tiles;
        public int sizeMap = 4;
        public List<RowMapTileSimple> mapTiles;
        public Tilemap tilemapInput;

        private void Awake()
        {
            mapTiles = new List<RowMapTileSimple>(sizeMap);
            for (int i = 0; i < sizeMap; i++)
            {
                mapTiles.Add(new RowMapTileSimple());
                for (int j = 0; j < sizeMap; j++)
                {
                    mapTiles[i].row.Add(null);
                }
            }
        }

        public void AddTile(MapTile mapTile)
        {
            var simpleTile = new MapTileSimple();
            simpleTile.position = mapTile.position;
            simpleTile.tile = mapTile.tileType;

            mapTiles[mapTile.position.x].row[mapTile.position.y] = simpleTile;
        }

        public bool LoadMap(string mapName)
        {
            var path = $"{Application.persistentDataPath}/Map/{mapName}.json";
            if (!File.Exists(path))
                return false;

            var json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);

            if (tilemapInput == null)
                tilemapInput = GetComponent<Tilemap>();

            foreach (var row in mapTiles)
            {
                foreach (var tiles in row.row)
                {
                    DrawTile(tiles.position, GetTile(tiles.tile));
                }
            }

            return true;
        }

        MapTile GetTile(int index)
        {
            index =
                (index > tiles.Count - 1)
                    ? tiles.Count - 1
                    : (index < 0)
                        ? 0
                        : index;
            return tiles[index];
        }

        MapTile GetTile(TileType type)
        {
            foreach (var tile in tiles)
            {
                if (tile.tileType == type)
                    return tile;
            }

            return null;
        }

        public void DrawTile(Vector2Int pos, MapTile tile)
        {
            tilemapInput.SetTile(new Vector3Int(pos.x, pos.y, 0), tile.tileBase);
        }

        public void SaveMap(string mapName)
        {
            var json = JsonUtility.ToJson(this);
            var path = $"{Application.persistentDataPath}/Map";

            // Determine whether the directory exists.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += $"/{mapName}.json";
            File.WriteAllText(path, json);
            // Debug.Log(json);
        }
    }

    [Serializable]
    public class MapTileSimple
    {
        public Vector2Int position;
        public TileType tile;
    }

    [Serializable]
    public class RowMapTileSimple
    {
        public List<MapTileSimple> row = new List<MapTileSimple>(10);
    }
}
