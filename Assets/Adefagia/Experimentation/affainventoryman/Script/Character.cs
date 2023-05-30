using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Adefagia;
using Adefagia.BattleMechanism;
using adefagia.CharacterStats;
using Adefagia.RobotSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public int Health  = 100;

    public CharacterStat Attack;
    public CharacterStat Armor;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private List<Item> listItem;

    private BaseItemSlot dragItemSlot;

    // Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    protected void OnValidate()
    {
        if (itemTooltip == null)
        {
            // Find ItemTooltip
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Start()
    {
        // Set Status
        statPanel.SetStats(Attack, Armor);
        statPanel.UpdateStatValues();

        //Setup Event
        inventory.OnRightClickEvent += InventoryRigthClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;

        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;

        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        craftingWindow.OnPointerExitEvent += HideTooltip;

        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;

        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;

        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;

        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
        
        // Starting robot maxHealth value
        var teamManager =  GameManager.instance.GetComponent<TeamManager>();
        
        // Robots A
        foreach (var robot in teamManager.robotsA)
        {
            robot.maxHealth = Health;
        }
        
        // Robots B
        foreach (var robot in teamManager.robotsB)
        {
            robot.maxHealth = Health;
        }

        // set variable
        listItem = new List<Item>();
    }

    private void InventoryRigthClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
            
        }
        else if (itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(this);

            if (usableItem.IsConsumable)
            {
                inventory.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        if (itemTooltip.gameObject.activeSelf)
        {
            itemTooltip.HideTooltip();
        }
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.gameObject.SetActive(true);
        }
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        draggableItem.transform.position = Input.mousePosition;
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (dragItemSlot == null) return;

        if (dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }       
        else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
    }

    public void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItem dragEquipItem  = dragItemSlot.Item as EquippableItem;
        EquippableItem dropEquipItem  = dropItemSlot.Item as EquippableItem;

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragEquipItem  != null) dragEquipItem .Unequip(this);
            if (dropEquipItem  != null) dropEquipItem .Equip(this);
        }

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragEquipItem  != null) dragEquipItem .Equip(this);
            if (dropEquipItem  != null) dropEquipItem .Unequip(this);
        }
        statPanel.UpdateStatValues();

        Item draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    public void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

        dragItemSlot.Amount -= stacksToAdd;
        dropItemSlot.RemoveStack(stacksToAdd); 
    }

    //Check items is Equippable item or not from inventory to place on equipment panel
    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateEquipmentId(item, item.GetItemType());
                statPanel.UpdateStatValues();
            } 
            else 
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {        
        if (inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {            
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
            statPanel.UnequipId(item.GetItemType());
        }
    }

    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
    }

    public void ChangeTeam()
    {
        // listItem = equipmentPanel.ListItem();
        
        // foreach (Item item in listItem)
        // {
        //     if (item is EquippableItem)
        //     {
                
        //         EquippableItem itemEq = (EquippableItem) item;

                
        //         if (inventory.CanAddItem(itemEq) && equipmentPanel.RemoveItem(itemEq))
        //         {            
        //             itemEq.Unequip(this);
        //             statPanel.UpdateStatValues();
        //             // inventory.AddItem(item);
        //         }
        //     }    
        // }
        statPanel.ChangeTeam();
        // inventory.AddItem(item);
    }

    public void ChangeRobotIndex(int index)
    {
        listItem = equipmentPanel.ListItem();
        Dictionary<String, Item> statRobot = statPanel.GetDetailEquipment(index);
        statPanel.ChangeRobotIndex(index);

        foreach (Item item in listItem)
        {
            if (item is EquippableItem)
            {
                
                EquippableItem itemEq = (EquippableItem) item;

                
                if (inventory.CanAddItem(itemEq) && equipmentPanel.RemoveItem(itemEq))
                {            
                    itemEq.Unequip(this);
                    statPanel.UpdateStatValues();
                    // inventory.AddItem(item);
                }
            }    
        }

        if (statRobot["armorId"] is EquippableItem)
        {
    
            EquippableItem itemCur = (EquippableItem) statRobot["armorId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();          
        }

        if (statRobot["weaponId"] is EquippableItem)
        {
            EquippableItem itemCur = (EquippableItem) statRobot["weaponId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();          
        }

        if (statRobot["helmetId"] is EquippableItem)
        {

            EquippableItem itemCur = (EquippableItem) statRobot["helmetId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();          
        }

         
    }
}
