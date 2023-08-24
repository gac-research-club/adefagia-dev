using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Action_", menuName = "MechAI/Action")]
public class CharacterAction: ScriptableObject
{
    public ActionType actionType;
    public Material material;
    public Color color;

}

public enum ActionType
{
    Deploy,
    Select,
    Move,
}