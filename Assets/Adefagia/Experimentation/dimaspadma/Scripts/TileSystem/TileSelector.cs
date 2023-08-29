    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Camera mCamera;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private CharacterAction actionSelect;
    
    public Tile CurrentTile { get; set; }

    public static event Action<Tile> TileSelecting;
    
    private const string Tag = "TileSelector";

    private bool _finishStart;
    private float _size;

    private void Start()
    {
        _finishStart = CheckingAllField();
    }

    private void Update()
    {
        if(!_finishStart) return;
        
        RayToTile();
    }

    private void RayToTile()
    {
        var ray = mCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, layerMask))
        {
            var tile = GetTileFromListener(raycastHit.transform);
            
            if (CurrentTile != tile)
            {
                CurrentTile = tile;
                
                TileSelecting?.Invoke(CurrentTile);
            }
        }
        else
        {
            if (CurrentTile != null)
            {
                CurrentTile = null;
                
                Debug.Log(CurrentTile);
                TileSelecting?.Invoke(CurrentTile);
            }
        }
    }

    private Tile GetTileFromListener(Transform listener)
    {
        // Debug.Log($"{listener.name}: {Functions.PositionToCoordinate(listener.position, _size)}");
        var coordinate = Functions.PositionToCoordinate(listener.position, _size);
        var tile = TileGenerator.GetTile(coordinate.x, coordinate.y);

        return tile;
    }

    private bool CheckingAllField()
    {
        if (mCamera == null)
        {
            Debug.LogError($"{Tag}: Insert Camera to TileSelector");
            return false;
        }

        if (actionSelect == null)
        {
            Debug.LogError($"{Tag}: Insert CharacterAction to moveAction");
            return false;
        }

        _size = GetComponent<TileGenerator>().tileSize;

        return true;
    }
}
