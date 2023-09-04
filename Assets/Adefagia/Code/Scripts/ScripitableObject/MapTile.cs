using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile_", menuName = "MechAI/Tile")]
public class MapTile : ScriptableObject
{
    public TileType tileType;
    public TileBase tileBase;
    public Vector2Int position;

}
public enum TileType
{
    Ground,
    Mountain,
    Forest,
}
