using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia
{
    public class EnemyStats : MonoBehaviour
    {
        public string name;
        public float baseHp;
        public float currentHp;
        public float damage;

        public EnemyStats()
        {
            name = "optimus";
            baseHp = 10f;
            currentHp = 10f;
            damage = 2f;
        }

        public bool TakeDamage(float dmg)
        {
            currentHp -= dmg;
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
