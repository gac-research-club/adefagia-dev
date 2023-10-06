using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.RobotSystem;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public float HealthAmount;

    public override void ExecuteEffect(RobotController character)
    {
        character.Robot.Healing(HealthAmount);
        character.Robot.healthBar.UpdateHealthBar(character.Robot.CurrentHealth);

        VFXController.Instance.PlayDebuffLoopVFX(character.gameObject.transform);
    }

    public override string GetDescription()
    {
        return "Heals for " + HealthAmount + " health";
    }
}
