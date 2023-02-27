using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia.PlayerAction
{
    public class AttackAction : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private EnemyStats enemyStats;
        public void AttackButtonOnClicked()
        {
            bool isDead = enemyStats.TakeDamage(playerStats.damage);

            if (isDead)
            {
                Debug.Log("Kamu mati!");
            }

        }
    }
}
