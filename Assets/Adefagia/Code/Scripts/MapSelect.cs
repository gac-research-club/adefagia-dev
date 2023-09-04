using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private string scene;
    [SerializeField] private bool canCreateNew;

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
            PlayerPrefs.SetString("Map", mapName);
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogWarning("Map Not found");
        }
    }

    public void Random()
    {
        PlayerPrefs.SetString("Map", "<random>");
        SceneManager.LoadScene(scene);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,200,200));
        if (GUILayout.Button("Back")) SceneManager.LoadScene("MainMenuAffaOpsi2");
        GUILayout.EndArea();
    }
}
