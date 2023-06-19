using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.Inventory;

[CreateAssetMenu(menuName = "Items/Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable;

    public EquipmentType EquipmentType;
    
    public List<UsableItemEffect> Effects;

    public virtual void Use(Character character)
    {
        foreach (UsableItemEffect effect in Effects)
        {   
            effect.ExecuteEffect(this, character);
        }
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Comsumable" : "Usable";
    }
    
    public override string GetDescription()
    {
        Sb.Length = 0;

        foreach (UsableItemEffect effect in Effects)
        {
            Sb.AppendLine(effect.GetDescription());
        }

        return Sb.ToString();
    }
}
