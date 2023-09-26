using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMoveTest : RobotAction
{
    private RobotData RobotData;
    private bool _isRunning;
    
    public override void OnStart()
    {
        Debug.Log("OnStart");
        _isRunning = true;
    }

    public override void OnRunning()
    {
        if (!_isRunning) return;
        Debug.Log($"OnRunning; Stamina : {--RobotData.stamina}");
    }

    public override void OnStop()
    {
        Debug.Log("OnStop");
        _isRunning = false;
    }

    public override void SetRobot(RobotData robotData)
    {
        RobotData = robotData;
    }
}
