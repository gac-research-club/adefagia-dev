using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.Inventory;
using Adefagia.BattleMechanism;
using UnityEngine;
using Adefagia.CharacterStats;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StatPanel : MonoBehaviour
{
	[SerializeField] StatDisplay[] statDisplays;
	[SerializeField] string[] statNames;

	[SerializeField] Text characterName;

	[SerializeField] private Button buttonBattle;

	private CharacterStat[] stats;

	private static int indexRobot = 0;

	private TeamManager _teamManager;
	private Team _currentTeam;

	private ItemState _itemState = ItemState.Initialize;


	private void Start()
	{
		// Initiate Robots
		_teamManager = GameManager.instance.teamManager;

		// Initial Name 
		_currentTeam = _teamManager.teamA;
		characterName.text = "Robot " + indexRobot.ToString() + " | Team " + _currentTeam.teamName;

		statDisplays = GetComponentsInChildren<StatDisplay>();
		UpdateStatNames();
	}

	private void Update()
	{

		if (_itemState == ItemState.Update)
		{
			// Update damage value
			_teamManager.GetRobots(_currentTeam)[indexRobot].damage = statDisplays[0].Stat.Value;

			// Update armor value
			_teamManager.GetRobots(_currentTeam)[indexRobot].armor = statDisplays[1].Stat.Value;

		}

	}

	private void OnValidate()
	{
		Debug.Log("OnValidate");
		_itemState = ItemState.Update;
	}

	// charStats = Armor, Attack
	public void SetStats(params CharacterStat[] charStats)
	{
		stats = charStats;

		// below stat display length
		if (stats.Length > statDisplays.Length)
		{
			Debug.LogError("Not Enough Stat Display!");
			return;
		}

		for (int i = 0; i < statDisplays.Length; i++)
		{
			statDisplays[i].gameObject.SetActive(i < stats.Length);

			if (i < stats.Length)
			{
				statDisplays[i].Stat = stats[i];
			}
		}
	}

	public void UpdateStatValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].UpdateStatValue();
		}
	}

	public void UpdateStatNames()
	{
		_itemState = ItemState.Update;
		for (int i = 0; i < statNames.Length; i++)
		{
			statDisplays[i].Name = statNames[i];
		}
	}
	public void UpdateUsableItemId(UsableItem item, string type)
	{
		if (type == EquipmentType.BuffItem1.ToString())
		{

			// Update Buff Item 1 id
			_teamManager.GetRobots(_currentTeam)[indexRobot].buffItem1 = item;

		}
		else if (type == EquipmentType.BuffItem2.ToString())
		{

			// Update Buff Item 2 id
			_teamManager.GetRobots(_currentTeam)[indexRobot].buffItem2 = item;
		}
	}

	public void UpdateEquipmentId(EquippableItem item, string type)
	{
		if (type == EquipmentType.Top.ToString())
		{

			// Update helmet id
			_teamManager.GetRobots(_currentTeam)[indexRobot].helmetId = item;

		}
		else if (type == EquipmentType.Body.ToString())
		{

			// Update armor id
			_teamManager.GetRobots(_currentTeam)[indexRobot].armorId = item;

		}
		else if (type == EquipmentType.Weapon.ToString())
		{

			// Update weapon id
			_teamManager.GetRobots(_currentTeam)[indexRobot].weaponId = item;
		}

	}



	public void UnequipId(string type)
	{
		if (type == EquipmentType.Top.ToString())
		{

			// Update helmet id
			_teamManager.GetRobots(_currentTeam)[indexRobot].helmetId = null;

		}
		else if (type == EquipmentType.Body.ToString())
		{

			// Update armor id
			_teamManager.GetRobots(_currentTeam)[indexRobot].armorId = null;

		}
		else if (type == EquipmentType.Weapon.ToString())
		{

			// Update weapon id
			_teamManager.GetRobots(_currentTeam)[indexRobot].weaponId = null;
		}
		else if (type == EquipmentType.BuffItem1.ToString())
		{

			// Update Buff Item 1 id
			_teamManager.GetRobots(_currentTeam)[indexRobot].buffItem1 = null;

		}
		else if (type == EquipmentType.BuffItem2.ToString())
		{

			// Update Buff Item 2 id
			_teamManager.GetRobots(_currentTeam)[indexRobot].buffItem2 = null;
		}
	}

	// Unity Event
	public void ChangeTeam()
	{
		_currentTeam = _teamManager.teamB;
		characterName.text = "Robot " + indexRobot.ToString() + " | Team " + _currentTeam.teamName;

		// Reset index robot
		indexRobot = 0;

		// Make sure 2 team have been selected equipment
		if (_currentTeam == _teamManager.teamB)
		{
			// _itemState = ItemState.Finish;
			buttonBattle.gameObject.SetActive(true);
		}

	}

	public Team GetCurrentTeam()
	{
		return _currentTeam;
	}

	public void ChangeRobotIndex(int index)
	{
		indexRobot = index;
		characterName.text = "Robot " + indexRobot.ToString() + " | Team " + _currentTeam;
	}

	public Dictionary<String, EquippableItem> GetDetailEquipment(int index)
	{
		Dictionary<String, EquippableItem> statRobot = new Dictionary<String, EquippableItem>();

		statRobot.Add("helmetId", _teamManager.GetRobots(_currentTeam)[index].helmetId);
		statRobot.Add("armorId", _teamManager.GetRobots(_currentTeam)[index].armorId);
		statRobot.Add("weaponId", _teamManager.GetRobots(_currentTeam)[index].weaponId);

		return statRobot;
	}

	public Dictionary<String, UsableItem> GetDetailItemEquip(int index)
	{
		Dictionary<String, UsableItem> statRobot = new Dictionary<String, UsableItem>();

		statRobot.Add("itemBuff1", _teamManager.GetRobots(_currentTeam)[index].buffItem1);
		statRobot.Add("itemBuff2", _teamManager.GetRobots(_currentTeam)[index].buffItem2);

		return statRobot;
	}

	public List<Dictionary<String, EquippableItem>> GetDetailEquipmentTeam(Team team)
	{

		List<Dictionary<String, EquippableItem>> listStatRobot = new List<Dictionary<String, EquippableItem>>();

		if (_teamManager.totalRobot == 1)
		{
			for (int index = 0; index < 1; index++)
			{
				Dictionary<String, EquippableItem> statRobot = new Dictionary<String, EquippableItem>();

				statRobot.Add("helmetId", _teamManager.GetRobots(team)[index].helmetId);
				statRobot.Add("armorId", _teamManager.GetRobots(team)[index].armorId);
				statRobot.Add("weaponId", _teamManager.GetRobots(team)[index].weaponId);

				listStatRobot.Add(statRobot);
			}
		}
		else
			for (int index = 0; index < 3; index++)
			{
				Dictionary<String, EquippableItem> statRobot = new Dictionary<String, EquippableItem>();

				statRobot.Add("helmetId", _teamManager.GetRobots(team)[index].helmetId);
				statRobot.Add("armorId", _teamManager.GetRobots(team)[index].armorId);
				statRobot.Add("weaponId", _teamManager.GetRobots(team)[index].weaponId);

				listStatRobot.Add(statRobot);
			}

		return listStatRobot;
	}

	public List<Dictionary<String, UsableItem>> GetDetailItemTeam(Team team)
	{

		List<Dictionary<String, UsableItem>> listStatRobot = new List<Dictionary<String, UsableItem>>();

		if (_teamManager.totalRobot == 1)
		{
			for (int index = 0; index < 1; index++)
			{
				Dictionary<String, UsableItem> statRobot = new Dictionary<String, UsableItem>();

				statRobot.Add("itemBuff1", _teamManager.GetRobots(team)[index].buffItem1);
				statRobot.Add("itemBuff2", _teamManager.GetRobots(team)[index].buffItem2);

				listStatRobot.Add(statRobot);
			}
		}
		else
		{
			for (int index = 0; index < 3; index++)
			{
				Dictionary<String, UsableItem> statRobot = new Dictionary<String, UsableItem>();

				statRobot.Add("itemBuff1", _teamManager.GetRobots(team)[index].buffItem1);
				statRobot.Add("itemBuff2", _teamManager.GetRobots(team)[index].buffItem2);

				listStatRobot.Add(statRobot);
			}
		}

		return listStatRobot;
	}

}

public enum ItemState
{
	Initialize,
	Update,
	Finish,
}