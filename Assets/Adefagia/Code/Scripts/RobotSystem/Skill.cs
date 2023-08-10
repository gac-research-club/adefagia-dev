using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Adefagia.Inventory;
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

        public TypePattern PatternAttack;
        
        public TypePattern PatternImpact;

        #endregion

        public enum SkillType{
            Damage,
            Heal,
            AttackBuff,
            DeffendBuff,
        }

        
    }
}