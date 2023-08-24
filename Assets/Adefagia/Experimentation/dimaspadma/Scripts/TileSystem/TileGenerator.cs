using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGenerator : MonoBehaviour
{
    private const string Tag = "TileGenerator";

    [Header("Bases")]
    [SerializeField] private Transform baseParent;
    [SerializeField] private List<TileObject> baseTileObjects;
    public float tileSize;
    
    [SerializeField] private Vector2Int gridLayout;
    
    [Header("Obstacles")] 
    [SerializeField] private int obstacleAmount;
    [SerializeField] private Transform propParent;
    [SerializeField] private List<TileObject> propTileObjects;

    [SerializeField] private Transform highlightParent;
    [SerializeField] private TileObject tileHighlight;

    [SerializeField] private CharacterAction deployAction;

    // List of Tiles
    private static Dictionary<Vector2Int, Tile> BaseTiles;
    private int _tileHeight;

    private void Awake()
    {
        GenerateBaseTile(gridLayout);
        GenerateObstacle(gridLayout, obstacleAmount); // 10 obstacle
        GenerateHighLight(gridLayout);
    }


    private void GenerateBaseTile(Vector2Int layout)
    {
        BaseTiles = new Dictionary<Vector2Int, Tile>();
        
        // X for column
        for (var x = 0; x < layout.x; x++)
        {
            // Y for row
            for (var y = 0; y < layout.y; y++)
            {
                var coordinate = new Vector2Int(x, y);

                var tile = CreateTile(coordinate);
                if (tile != null)
                {
                    BaseTiles[coordinate] = tile;
                }
            }
        }
    }

    private void GenerateObstacle(Vector2Int layout, int amount)
    {
        if (propParent == null)
        {
            Debug.LogWarning($"{Tag}: Map without Obstacle. Add parent to obstacles gameObject");
            return;
        }
        
        var count = 0;
        while (count < amount)
        {
            // TODO: Random per chunk. Misal 4x4 membentu pola foret. atau pola montain
            
            // Random x and y
            var x = Random.Range(0, layout.x);
            var y = Random.Range(0, layout.y);

            var tile = GetTile(x, y);
            
            if (!tile.AnyObstacle)
            {
                // Add Obstacle
                CreateObstacle(tile);
                
                count++;
            }
            
        }
    }

    private void GenerateHighLight(Vector2Int layout)
    {
        // X for column
        for (var x = 0; x < layout.x; x++)
        {
            // Y for row
            for (var y = 0; y < layout.y; y++)
            {
                var tile = GetTile(x, y);

                CreateHighlight(tile);
            }
        }
    }

    private Tile CreateTile(Vector2Int coordinate)
    {
        try
        {
            // Randomly select baseTileObject
            var index = Random.Range(0, baseTileObjects.Count);

            // Create new tile
            var tile = new Tile(
                coordinate,
                _tileHeight,
                tileSize);

            tile.ChangeTileObject(baseParent, baseTileObjects[index]);

            return tile;
        }
        catch (Exception err)
        {
            Debug.LogError($"{Tag}: Add minimum 1 TileObject on List BaseTileObjects");
            BaseTiles = null;
            return null;
        }
    }

    private void CreateObstacle(Tile tile)
    {
        try
        {
            // Randomly select baseTileObject
            var index = Random.Range(0, propTileObjects.Count);
            
            tile.AddProp(
                propParent, 
                propTileObjects[index]);
        }
        catch (Exception err)
        {
            Debug.LogWarning($"{Tag}: Map without Obstacle. Add minimum 1 TileObject on List PropTileObjects");
        }
    }

    private void CreateHighlight(Tile tile)
    {
        tile.AddHighlight(highlightParent, tileHighlight);
    }

    public static void ShowListHighlight(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.TileHighlight.Show();
        }
    }

    public static Tile GetTile(int x, int y)
    {
        var coordinate = new Vector2Int(x, y);
        
        try
        {
            return BaseTiles[coordinate];
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogWarning($"{Tag}: Tile with {coordinate} coordinate not Found");
            return null;
        }
        catch (NullReferenceException)
        {
            Debug.LogError($"{Tag}: Tilemap has not generated and you want getTile");
            return null;
        }
    }
}
