using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable;

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
        sb.Length = 0;

        foreach (UsableItemEffect effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        }

        return sb.ToString();
    }
}
