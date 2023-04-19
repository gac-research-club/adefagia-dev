using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuButton : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        // To quit play from editor (Can delete it later when it BT)
        /* #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif */

        Application.Quit();
        Debug.Log("Quitting game (Not work while play in editor)");
    }
}
