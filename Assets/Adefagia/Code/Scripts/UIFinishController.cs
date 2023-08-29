using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIFinishController : MonoBehaviour
{
    [SerializeField] private Text textTeamName;
    [SerializeField] private Text textTeam1;
    [SerializeField] private Text textTeam2;
    [SerializeField] private Text textRobotDead;
    [SerializeField] private Text textTotalDamage1;
    [SerializeField] private Text textTotalDamage2;
    [SerializeField] private CanvasGroup canvasFinish;

    [SerializeField] private float lifespan;

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
                    Invoke("ChangeScene", lifespan);
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

    public void ChangeRobotDead(string robotDead)
    {
        textRobotDead.text = $"Robot Dead : \n \n {robotDead} Win";
    }

    public void ChangeTotalDamage(float totalDamage, string nameTeam, int index)
    {
        if (index == 0)
        {
            textTotalDamage1.text = $"Total Damage : {totalDamage}";
            textTeam1.text = $"{nameTeam}";
        }
        else
        {
            textTotalDamage2.text = $"Total Damage : {totalDamage}";
            textTeam2.text = $"{nameTeam}";
        }
    }
}
