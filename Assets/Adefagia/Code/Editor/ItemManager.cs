using System;
using System.Collections.Generic;
using System.Linq;
using Adefagia.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class ItemManager : EditorWindow
{
    [MenuItem("MechAI/ItemManager")]
    public static void ShowExample()
    {
        ItemManager wnd = GetWindow<ItemManager>();
        wnd.titleContent = new GUIContent("ItemManager");
    }
    
    [SerializeField] private int m_SelectedIndex = -1;

    private ListView _listView;
    private ScrollView _detailView;
    private EquippableItem _currentItem;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(splitView);

        var buttonAdd = new Button { text = "Add Weapon" };
        root.Add(buttonAdd);

        var equipName = new TextField("Equip Name");
        root.Add(equipName);
        buttonAdd.RegisterCallback<ClickEvent>(_ =>
        {
            var weaponName = equipName.value == "" ? "Weapon" : equipName.value;
            var newEquip = ScriptableObject.CreateInstance<EquippableItem>();
            newEquip.ItemName = weaponName;
            AssetDatabase.CreateAsset(newEquip, $"Assets/Adefagia/Code/Scriptable/{weaponName}.asset");
            AssetDatabase.SaveAssets();

            ShowAllEquipment();
        });
        
        _listView = new ListView();
        splitView.Add(_listView);

        var button1 = new Button() { text = "Refresh" };
        root.Add(button1);
        button1.RegisterCallback<ClickEvent>(evt => ShowAllEquipment());
        

        _detailView = new ScrollView();
        splitView.Add(_detailView);

        ShowAllEquipment();

        _listView.onSelectionChange += objects =>
        {
            _currentItem = (EquippableItem)objects.First();
            if (_currentItem) ShowDetail(_currentItem);
        };
        
        _listView.selectedIndex = m_SelectedIndex;
        
        _listView.onSelectionChange += _ => m_SelectedIndex = _listView.selectedIndex;
    }

    void ShowAllEquipment()
    {
        _listView.Clear();

        var equippableItems = new List<EquippableItem>();
        foreach (var guid in AssetDatabase.FindAssets("t:EquippableItem"))
        {
            equippableItems.Add(AssetDatabase.LoadAssetAtPath<EquippableItem>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        _listView.makeItem = () => new Label();
        _listView.bindItem = (element, i) => ((Label)element).text = equippableItems[i].ItemName;
        _listView.itemsSource = equippableItems;
    }

    void ShowDetail(EquippableItem item)
    {
        _detailView.Clear();

        var path = AssetDatabase.GetAssetPath(item);
        
        var location = new Label($"Location: {path}");
        _detailView.Add(location);
        location.RegisterCallback<ClickEvent>(_ =>
        {
            EditorGUIUtility.PingObject(item);
            Selection.activeObject = item;  
        });

        _detailView.Add(new Label($"Id: {item.ID}"));

        var equipName = new TextField("Name") { value = item.ItemName };
        _detailView.Add(equipName);
        equipName.RegisterValueChangedCallback(evt =>
        {
            item.ItemName = evt.newValue;
        });

        var imageSprite = new Image
        {
            scaleMode = ScaleMode.ScaleToFit,
            sprite = item.Icon
        };
        _detailView.Add(imageSprite);
        if (item.Icon)
        {
            _detailView.Add(new Label($"{item.Icon.texture.width}X{item.Icon.texture.height}"));
        }

        var button = new Button { text = "Change Icon" };
        _detailView.Add(button);
        button.RegisterCallback<ClickEvent>(_ =>
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            EditorGUIUtility.ShowObjectPicker<Sprite>(item.Icon, true, "", controlID);
        });

        var sliderMaxStack = new SliderInt("Maximum Stack", 1, 10) { value = item.MaximumStacks };
        _detailView.Add(sliderMaxStack);
        sliderMaxStack.RegisterValueChangedCallback(evt =>
        {
            item.MaximumStacks = evt.newValue;
            ShowDetail(item);
        });
        
        var maxStack = new IntegerField { value = item.MaximumStacks };
        _detailView.Add(maxStack);
        maxStack.RegisterValueChangedCallback(evt =>
        {
            item.MaximumStacks = evt.newValue;
            // Refresh
            ShowDetail(item);
        });
        
        var attack = new IntegerField("Attack Bonus") { value = item.AttackBonus };
        _detailView.Add(attack);
        attack.RegisterValueChangedCallback(evt => item.AttackBonus = evt.newValue);
        
        var armor = new IntegerField("Armor Bonus") { value = item.ArmorBonus };
        _detailView.Add(armor);
        armor.RegisterValueChangedCallback(evt => item.ArmorBonus = evt.newValue);
        
        var attackBonus = new FloatField("Attack Percent Bonus") { value = item.AttackPercentBonus };
        _detailView.Add(attackBonus);
        attackBonus.RegisterValueChangedCallback(evt => item.AttackPercentBonus = evt.newValue);

        var armorBonus = new FloatField("Armor Percent Bonus") {value = item.ArmorPercentBonus};
        _detailView.Add(armorBonus);
        armorBonus.RegisterValueChangedCallback(evt => item.ArmorPercentBonus = evt.newValue);
        
        var equipment = new EnumField(item.EquipmentType);
        _detailView.Add(equipment);
        equipment.RegisterValueChangedCallback(_ => item.EquipmentType = (EquipmentType)equipment.value);

        var typePattern = new EnumField(item.TypePattern);
        _detailView.Add(typePattern);
        typePattern.RegisterValueChangedCallback(_ => item.TypePattern = (TypePattern)typePattern.value);
    }

    private void OnGUI()
    {
        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            _currentItem.Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            ShowDetail(_currentItem);
        }
    }
}