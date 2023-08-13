using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adefagia.RobotSystem
{
    [Serializable]    
    public class Potion
    {
       
        #region Properties
        
        // Status
        public string Name;

        public List<UsableItemEffect> Effects;
        
        public bool HasUsed { get; set; } 
        // public PotionType skillType;

        #endregion

        public Potion(string name, List<UsableItemEffect> effects)
        {
            Name = name;
            Effects = effects;
           
        }

        public enum PotionType{
            Heal,

            AttackBuff,
            
            DeffendBuff,
        }

        
    }
}