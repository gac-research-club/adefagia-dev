using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.BattleMechanism;
using UnityEngine;
using adefagia.CharacterStats;
using UnityEngine.SceneManagement;


public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

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

        // Add team
        _teams = new List<List<RobotStat>>
        {
            teamManager.robotsA,
            teamManager.robotsB
        };
        
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
    
    // Unity Event
    public void ChangeTeam()
    {
        countTeam++;
        
        // Reset index robot
        indexRobot = 0;
        
        // Make sure 2 team have been selected equipment
        if (countTeam > _teams.Count-1)
        {
            _itemState = ItemState.Finish;
            SceneManager.LoadScene("DimasTesting");
        }
        
    }

    public void ChangeRobotIndex(int index)
    {
        indexRobot = index;
    }
}

public enum ItemState
{
    Initialize,
    Update,
    Finish,
}