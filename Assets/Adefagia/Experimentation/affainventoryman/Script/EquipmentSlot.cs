using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : BaseItemSlot
{
    public EquipmentType EquipmentType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = EquipmentType.ToString() + " Slot";
    }

    //For equipment slot its empty to replace with equippable item
    public override bool CanReceiveItem(Item item)
    {
        if (item == null)
            return true;

        EquippableItem equippableItem = item as EquippableItem;
        return equippableItem != null && equippableItem.EquipmentType == EquipmentType;
    }


}
