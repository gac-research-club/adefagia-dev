using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private int skillIndex;
    [FormerlySerializedAs("text")] [SerializeField] private Text skillText;

    private Button _button;
    private CanvasGroup _canvasGroup;
    
    public static event Action<int> SkillEvent;
    // Start is called before the first frame update
    void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _button.onClick.AddListener(Skill);
        // Debug.Log("Start");
    }

    private void OnEnable()
    {
        BattleManager.RobotNotHaveSkill += SkillUpdate;
    }
    
    private void OnDisable()
    {
        BattleManager.RobotNotHaveSkill -= SkillUpdate;
    }
    
    public void Skill()
    {
        SkillEvent?.Invoke(skillIndex);
    }
    
    private void SkillUpdate(RobotController robotController)
    {
        var robot = robotController.Robot;
        if (robotController.SkillController == null)
        {
            _button.interactable = false;
            _canvasGroup.alpha = 0;
            return;   
        }
        
        var skill = robotController.SkillController.ChooseSkill(skillIndex);

        if (skill == null)
        {
            _button.interactable = false;
            _canvasGroup.alpha = 0;
            return;
        }

        skillText.text = skill.Name;
        _button.interactable = (robot.CurrentStamina >= skill.StaminaRequirement);
        _canvasGroup.alpha = (robot.CurrentStamina >= skill.StaminaRequirement) ? 1 : 0.4f;
    }
}
