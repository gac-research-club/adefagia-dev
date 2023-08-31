using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Adefagia;
using Adefagia.BattleMechanism;
using Adefagia.CharacterStats;
using Adefagia.Inventory;
using Adefagia.RobotSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public int Health = 100;

    public CharacterStat Attack;
    public CharacterStat Armor;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    public static event Action<int> PrevNextEvent;
    public static event Action<int> ChangeTeamEvent;

    private List<EquippableItem> listItem;
    private List<UsableItem> listUsableItem;

    private BaseItemSlot dragItemSlot;
    private int _currentIndex;

    private TeamManager _teamManager;
    

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
        _teamManager = GameManager.instance.teamManager;

        // Robots A
        foreach (var robot in _teamManager.robotsA)
        {
            robot.maxHealth = Health;
        }

        // Robots B
        foreach (var robot in _teamManager.robotsB)
        {
            robot.maxHealth = Health;
        }

        // set variable
        listItem = new List<EquippableItem>();
    }

    private void InventoryRigthClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem) itemSlot.Item);
            
        }
        else if (itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem) itemSlot.Item;
            if (equipmentPanel.AddItem(usableItem))
            {
                statPanel.UpdateUsableItemId(usableItem, usableItem.EquipmentType.ToString());
            } 
        }
    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem) itemSlot.Item);
        }else if(itemSlot.Item is UsableItem){

            Unequip((UsableItem) itemSlot.Item);
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
        EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
        EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) dragEquipItem.UnEquip(this);
            if (dropEquipItem != null) dropEquipItem.Equip(this);
        }

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) dragEquipItem.Equip(this);
            if (dropEquipItem != null) dropEquipItem.UnEquip(this);
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
                    previousItem.UnEquip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);

                try
                {
                    statPanel.UpdateEquipmentId(item, item.GetItemType());
                }
                catch (System.Exception)
                {
                }

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
            item.UnEquip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
            statPanel.UnequipId(item.GetItemType());
        }
    }

    public void Unequip(UsableItem item)
    {
        if (equipmentPanel.RemoveItem(item))
        {
            statPanel.UnequipId(item.GetItemType());
        }
    }

    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
    }

    public void ChangeTeam()
    {
        _currentIndex = 0;
        ChangeTeamEvent?.Invoke(0);
        
        Team team = statPanel.GetCurrentTeam();

        List<Dictionary<String, EquippableItem>> listEquipRobot = statPanel.GetDetailEquipmentTeam(team);
        List<Dictionary<String, UsableItem>> listUsableItem = statPanel.GetDetailItemTeam(team);

        statPanel.ChangeTeam();

        foreach (Dictionary<String, EquippableItem> equipRobot in listEquipRobot)
        {
            if (equipRobot["armorId"] is EquippableItem && inventory.CanAddItem(equipRobot["armorId"]))
            {
                EquippableItem armorEquip = (EquippableItem)equipRobot["armorId"];
                if (equipmentPanel.RemoveItem(armorEquip))
                {
                    armorEquip.UnEquip(this);
                    statPanel.UpdateStatValues();
                };
                inventory.AddItem(equipRobot["armorId"]);
            }

            if (equipRobot["weaponId"] is EquippableItem && inventory.CanAddItem(equipRobot["weaponId"]))
            {
                EquippableItem weaponEquip = (EquippableItem)equipRobot["weaponId"];
                if (equipmentPanel.RemoveItem(weaponEquip))
                {
                    weaponEquip.UnEquip(this);
                    statPanel.UpdateStatValues();
                };
                inventory.AddItem(equipRobot["weaponId"]);
            }

            if (equipRobot["helmetId"] is EquippableItem && inventory.CanAddItem(equipRobot["helmetId"]))
            {
                EquippableItem helmetEquip = (EquippableItem)equipRobot["helmetId"];
                if (equipmentPanel.RemoveItem(helmetEquip))
                {
                    helmetEquip.UnEquip(this);
                    statPanel.UpdateStatValues();
                };
                inventory.AddItem(equipRobot["helmetId"]);
            }
        }
        
        foreach (Dictionary<String, UsableItem> itemRobot in listUsableItem)
        {
            if (itemRobot["itemBuff1"] is UsableItem)
            {
                
                if(equipmentPanel.RemoveItem(itemRobot["itemBuff1"])){
                    statPanel.UpdateStatValues();
                }
            }

            if (itemRobot["itemBuff2"] is UsableItem)
            {
                if(equipmentPanel.RemoveItem(itemRobot["itemBuff2"])){
                    statPanel.UpdateStatValues();
                }
            }

        }

        _teamManager.SaveToJson();
    }


    public void ChangeRobotIndex(int index)
    {
        listItem = equipmentPanel.ListItem();
        listUsableItem = equipmentPanel.ListUsableItem();
        
        Dictionary<String, EquippableItem> statRobot = statPanel.GetDetailEquipment(index);
        Dictionary<String, UsableItem> itemRobot = statPanel.GetDetailItemEquip(index);

        statPanel.ChangeRobotIndex(index);

        foreach (EquippableItem item in listItem)
        {
            if (item is EquippableItem)
            {
                
                EquippableItem itemEq = (EquippableItem) item;
                if (inventory.CanAddItem(itemEq) && equipmentPanel.RemoveItem(itemEq))
                {
                    itemEq.UnEquip(this);
                    statPanel.UpdateStatValues();
                    // inventory.AddItem(item);
                }
            }
        }

        foreach (UsableItem itemUs in listUsableItem)
        {
            if (itemUs is UsableItem)
            {
                
                UsableItem itemUsable = (UsableItem) itemUs;
                if (inventory.CanAddItem(itemUsable) && equipmentPanel.RemoveItem(itemUsable))
                {            
                    statPanel.UpdateStatValues();
                }
            }
        }

        if (statRobot["armorId"] is EquippableItem)
        {

            EquippableItem itemCur = (EquippableItem)statRobot["armorId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();
        }

        if (statRobot["weaponId"] is EquippableItem)
        {
            EquippableItem itemCur = (EquippableItem)statRobot["weaponId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();
        }

        if (statRobot["helmetId"] is EquippableItem)
        {

            EquippableItem itemCur = (EquippableItem)statRobot["helmetId"];

            equipmentPanel.AddItem(itemCur);
            itemCur.Equip(this);
            statPanel.UpdateStatValues();
        }

        if (itemRobot["itemBuff1"] is UsableItem)
        {
            UsableItem itemCur = (UsableItem) itemRobot["itemBuff1"];

            equipmentPanel.AddItem(itemCur);
            statPanel.UpdateStatValues();          
        }


        if (itemRobot["itemBuff2"] is UsableItem)
        {
            UsableItem itemCur = (UsableItem) itemRobot["itemBuff2"];

            equipmentPanel.AddItem(itemCur);
            statPanel.UpdateStatValues();          
        }
    }

    public void NextRobot()
    {
        _currentIndex++;
        ChangeRobotIndex(_currentIndex);
        PrevNextEvent?.Invoke(_currentIndex);
    }

    public void PreviousRobot()
    {
        _currentIndex--;
        ChangeRobotIndex(_currentIndex);
        PrevNextEvent?.Invoke(_currentIndex);
    }
}