using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneload : MonoBehaviour
{
    public void PlaytoMainMode()
    {
        SceneManager.LoadScene("ChooseModeAffa");
    }

    public void Play3VS3()
    {
        SceneManager.LoadScene("Inventory");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuAffa");
    }

    public void LoadStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LogiScene()
    {
        SceneManager.LoadScene("LoginForm");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
