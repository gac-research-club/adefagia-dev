using UnityEngine;
using System.Collections.Generic;
using Adefagia.CharacterStats;
using Adefagia.RobotSystem;

namespace Adefagia.Inventory
{
    public enum EquipmentType
    {
        Top,
        Body,
        Weapon,
        BuffItem1,
        BuffItem2,
    }

    public enum TypePattern
    {
        Surround,
        Diamond,
        SmallDiamond,
        Cross,
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

        public TypePattern TypePattern;

        // Skill Type 
        public List<Skill> WeaponSkill;

        public override Item GetCopy()
        {
            // Generate this prefab
            return Instantiate(this);
        }

        public override void Destroy()
        {
            Destroy(this);
        }

        public void Equip(Character character)
        {
            if (AttackBonus != 0)
            {
                character.Attack.AddModifier(new StatModifier(AttackBonus, StatModType.Flat, this));
            }

            if (ArmorBonus != 0)
            {
                character.Armor.AddModifier(new StatModifier(ArmorBonus, StatModType.Flat, this));
            }

            if (AttackPercentBonus != 0)
            {
                character.Attack.AddModifier(new StatModifier(AttackPercentBonus, StatModType.PercentMult, this));
            }

            if (ArmorPercentBonus != 0)
            {
                character.Armor.AddModifier(new StatModifier(ArmorPercentBonus, StatModType.PercentMult, this));
            }
        }

        public void UnEquip(Character character)
        {
            character.Attack.RemoveAllModifiersFromSource(this);
            character.Armor.RemoveAllModifiersFromSource(this);
        }

        public override string GetItemType()
        {
            return EquipmentType.ToString();
        }


        public override string GetDescription()
        {
            Sb.Length = 0;
            AddStat(AttackBonus, "Attack");
            AddStat(ArmorBonus, "Armor");

            AddStat(AttackPercentBonus, "Attack", isPercent: true);
            AddStat(ArmorPercentBonus, "Armor", isPercent: true);

            return Sb.ToString();
        }

        private void AddStat(float value, string statName, bool isPercent = false)
        {
            if (value != 0)
            {
                if (Sb.Length > 0)
                    Sb.AppendLine();

                if (value > 0)
                    Sb.Append("+");

                if (isPercent)
                {
                    Sb.Append(value * 100);
                    Sb.Append("% ");
                }
                else
                {
                    Sb.Append(value);
                    Sb.Append(" ");
                }
                Sb.Append(statName);
            }
        }
    }
}