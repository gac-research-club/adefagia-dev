using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneload : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainMode");
    }

    public void VS3()
    {
        SceneManager.LoadScene("Inventory");
    }
}
