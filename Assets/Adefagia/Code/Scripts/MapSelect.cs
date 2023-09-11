using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Adefagia;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMeshProUGUI;
	// [SerializeField] private string scene;
	[SerializeField] private bool canCreateNew;

	private TeamManager _teamManager;

	private void Start()
	{
		_teamManager = GameManager.instance.teamManager;
	}

	public void Choose()
	{
		var mapName = textMeshProUGUI.text;
		mapName = mapName
			.Replace("/r", "")
			.Replace("\u200B", "")
			.Replace("/n", "");

		var path = $"{Application.persistentDataPath}/Map/{mapName}.json";

		if (File.Exists(path) || canCreateNew)
		{
			if (_teamManager.totalRobot == 1)
			{
				PlayerPrefs.SetString("Map", mapName);
				SceneManager.LoadScene("1v1");
			}
			else
			{
				PlayerPrefs.SetString("Map", mapName);
				SceneManager.LoadScene("Battle");
			}
		}
		else
		{
			Debug.LogWarning("Map Not found");
		}
	}

	public void Random()
	{
		if (_teamManager.totalRobot == 1)
		{
			PlayerPrefs.SetString("Map", "<random>");
			SceneManager.LoadScene("1v1");
		}
		else
		{
			PlayerPrefs.SetString("Map", "<random>");
			SceneManager.LoadScene("Battle");
		}
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, 200, 200));
		if (GUILayout.Button("Back")) SceneManager.LoadScene("MainMenuAffaUtama");
		GUILayout.EndArea();
	}
}
