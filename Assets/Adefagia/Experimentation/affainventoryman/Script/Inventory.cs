using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Inventory : ItemContainer
{
    [SerializeField] private Item[] startingItems;
    [SerializeField] private Transform itemsParent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start()
    {
        foreach (var itemSlot in itemSlots)
        {
            itemSlot.OnPointerEnterEvent += slot => OnPointerEnterEvent?.Invoke(slot);
            itemSlot.OnPointerExitEvent  += slot => OnPointerExitEvent(slot);
            itemSlot.OnRightClickEvent   += slot => OnRightClickEvent(slot);
            itemSlot.OnBeginDragEvent    += slot => OnBeginDragEvent(slot);
            itemSlot.OnEndDragEvent      += slot => OnEndDragEvent(slot);
            itemSlot.OnDragEvent         += slot => OnDragEvent(slot);
            itemSlot.OnDropEvent         += slot => OnDropEvent(slot);
        }

        SetStartingItems();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }
        
        SetStartingItems();
    }

    private void SetStartingItems()
    {
        Clear();
        foreach (var startingItem in startingItems)
        {
            AddItem(startingItem.GetCopy());
        }
    }
}
