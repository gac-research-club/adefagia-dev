using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace adefagia
{
    public class PlayerStats : MonoBehaviour
    {
        public string name;
        public float baseHp;
        public float currentHp;
        public float damage;

        public PlayerStats()
        {
            name = "Megatron";
            baseHp = 10f;
            currentHp = 10f;
            damage = 2f;
        }

        public bool TakeDamage(float dmg)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Heal(float amount)
        {
            currentHp += amount;
            if (currentHp > baseHp)
            {
                currentHp = baseHp;
            }
        }

    }

}
