using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using Adefagia.Inventory;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;
using Random = UnityEngine.Random;

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

        public float Damage { get; set; }
        public float TempDamage {get; set;}
        public float Speed { get; set; }
        public float Defend { get; set; }
        public float TempDefend {get; set;}
        
        public bool IsDead { get; set; }
        
        // Pattern Type
        public TypePattern TypePattern { get; set; }
        
        // Step Status
        public bool HasMove { get; set; }
        public bool HasAttack { get; set; }
        public bool HasSkill { get; set; }
        public bool HasEffect { get; set; }

        #endregion

        public event Action Damaged;
        public event Action Dead;

        public Robot(string name)
        {
            Name = name;
            MaxHealth = 100;
            MaxStamina = 50;
            _health = MaxHealth;
            _stamina = StaminaInitial;
            Damage = 15;
            IsDead = false;

            //-----------------------
            ResetStepStat();
        }
        public Robot(string name, float maxHealth, float maxStamina, float damage, float defend)
        {
            Name = name;
            MaxHealth = maxHealth;
            MaxStamina = maxStamina;
            _health = MaxHealth;
            _stamina = StaminaInitial;
            Damage = damage;
            Defend = defend;
            IsDead = false;
            
            //-----------------------
            ResetStepStat();
        }
        
        /*-------------------------------------------------------
         * Robot battle methods
         *-------------------------------------------------------*/
        public void TakeDamage(float damage)
        {
            Damaged?.Invoke();
            
            /* 100 * 0.9 = 90

            /* calculation damage */
            float critical = Random.Range(0.8f, 1.0f);

            /* 30 * 0.3 - 0.5 = 15 
            /* calculation absorption */ 
            float absorption = Random.Range(0.3f, 0.5f);
            float txt1 = (critical * damage);
            float txt2 = (absorption * Defend);

            float TotalDamage = (txt1 - txt2);
            _health -= Math.Abs(TotalDamage);
         
            if(_health <= 0){
               IsDead = true;
               Dead?.Invoke();
               
               GameManager.instance.logManager.AddDeadRobot(this);
            }
           
        }

        public void Healing(float heal)
        {
            _health += heal;
            if(_health >= MaxHealth){
               _health = MaxHealth;
            }
        }

        public void IncreaseDamage(float damage){
            TempDamage = (Damage * damage);
            Damage = Damage + TempDamage;
            
        }
        
        public void IncreaseArmor(float armor){
            TempDefend = (Defend * armor);
            Defend = Defend + TempDefend;

        }
        
        public void Normalize(){
            if(TempDamage > 0){
                Damage = Damage - TempDamage;
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