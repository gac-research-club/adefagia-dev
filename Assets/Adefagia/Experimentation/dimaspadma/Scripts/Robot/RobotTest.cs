using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTest : MonoBehaviour
{
    private RobotData Data;
    private WeaponTest weapon;
    
    private RobotAction robotAction;

    private void Start()
    {
        Data = ScriptableObject.CreateInstance<RobotData>();
        Data.stamina = 100;
        
        robotAction = new RobotMoveTest();
        robotAction.SetRobot(Data);
    }

    private void Update()
    {
        robotAction.OnRunning();
    }

    private void Move()
    {
        robotAction.OnStart();
    }

    public virtual void SetWeapon<T>(T weaponTest) where T: WeaponTest
    {
        // if()
        weapon = weaponTest;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));
        if (GUILayout.Button("Attack")) Move();
        if (GUILayout.Button("Stop Attack")) robotAction.OnStop();
        GUILayout.EndArea();
    }
}

public class RobotData : ScriptableObject
{
    public int robotId;
    public string name;
    public int stamina;
}

public class WeaponTest : ScriptableObject
{
    public string range;
    public string weaponName;
}