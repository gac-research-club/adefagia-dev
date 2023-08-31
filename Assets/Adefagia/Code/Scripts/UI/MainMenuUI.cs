using System.Collections;
using System.Collections.Generic;
using Adefagia;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadBattle()
    {
        GameManager.instance.teamManager.SaveToJson();
        SceneManager.LoadScene("Battle");
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
