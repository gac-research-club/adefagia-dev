using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.SelectObject;
using Adefagia.Collections;

namespace Adefagia.RobotSystem
{
    public class Skill
    {
       
        #region Properties
        
        // Status
        public string Name { get; }
        
        public float StaminaReq { get; }
        
        public float Damage { get; }

        #endregion

        public Skill(string name, float staminaReq, float damage){
            Name = name;
            StaminaReq = staminaReq;
            Damage = damage;
        }

    }
}