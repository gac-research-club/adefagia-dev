using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotAction
{
    public abstract void SetRobot(RobotData robotData);
    public abstract void OnStart();
    public abstract void OnRunning();
    public abstract void OnStop();
}
