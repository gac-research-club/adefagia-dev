using System.Collections.Generic;
using adefagia.Collections;
using adefagia.Graph;
using UnityEngine;

namespace adefagia
{
    public class BattleManager : MonoBehaviour
    {
        public BattlePhase battlePhase;
        private Dictionary<SelectType, Select> SelectManager { get; set; }

        private void Start()
        {
            SelectManager = new Dictionary<SelectType, Select>
            {
                { SelectType.Grid, GameManager.instance.gridManager.Select.GetSelectComponent() },
                { SelectType.Robot, GameManager.instance.robotManager.Select.GetSelectComponent() }
            };
            
            // Disable selecting grid and robot
            // DisableAllSelecting();
        }

        // Update is called once per frame
        void Update()
        {
            Battle();
        }

        private void Battle()
        {
            if (GameManager.instance.robotManager.IsRobotSelect())
            {
                battlePhase = BattlePhase.SelectingGrid;
            }
            
            switch (battlePhase)
            {
                case BattlePhase.SelectingRobot:
                    DisableSelecting(SelectType.Grid);
                    // EnableSelecting(SelectType.Robot);
                    break;
                    
                case BattlePhase.SelectingGrid:
                    DisableSelecting(SelectType.Robot);
                    EnableSelecting(SelectType.Grid);
                    break;
            }
        }

        private Select GetSelecting(SelectType selectType)
        {
            return SelectManager[selectType];
        }

        private void EnableSelecting(SelectType selectType)
        {
            GetSelecting(selectType).enabled = true;
        }
        
        private void DisableSelecting(SelectType selectType)
        {
            GetSelecting(selectType).enabled = false;
        }

        private void DisableAllSelecting()
        {
            foreach (var select in SelectManager.Values)
            {
                select.enabled = false;
            }
        }
    }

    public enum BattlePhase
    {
        SelectingRobot,
        SelectingGrid
    }
}
