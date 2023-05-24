using System;
using Adefagia.BattleMechanism;
using Adefagia.Code.Scripts.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class RobotDead : MonoBehaviour
    {
        private void Start()
        {
            // Dead
            BattleController.OnRobotDead += RobotDead.Dead;
        }

        public static void Dead(RobotController robotController)
        {
            robotController.GridController.Grid.SetFree();
            Debug.Log(robotController.Robot + " Dead");
            // Destroy(robotController.gameObject);
        }
    }
}