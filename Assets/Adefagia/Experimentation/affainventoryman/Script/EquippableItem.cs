using UnityEngine;
using adefagia.CharacterStats;

public enum EquipmentType
{
    Top,
    Body,
    Weapon,
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{
    // Item stats
    public int AttackBonus;
    public int ArmorBonus;
    [Space]
    public float AttackPercentBonus;
    public float ArmorPercentBonus;
    [Space]
    
    // Item Type
    public EquipmentType EquipmentType;

    public override Item GetCopy()
    {
        // Generate this prefab
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(Character c)
    {
        if (AttackBonus != 0)
            c.Attack.AddModifier(new StatModifier(AttackBonus, StatModType.Flat, this));
        if (ArmorBonus != 0)
            c.Armor.AddModifier(new StatModifier(ArmorBonus, StatModType.Flat, this));

        if (AttackPercentBonus != 0)
            c.Attack.AddModifier(new StatModifier(AttackPercentBonus, StatModType.PercentMult, this));
        if (ArmorPercentBonus != 0)
            c.Armor.AddModifier(new StatModifier(ArmorPercentBonus, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.Attack.RemoveAllModifiersFromSource(this);
        c.Armor.RemoveAllModifiersFromSource(this);
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        AddStat(AttackBonus, "Attack");
        AddStat(ArmorBonus, "Armor");

        AddStat(AttackPercentBonus, "Attack", isPercent: true);
        AddStat(ArmorPercentBonus, "Armor", isPercent: true);

        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+");
            
            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            } 
            else 
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }
}
