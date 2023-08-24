using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private const string Tag = "Tile";
    
    private float _height;
    private float _initHeight;
    
    private GameObject _baseGameObject;
    private GameObject _propGameObject;
    private GameObject _highlightGameObject;
    public Vector2Int Coordinate { get; }

    public TileHighlight TileHighlight;

    public Vector3 Position => Functions.CoordinateToPosition(Coordinate, _initHeight, Size);
    public Vector3 PropPosition => Functions.CoordinateToPosition(Coordinate, Height, Size);

    public Mode Mode { get; private set; }

    private TileObject BaseObject { get; set; }
    private TileObject PropObject { get; set; }

    private float Height
    {
        get => _height;
        set => _height = value * Size;
    }

    private float Size { get; }

    public bool AnyObstacle => (PropObject != null);
    public bool CannotMove => AnyObstacle ? PropObject.cannotMove : BaseObject.cannotMove;

    // Info Tile
    public string Description => AnyObstacle ? PropObject.description : BaseObject.description;
    

    public Tile(Vector2Int coordinate, float height = 0, float size = 1)
    {
        Coordinate = coordinate;
        Mode = Mode.Free;
        Size = size;

        Height = height;
        _initHeight = height;
    }

    public void ChangeTileObject(Transform parent, TileObject tileObject)
    {
        BaseObject = tileObject;
        if (_baseGameObject != null)
        {
            Object.Destroy(_baseGameObject);
        }
        
        var tilePrefab = tileObject.prefab;
        tilePrefab.transform.position = Position;
        
        _baseGameObject = Object.Instantiate(tilePrefab, parent);
        
        // For raycast
        // ChangeCoordinateListener(_baseGameObject, 0, Coordinate);
    }

    public void AddProp(Transform parent, TileObject tileObject)
    {
        Height++;
        
        PropObject = tileObject;
        
        var propPrefab = tileObject.prefab;
        propPrefab.transform.position = PropPosition;
        
        _propGameObject = Object.Instantiate(propPrefab, parent);
    }

    public void RemoveProp()
    {
        Height--;
        
        PropObject = null;
        
        if (_propGameObject != null)
        {
            Object.Destroy(_propGameObject);
        }
    }
    
    /// <summary>
    ///  Highlight for each Tile
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tileObject"></param>
    public void AddHighlight(Transform parent, TileObject tileObject)
    {
        TileHighlight = new TileHighlight(this, tileObject, parent);
    }

    public void Occupy(bool isOccupy)
    {
        if (isOccupy)
        {
            Mode = Mode.Occupy;
        }
        else
        {
            Mode = Mode.Free;
        }
    }

    private bool ChangeCoordinateListener(GameObject parent, int childIndex, Vector2Int coordinate)
    {
        var child = parent.transform.GetChild(childIndex);
        if (!child)
        {
            Debug.LogWarning($"{Tag}: GameObject ({child.gameObject.name}) is Not Tile Listener. Please Change index");
            return false;
        }
        
        child.GetComponent<TileListener>().coordinate = coordinate;
        return true;
    }

    public override string ToString()
    {
        return AnyObstacle ? PropObject.tileName : BaseObject.tileName;
    }
}
