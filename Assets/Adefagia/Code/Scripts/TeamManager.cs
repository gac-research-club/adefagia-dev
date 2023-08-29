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
    // TeamA
    public Team teamA;
    public List<RobotStat> robotsA;

    // TeamB
    public Team teamB;
    public List<RobotStat> robotsB;
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