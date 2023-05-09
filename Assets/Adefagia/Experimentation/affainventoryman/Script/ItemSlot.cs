using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : BaseItemSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

<<<<<<< HEAD

    //To enable and disable, while enable its normalcolor then its disable its disableColor
    private Color normalColor = Color.white;
    private Color disabledColor = new Color(1, 1, 1, 0);

    //Check its item slot on inventory its empty or any item there
    private Item _item;
    public Item Item
=======
    /*
    To enable and disable, while enable its normalcolor then its disable its disableColor
    */
    private Color dragColor = new Color(1, 1, 1, 0.5f);

    public override bool CanAddStack(Item item, int amount = 1)
>>>>>>> 18-feature-highlight-movement-an-bot
    {
        return base.CanAddStack(item, amount) && Amount + amount <= item.MaximumStacks;
    }

    public override bool CanReceiveItem(Item item)
    {
        return true;
    }
<<<<<<< HEAD
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClickEvent != null)
                OnRightClickEvent(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }
=======
>>>>>>> 18-feature-highlight-movement-an-bot

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item != Item)
            image.color = dragColor;

        if (OnBeginDragEvent != null)
            OnBeginDragEvent(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Item != Item)
            image.color = normalColor;

        if (OnEndDragEvent != null)
            OnEndDragEvent(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragEvent != null)
            OnDragEvent(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null)
            OnDropEvent(this);
    }
}
