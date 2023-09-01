using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.Inventory;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int totalRobot;
    
    // TeamA
    public Team teamA;
    public List<RobotStat> robotsA;

    // TeamB
    public Team teamB;
    public List<RobotStat> robotsB;

    public Team currentTeam;

    public List<RobotStat> robots = new List<RobotStat>();

    public void SaveToJson()
    {
        string jsonTeamManager = JsonUtility.ToJson(this);
        Debug.Log(jsonTeamManager);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/TeamManager.json", jsonTeamManager);
    }

    public List<RobotStat> GetRobots(Team team)
    {
        if (team == teamA)
        {
            return robotsA;
        }

        return robotsB;
    }
}

[Serializable]
public class RobotStat
{

    const float MAX_STAMINA = 50;
    const float MAX_HEALTH = 100;

    public string name;

    public float maxHealth = MAX_HEALTH;
    public float maxStamina = MAX_STAMINA;
    public float damage;
    public float armor;

    public EquippableItem armorId;
    public EquippableItem helmetId;
    public EquippableItem weaponId;

    public UsableItem buffItem1;

    public UsableItem buffItem2;

}