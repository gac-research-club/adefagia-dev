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

    private void Awake()
    {
        GameManager.instance.gameObject.AddComponent<TeamManager>();
    }
}

[Serializable]
public class RobotStat
{
    public float maxHealth;
    public float maxStamina;
    public float damage;
    public float armor;

    public EquippableItem armorId;
    public EquippableItem helmetId;
    public EquippableItem weaponId;

    public UsableItem buffItem1;

    public UsableItem buffItem2;

}