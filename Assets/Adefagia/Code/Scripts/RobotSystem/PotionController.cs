using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Adefagia.RobotSystem
{
    public class PotionController : MonoBehaviour
    {
        
        public List<Potion> Potions { get; set; } 

        private void Awake(){
            Potions = new List<Potion>();
        }
        
        public Potion ChoosePotion(int type){
            return Potions[type];
        }

    }
}