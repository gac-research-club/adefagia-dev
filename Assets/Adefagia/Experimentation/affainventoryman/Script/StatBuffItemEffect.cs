using UnityEngine;
using System.Collections;
using Adefagia.CharacterStats;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
    public int AttackBuff;
    public int ArmorBuff;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        StatModifier statModifierAttack = new StatModifier(AttackBuff, StatModType.Flat, parentItem);
        character.Attack.AddModifier(statModifierAttack);

        StatModifier statModifierArmor = new StatModifier(ArmorBuff, StatModType.Flat, parentItem);
        character.Armor.AddModifier(statModifierArmor);

        character.StartCoroutine(RemoveBuff(character, statModifierAttack, Duration));
        character.StartCoroutine(RemoveBuff(character, statModifierArmor, Duration));

        character.UpdateStatValues();
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
