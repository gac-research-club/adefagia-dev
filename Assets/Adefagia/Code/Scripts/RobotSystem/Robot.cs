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
        private Grid _grid;
        private float _health;
        private float _stamina;

        #region Constants

        private const float StaminaInitial = 20;
        private const float StaminaRound = 10;

        #endregion

        #region Properties
        
        // Status
        public HealthBar healthBar {get; set;}
        public int ID { get; set; }
        public string Name { get; }
        public Grid Location => _grid;
        public float MaxHealth { get; }
        public float MaxStamina { get; }

        public float CurrentHealth => _health;

        public float CurrentStamina => _stamina;

        public float Damage { get; }
        public float Speed { get; set; }
        
        public float Skill { get; }

        public bool IsDead { get; set; }
        // Step Status
        public bool HasMove { get; set; }
        public bool HasAttack { get; set; }
        public bool HasSkill { get; set; }
        
        #endregion
        

        public Robot(string name)
        {
            Name = name;
            MaxHealth = 30;
            MaxStamina = 50;
            _health = MaxHealth;
            _stamina = StaminaInitial;
            Damage = 10;
            IsDead = false;

            //-----------------------
            ResetStepStat();
        }
        public Robot(string name, float maxHealth, float maxStamina, float damage)
        {
            Name = name;
            MaxHealth = maxHealth;
            MaxStamina = maxStamina;
            _health = MaxHealth;
            _stamina = StaminaInitial;
            Damage = damage;
            IsDead = false;
            
            //-----------------------
            ResetStepStat();
        }
        
        /*-------------------------------------------------------
         * Robot battle methods
         *-------------------------------------------------------*/
        public void TakeDamage(float damage)
        {
            _health -= damage;
            if(_health <= 0){
               IsDead = true;
            }
           
        }

        public void IncreaseStamina()
        {
            _stamina += StaminaRound;
            if(_stamina > MaxStamina) _stamina = MaxStamina;
        }

        public void DecreaseStamina(float stamina)
        {
            _stamina -= stamina;
        }

        /*------------------------------------------------------------------
         * Change location equal grid where he stand
         *------------------------------------------------------------------*/
        public void ChangeLocation(Grid grid)
        {
            _grid = grid;
        }

        public void ResetStepStat()
        {
            HasMove = false;
            HasAttack = false;
            HasSkill = false;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}