using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using System.Collections;
using System.Collections.Generic;

namespace Adefagia.RobotSystem
{
    public class SkillController : MonoBehaviour
    {
        
        public List<Skill> Skills { get; set; } 

        private void Awake(){
            Skills = new List<Skill>();
        }
        
        public Skill ChooseSkill(int type){
            return Skills[type];
        }
    }
}