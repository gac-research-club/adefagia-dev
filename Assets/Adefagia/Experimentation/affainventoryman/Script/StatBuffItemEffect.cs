using UnityEngine;
using System.Collections;
using Adefagia.CharacterStats;
using Adefagia.RobotSystem;
using System;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
    public float AttackBuff;
    public float ArmorBuff;
    public float Duration;


    public override void ExecuteEffect(RobotController character)
    {
        character.Robot.IncreaseDamage(AttackBuff);
        character.Robot.IncreaseArmor(ArmorBuff);
        
        VFXController.Instance.PlayBuffLoopVFX(character.gameObject.transform);
    }

    public override string GetDescription()
    {
        if (AttackBuff > 0)
        {
            return "Grants " + AttackBuff + " Attack for " + Duration + " seconds";
        }
        else
        {
            return "Grants " + ArmorBuff + " Armor for " + Duration + " seconds";
        }
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.Attack.RemoveModifier(statModifier);
        character.Armor.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}
