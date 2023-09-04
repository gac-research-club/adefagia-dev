using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private string mapName;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Map map;
    [SerializeField] private List<MapTile> tiles;
    [SerializeField] private Vector2Int offset;
    [SerializeField] private int maxTile;
    [SerializeField] private int minTile;
    
    [SerializeField] private Camera camera;

    private bool _multipleSelect;
    private Vector2Int _startPos, _endPos;
    private int _index;

    private void Start()
    {
        //Clear the map (ensures we dont overlap)
        mapName = PlayerPrefs.GetString("Map");
        if (map.LoadMap(mapName))
        {
            foreach (var row in map.mapTiles)
            {
                foreach (var col in row.row)
                {
                    DrawTile(col.position, GetTile(col.tile));
                }
            }
            return;
        }
        
        tilemap.ClearAllTiles();
        // map.test = 1;li
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var pos = new Vector2Int(i, j);
                var tile = GetTile(_index);
                
                DrawTile(pos, tile);
                AddTile(pos, tile);
            }
        }
    }

    private void Update()
    {
        ChangeIndex();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _multipleSelect = !_multipleSelect;
            // Debug.Log(_multipleSelect);
        }
        
        if (_multipleSelect)
        {
            MultipleSelect(GetTile(_index));
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                // Debug.Log(GetPosition());
                var pos= GetPosition() - offset;

                if (!InRange(pos)) return;
                
                var tile = GetTile(_index);
                DrawTile(pos, tile);
                AddTile(pos, tile);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,200,200));
        if (GUILayout.Button("Save")) map.SaveMap(mapName);
        if (GUILayout.Button("Back")) SceneManager.LoadScene("MapEditorSelect");
        GUILayout.EndArea();
    }

    void ChangeIndex()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _index = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) _index = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) _index = 2;
    }
    
    MapTile GetTile(int index)
    {
        index = (index > tiles.Count-1) ? tiles.Count - 1 : (index < 0) ? 0 : index;
        return tiles[index];
    }
    
    MapTile GetTile(TileType type)
    {
        foreach (var tile in tiles)
        {
            if (tile.tileType == type) return tile;
        }

        return null;
    }

    void DrawTile(Vector2Int pos, MapTile tile)
    {
        tilemap.SetTile(new Vector3Int(pos.x, pos.y,0), tile.tileBase);
    }

    void AddTile(Vector2Int pos, MapTile tile)
    {
        tile.position = pos;
        map.AddTile(tile);
    }

    Vector2Int GetPosition()
    {
        var mouse = Input.mousePosition;
        var point = camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, camera.nearClipPlane));
        
        var x = (int)Mathf.Floor(point.x);
        var y = (int)Mathf.Floor(point.y);

        return new Vector2Int(x, y);
    }

    bool InRange(Vector2Int pos)
    {
        return (pos.x < maxTile && pos.x >= minTile && pos.y < maxTile && pos.y >= minTile);
    }

    void MultipleSelect(MapTile tile)
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = GetPosition() - offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPos = GetPosition() - offset;

            _startPos.x = (_startPos.x > maxTile - 1) ? maxTile - 1 : (_startPos.x < minTile) ? minTile : _startPos.x;
            _startPos.y = (_startPos.y > maxTile - 1) ? maxTile - 1 : (_startPos.y < minTile) ? minTile : _startPos.y;
            
            _endPos.x = (_endPos.x > maxTile - 1) ? maxTile - 1 : (_endPos.x < minTile) ? minTile : _endPos.x;
            _endPos.y = (_endPos.y > maxTile - 1) ? maxTile - 1 : (_endPos.y < minTile) ? minTile : _endPos.y;

            var maxX = (_startPos.x > _endPos.x) ? _startPos.x : _endPos.x;
            var maxY = (_startPos.y > _endPos.y) ? _startPos.y : _endPos.y;
            
            var minX = (_startPos.x > _endPos.x) ? _endPos.x : _startPos.x;
            var minY = (_startPos.y > _endPos.y) ? _endPos.y : _startPos.y;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var pos = new Vector2Int(x, y);
                    DrawTile(pos, tile);
                    AddTile(pos, tile);
                }
            }
        }
    }
}
