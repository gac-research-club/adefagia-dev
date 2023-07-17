using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIFinishController : MonoBehaviour
{
    [SerializeField] private Text textTeamName;
    [SerializeField] private CanvasGroup canvasFinish;

    [SerializeField] private bool fadeIn;
    [SerializeField] private float scaleSpeed = 1;
    
    private void Start()
    {
        // Fade in
        canvasFinish.alpha = 0;
        fadeIn = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (canvasFinish.alpha < 1)
            {
                canvasFinish.alpha += Time.deltaTime * scaleSpeed;
                if (canvasFinish.alpha >= 1)
                {
                    fadeIn = false;
                    
                    // 3 second
                    Invoke("ChangeScene", 3);
                }
            }
            else
            {
                fadeIn = false;
            }
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("PostBattle");
    }


    public void ChangeName(string teamName)
    {
        textTeamName.text = $"Team {teamName} Win";
    }
}
