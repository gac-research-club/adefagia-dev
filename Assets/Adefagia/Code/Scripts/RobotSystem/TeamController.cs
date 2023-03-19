using System;
using System.Collections;
using Adefagia.RobotSystem;
using UnityEngine;

namespace Adefagia.BattleMechanism
{
    public class TeamController : MonoBehaviour
    {
        public Team team;

        public RobotController robotControllerActive;

        private Vector3 _defaultPosition;

        private int _number = 0;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) _number = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) _number = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) _number = 2;
            
            if (BattleManager.preparationState == PreparationState.ChooseRobot)
            {
                if (this == GameManager.instance.battleManager.TeamActive)
                {
                    if (robotControllerActive != team.robotControllers[_number])
                    {
                        if (team.robotControllers[_number] != null)
                        {
                            robotControllerActive = team.robotControllers[_number];
                        }
                    }
                    
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        BattleManager.ChangePreparationState(PreparationState.DeployRobot);
                    }
                }
            }

            if (BattleManager.preparationState == PreparationState.DeployRobot)
            {

                if (robotControllerActive == null) return;

                var robotObject = robotControllerActive.gameObject;
                
                if (_defaultPosition == Vector3.zero)
                {
                    _defaultPosition = robotObject.transform.position;
                }

                try
                {
                    if (GameManager.instance.gridManager.GridHover == null) throw new NullReferenceException();

                    robotObject.transform.position =
                        GameManager.instance.gridManager.GridHover.transform.position;
                }
                catch (NullReferenceException)
                {
                    robotObject.transform.position = _defaultPosition;
                }
            }
        }

        private void ChooseRobot()
        {
            
        }

        private void OnGUI()
        {
            var text = "";
            try
            {
                text = robotControllerActive.name;
            }
            catch (NullReferenceException) { }
            
            GUI.Box (new Rect (0,Screen.height - 50,100,50), text);
        }
    }
}