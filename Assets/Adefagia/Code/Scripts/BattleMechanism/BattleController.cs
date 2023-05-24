using System;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.Code.Scripts.BattleMechanism
{
    public class BattleController : MonoBehaviour
    {
        // event ketika robot mati
        public static event Action<RobotController> OnRobotDead;
        
        // Event ketika robot attack
        public static event Action<RobotController, GridController> OnRobotAttack;

        private void Start()
        {
            // Attack
            OnRobotAttack += RobotAttack.Attack;
            
            // Dead
            // OnRobotDead += RobotDead.Dead;
        }

        public static void InvokeRobotDead(RobotController robotController)
        {
            OnRobotDead?.Invoke(robotController);
        }
        
        public static void InvokeRobotAttack(RobotController robotController, GridController gridController)
        {
            OnRobotAttack?.Invoke(robotController, gridController);
        }

        public static void ClearRobotDeadAction()
        {
            OnRobotDead = null;
        }
        
        /*----------------------------------------------------------------------
         * Click attack button in UI step
         *----------------------------------------------------------------------*/
        
    }
    
}