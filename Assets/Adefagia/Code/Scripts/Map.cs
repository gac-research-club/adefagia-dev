using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<RowMapTileSimple> mapTiles = new List<RowMapTileSimple>(10);

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            mapTiles.Add(new RowMapTileSimple());
            for (int j = 0; j < 10; j++)
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
        if (!File.Exists(path)) return false;
        
        var json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, this);
        return true;
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