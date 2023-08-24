using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile_", menuName = "MechAI/Tile")]
public class TileObject : ScriptableObject
{
    [Header("Common")]
    public GameObject prefab;

    public string tileName;
    [TextArea]
    public string description;

    [Header("Status")] 
    public Type type;

    public bool cannotMove;
}

public enum Type
{
    Base,
    Prop,
    Highlight,
}