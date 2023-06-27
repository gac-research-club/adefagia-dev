using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.RobotSystem;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public float HealthAmount;

    public override void ExecuteEffect(Robot character)
    {
        character.Healing(HealthAmount);
    }

    public override string GetDescription()
    {
        return "Heals for " + HealthAmount + " health";
    }
}
