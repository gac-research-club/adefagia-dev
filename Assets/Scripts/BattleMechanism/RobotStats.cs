using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia
{
    public class RobotStats : MonoBehaviour
    {
        public string robotName;
        public float baseHp;
        public float currentHp;
        public float damage;
        public float healAmount;

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
