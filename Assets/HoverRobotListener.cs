using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.UI;

public class HoverRobotListener : MonoBehaviour
{
    [SerializeField] private CanvasGroup robotStat;
    [SerializeField] private float speed;

    [SerializeField] private Text robotName;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    [SerializeField] private Text staminaText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text defendText;
    

    private float _deltaAlpha;
    private bool _show;
    
    private void OnEnable()
    {
        GridManager.GridRobotHoverInfo += OnHoverRobot;
        RobotAttack.RobotDamaged += UpdateStat;
    }

    private void Update()
    {
        if (_show)
        {
            if (_deltaAlpha < 1)
            {
                _deltaAlpha += Time.deltaTime * speed;
            }
        }
        else
        {
            if (_deltaAlpha > 0)
            {
                _deltaAlpha -= Time.deltaTime * speed;
            }
        }
        
        robotStat.alpha = _deltaAlpha;
    }

    private void OnHoverRobot(GridController gridController, bool show)
    {
        _show = show;
        
        var robotController = gridController.RobotController;
        
        if(robotController == null) return;
        
        var robot = robotController.Robot;
        
        UpdateName(robot);
        UpdateStat(robot);
    }

    private void OnDisable()
    {
        GridManager.GridRobotHoverInfo -= OnHoverRobot;
    }

    private void UpdateName(Robot robot)
    {
        robotName.text = robot.ToString();
    }

    private void UpdateStat(Robot robot)
    {
        // Image
        var image = healthBar.transform.GetChild(1).GetComponent<Image>();

        // Text
        var text = healthBar.transform.GetChild(2).GetComponent<Text>();

        image.fillAmount = robot.CurrentHealth / robot.MaxHealth;
        
        var template = "HP: {0}/{1}";
        if (robot.CurrentHealth % 1 > 0)
        {
            template = "HP: {0, 10:f2}/{1}";
        }
        text.text = string.Format(template, robot.CurrentHealth, robot.MaxHealth);

        staminaBar.fillAmount = robot.CurrentStamina / robot.MaxStamina;
        // Text Stamina
        var templateStamina = "EN: {0}/{1}";
        staminaText.text = string.Format(templateStamina, robot.CurrentStamina, robot.MaxStamina);

        damageText.text = robot.Damage.ToString();
        defendText.text = robot.Defend.ToString();
    }
}
