using System;
using System.Collections.Generic;
using UnityEngine;

using Adefagia.RobotSystem;

namespace Adefagia.SelectObject
{
    public class SelectRobotManager : MonoBehaviour
    {
        public int index;
        public List<GameObject> selectRobots;
        public SelectRobot selectRobotActive;
        public GameObject robotSelect;

        public OutlineScriptableObject[] outlineStyles;
        
        private void Start()
        {
            selectRobots = new List<GameObject>();
            
            CreateRobotSelect(5);
            ChangeIndex(0);
        }
        
        private void Update()
        {
            // TODO: Key 1,2,3,4,5 for change index
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeIndex(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeIndex(1);
            }
        }
        
        
        /*-------------------------------------------------------------------------
         * Create robot select
         *-------------------------------------------------------------------------*/
        public void CreateRobotSelect(int size = 1)
        {
            if (size < 1) return;
            for (int i = 0; i < size; i++)
            {
                var selectRobot = new GameObject
                {
                    transform = { parent = transform },
                    name = "SelectObject " + i,
                };
                selectRobot.AddComponent<SelectRobot>();
                selectRobots.Add(selectRobot);
            }
        }

        /*-------------------------------------------------------------------------
         * change SelectRobot active
         *-------------------------------------------------------------------------*/
        public void ChangeIndex(int i)
        {
            if (i < 0 || i >= selectRobots.Count) return;
            selectRobotActive = selectRobots[i].GetComponent<SelectRobot>();
        }

        /*----------------------------------------------------------------------------
         * Delegate UnityEvent for Select
         *----------------------------------------------------------------------------*/
        
    }
}