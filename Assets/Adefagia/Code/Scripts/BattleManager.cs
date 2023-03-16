using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia.Adefgia.Code.Scripts
{
    // public class BattleManager : MonoBehaviour
    // {
    //     public BattlePhase battlePhase;
    //     private Dictionary<SelectType, Select> SelectManager { get; set; }
    //
    //     private Robot.Robot _robot;
    //
    //     private void Start()
    //     {
    //         SelectManager = new Dictionary<SelectType, Select>
    //         {
    //             { SelectType.Grid, GameManager.instance.gridManager.Select.GetSelectComponent() },
    //             { SelectType.Robot, GameManager.instance.robotManager.Select.GetSelectComponent() }
    //         };
    //         
    //         // Disable selecting grid and robot
    //         // DisableAllSelecting();
    //     }
    //
    //     // Update is called once per frame
    //     void Update()
    //     {
    //         Selecting();
    //
    //         if (Input.GetKeyDown(KeyCode.O))
    //         {
    //             // Skip
    //             _robot.ResetDefaultGrid();
    //             _robot = null;
    //             GameManager.instance.robotManager.DeleteRobotSelect();
    //         }
    //     }
    //
    //     private void Selecting()
    //     {
    //         if (GameManager.instance.robotManager.IsRobotSelect())
    //         {
    //             battlePhase = BattlePhase.Move;
    //             if (_robot.IsUnityNull())
    //             {
    //                 _robot = GameManager.instance.robotManager.GetRobotSelect();
    //                 _robot.ClearGridRange();
    //                 _robot.SetGridRange();
    //             }
    //         }
    //         else
    //         {
    //             battlePhase = BattlePhase.Select;
    //         }
    //         
    //         switch (battlePhase)
    //         {
    //             case BattlePhase.Select:
    //                 DisableSelecting(SelectType.Grid);
    //                 EnableSelecting(SelectType.Robot);
    //                 break;
    //                 
    //             case BattlePhase.Move:
    //                 DisableSelecting(SelectType.Robot);
    //                 EnableSelecting(SelectType.Grid);
    //                 break;
    //         }
    //     }
    //
    //     private Select GetSelecting(SelectType selectType)
    //     {
    //         return SelectManager[selectType];
    //     }
    //
    //     private void EnableSelecting(SelectType selectType)
    //     {
    //         GetSelecting(selectType).enabled = true;
    //     }
    //     
    //     private void DisableSelecting(SelectType selectType)
    //     {
    //         GetSelecting(selectType).enabled = false;
    //     }
    //
    //     private void DisableAllSelecting()
    //     {
    //         foreach (var select in SelectManager.Values)
    //         {
    //             select.enabled = false;
    //         }
    //     }
    // }
    //
    // public enum BattlePhase
    // {
    //     Select,
    //     Move,
    //     Attack
    // }
}
