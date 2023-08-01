using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.Inventory;
using Adefagia.BattleMechanism;
using UnityEngine;
using Adefagia.CharacterStats;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    [SerializeField] Text characterName;

    [SerializeField] private Button buttonBattle;

    private CharacterStat[] stats;

    private List<RobotStat> robotSelected;
    private static int indexRobot = 0;

    private TeamManager teamManager;
    private List<List<RobotStat>> _teams;
    private static int countTeam = 0;

    private ItemState _itemState = ItemState.Initialize;


    private void Start()
    {
        // Initiate Robots
        teamManager = GameManager.instance.gameObject.GetComponent<TeamManager>();

        // Initial Name 
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName();


        // Add team
        _teams = new List<List<RobotStat>>
        {
            teamManager.robotsA,
            teamManager.robotsB
        };

    }

    public String GetTeamName()
    {
        if (countTeam == 0)
        {
            return teamManager.teamA.teamName;
        }
        else
        {
            return teamManager.teamB.teamName;
        }
    }

    private void Update()
    {
        if (_itemState == ItemState.Update)
        {
            // Update damage value
            _teams[countTeam][indexRobot].damage = statDisplays[0].Stat.Value;

            // Update armor value
            _teams[countTeam][indexRobot].armor = statDisplays[1].Stat.Value;

        }

    }

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();

        Debug.Log("OnValidate");
        _itemState = ItemState.Update;

        UpdateStatNames();
    }

    // charStats = Armor, Attack
    public void SetStats(params CharacterStat[] charStats)
    {
        stats = charStats;

        // below stat display length
        if (stats.Length > statDisplays.Length)
        {
            Debug.LogError("Not Enough Stat Display!");
            return;
        }

        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < stats.Length);

            if (i < stats.Length)
            {
                statDisplays[i].Stat = stats[i];
            }
        }
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].UpdateStatValue();
        }
    }

    public void UpdateStatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].Name = statNames[i];
        }
    }
    public void UpdateUsableItemId(UsableItem item, string type){    
        if(type == EquipmentType.BuffItem1.ToString()){

            // Update Buff Item 1 id
            _teams[countTeam][indexRobot].buffItem1 = item;
            
        }else if(type == EquipmentType.BuffItem2.ToString()){

            // Update Buff Item 2 id
            _teams[countTeam][indexRobot].buffItem2 = item;
        }    
    }

    public void UpdateEquipmentId(EquippableItem item, string type){    
        if(type == EquipmentType.Top.ToString()){
            
            // Update helmet id
            _teams[countTeam][indexRobot].helmetId = item;

        }
        else if (type == EquipmentType.Body.ToString())
        {

            // Update armor id
            _teams[countTeam][indexRobot].armorId = item;

        }
        else if (type == EquipmentType.Weapon.ToString())
        {

            // Update weapon id
            _teams[countTeam][indexRobot].weaponId = item;
        } 
    }

    

    public void UnequipId(string type){    
        if(type == EquipmentType.Top.ToString()){
            
            // Update helmet id
            _teams[countTeam][indexRobot].helmetId = null;

        }
        else if (type == EquipmentType.Body.ToString())
        {

            // Update armor id
            _teams[countTeam][indexRobot].armorId = null;

        }
        else if (type == EquipmentType.Weapon.ToString())
        {

            // Update weapon id
            _teams[countTeam][indexRobot].weaponId = null;
        }
    }

    // Unity Event
    public void ChangeTeam()
    {
        countTeam++;
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName();

        // Reset index robot
        indexRobot = 0;

        // Make sure 2 team have been selected equipment
        if (countTeam > _teams.Count - 2)
        {
            _itemState = ItemState.Finish;
            buttonBattle.gameObject.SetActive(true);
        }

    }

    public int GetCurrentTeam()
    {
        return countTeam;
    }

    public void ChangeRobotIndex(int index)
    {
        indexRobot = index;
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName();
    }

    public Dictionary<String, EquippableItem> GetDetailEquipment(int index)
    {
        Dictionary<String, EquippableItem> statRobot = new Dictionary<String, EquippableItem>();

        statRobot.Add("helmetId", _teams[countTeam][index].helmetId);
        statRobot.Add("armorId", _teams[countTeam][index].armorId);
        statRobot.Add("weaponId", _teams[countTeam][index].weaponId);

        return statRobot;
    }

    public Dictionary<String, UsableItem> GetDetailItemEquip(int index){
        Dictionary<String, UsableItem> statRobot = new Dictionary<String, UsableItem>();
        
        statRobot.Add("itemBuff1", _teams[countTeam][index].buffItem1);
        statRobot.Add("itemBuff2", _teams[countTeam][index].buffItem2);

        return statRobot;
    }

    public List<Dictionary<String, EquippableItem>> GetDetailEquipmentTeam(int count){
        
        List<Dictionary<String, EquippableItem>> listStatRobot = new List<Dictionary<String, EquippableItem>>();

        for (int index = 0; index < 3; index++)
        {
            Dictionary<String, EquippableItem> statRobot = new Dictionary<String, EquippableItem>();

            statRobot.Add("helmetId", _teams[count][index].helmetId);
            statRobot.Add("armorId", _teams[count][index].armorId);
            statRobot.Add("weaponId", _teams[count][index].weaponId);

            listStatRobot.Add(statRobot);
        }

        return listStatRobot;
    }

    public List<Dictionary<String, UsableItem>> GetDetailItemTeam(int count){
        
        List<Dictionary<String, UsableItem>> listStatRobot = new List<Dictionary<String, UsableItem>>();
        
        for (int index = 0; index < 3; index++)
        {
            Dictionary<String, UsableItem> statRobot = new Dictionary<String, UsableItem>();
            
            statRobot.Add("itemBuff1", _teams[count][index].buffItem1);
            statRobot.Add("itemBuff2", _teams[count][index].buffItem2);

            listStatRobot.Add(statRobot);
        }

        return listStatRobot;
    }

}

public enum ItemState
{
    Initialize,
    Update,
    Finish,
}