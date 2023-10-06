using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.RobotSystem;

public abstract class UsableItemEffect : ScriptableObject
{
    public abstract void ExecuteEffect(RobotController character);

    public abstract string GetDescription();
}
