using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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

        [TextArea(maxLines: 10, minLines:0)]
        public string patternAttack;
        
        [TextArea(maxLines: 10, minLines:0)]
        public string patternImpact;

        public Vector2Int origin;

        #endregion

        public enum SkillType{
            Damage,
            Heal,
            AttackBuff,
            DeffendBuff,
        }

        
    }
}