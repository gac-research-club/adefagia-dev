using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RobotSelectedListener : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text hpText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text defendText;
    
    
    private void Start()
    {
        BattleManager.SelectRobot += UpdateStatus;
    }

    private void UpdateStatus(Robot robot)
    {
        hpSlider.maxValue = robot.MaxHealth;
        hpSlider.value = robot.CurrentHealth;
        
        // Text HP
        var templateHP = "HP: {0}/{1}";
        if (robot.CurrentHealth % 1 > 0)
        {
            templateHP = "HP: {0, 10:f2}/{1}";
        }
        hpText.text = string.Format(templateHP, robot.CurrentHealth, robot.MaxHealth);
        
        // Text Stamina
        var templateStamina = "Stamina: {0}";
        staminaText.text = string.Format(templateStamina, robot.CurrentStamina);

        damageText.text = robot.Damage.ToString();
        defendText.text = robot.Defend.ToString();
    }
}
