using System.Collections;
using System.Collections.Generic;
using Adefagia.Inventory;
using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            equipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            equipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            equipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            equipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            equipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            equipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(UsableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == EquipmentType.BuffItem1 && equipmentSlots[i].Item == null)
            {
                equipmentSlots[i].Item = item;
                item.EquipmentType = EquipmentType.BuffItem1;
                return true;
            
            }else if(equipmentSlots[i].EquipmentType == EquipmentType.BuffItem2 && equipmentSlots[i].Item == null){
                
                equipmentSlots[i].Item = item;
                item.EquipmentType = EquipmentType.BuffItem2;
                return true;
            
            }

        }
        return false;
    }

    public bool AddItem(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        return false;
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public List<EquippableItem> ListItem()
    {
        List<EquippableItem> listItem = new List<EquippableItem>();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item && equipmentSlots[i].Item is EquippableItem)
            {
                EquippableItem equipItem = (EquippableItem)equipmentSlots[i].Item;
                listItem.Add(equipItem);
            }
        }
        return listItem;
    }

     public List<UsableItem> ListUsableItem(){
        List<UsableItem> listItem = new List<UsableItem>();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item && equipmentSlots[i].Item is UsableItem)
            {
                UsableItem equipItem = (UsableItem) equipmentSlots[i].Item;
                listItem.Add(equipItem);
            }
        }
        return listItem;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(UsableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}