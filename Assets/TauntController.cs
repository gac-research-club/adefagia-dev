using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adefagia.BattleMechanism;
using Adefagia;
using Adefagia.RobotSystem;
public class TauntController : MonoBehaviour
{
    TeamController TeamActive => BattleManager.TeamActive;
    private void Update()
    {
        Debug.Log(TeamActive.robotControllers.Count);
    }
    public void Taunting()
    {
        foreach(RobotController robot in TeamActive.robotControllers)
        {
            robot.gameObject.GetComponentInChildren<EmotScript>().ShowEmot();
        }
    }
}
