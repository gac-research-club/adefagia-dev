using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using UnityEngine;
using UnityEngine.UI;

namespace Adefagia.UI
{
    public class DeployIndexing : MonoBehaviour
    {
        public List<GameObject> hudRobots;


        // Get Team active
        public TeamController TeamActive => BattleManager.TeamActive;
        
        // Text Team active
        public Text textTeamActive;
        
        private void Update()
        {
            // TeamActive.
        }

        public Button GetButton(GameObject gameObjects)
        {
            try
            {
                // Index 1 = button
                return gameObjects.transform.GetChild(1).GetComponent<Button>();
            }
            catch (NullReferenceException err)
            {
                return null;
            }
        }
        
        // Unity Event
        public void ChangeIndex(int index)
        {
            TeamActive.ChooseRobot(index);
        }
    }
}