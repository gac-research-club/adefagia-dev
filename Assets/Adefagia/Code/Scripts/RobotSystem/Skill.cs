using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adefagia.RobotSystem
{
    [Serializable]    
    public class Skill
    {
       
        #region Properties
        
        // Status
        public string Name;
        
        public float StaminaRequirement;
        
        public float Value;

        public bool IsUltimate;

        public SkillType skillType;

        #endregion

        public enum SkillType{
            Damage,
            Heal,
            AttackBuff,
            DeffendBuff,
        }

        
    }
}