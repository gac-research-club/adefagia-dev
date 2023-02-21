using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia
{
    public class RobotStatus : MonoBehaviour
    {

        public Robot.Robot Robot { get; set; }

        private void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Robot.IsHover)
            {
            }
            else if (Robot.IsSelect)
            {
            }
            else
            {
            }
        }
    }
}
