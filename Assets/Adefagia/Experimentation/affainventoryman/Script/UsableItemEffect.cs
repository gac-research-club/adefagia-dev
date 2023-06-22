using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.RobotSystem;

public abstract class UsableItemEffect : ScriptableObject
{
    public abstract void ExecuteEffect(Robot character);

    public abstract string GetDescription();
}
