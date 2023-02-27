using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia.PlayerAction
{
    public class DefendAction : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private EnemyStats enemyStats;
        public void DefendButtonOnClicked()
        {
            playerStats.Heal(playerStats.healAmount);
        }
    }
}
