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
	[FormerlySerializedAs("text")][SerializeField] private Text skillText;

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
	void Update()
	{
		SkillUpdate(BattleManager.TeamActive.RobotControllerSelected);
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
		var skill = robotController.SkillController.ChooseSkill(skillIndex);
		var stamina = robot.CurrentStamina >= skill.StaminaRequirement;
		if (robotController.SkillController == null || skill == null)
		{
			_button.interactable = false;
			_canvasGroup.alpha = 0;
			return;
		}

		skillText.text = skill.Name;
		_button.interactable = stamina;
		_canvasGroup.alpha = stamina ? 1 : 0.4f;
	}
}
