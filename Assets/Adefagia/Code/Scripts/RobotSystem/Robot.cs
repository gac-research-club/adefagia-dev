using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.SelectObject;
using Adefagia.Collections;

namespace Adefagia.RobotSystem
{
    public class Robot
    {
        public Grid Grid { get; set; }
        
        public float MaxHealth { get; }
        public float CurrentHealth { get; }
        public float Damage { get; }

        public Robot(float maxHealth, float damage)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            Damage = damage;
        }
    }
}