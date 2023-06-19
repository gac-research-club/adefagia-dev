using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneload : MonoBehaviour
{
    public void PlaytoMainMode()
    {
        SceneManager.LoadScene("MainMode");
    }

    public void Play3VS3()
    {
        SceneManager.LoadScene("Inventory");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadStore()
    {
        SceneManager.LoadScene("Store");
    }
}