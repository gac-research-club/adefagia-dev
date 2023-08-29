using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.UI;

public class SelectRobotListener : MonoBehaviour
{
    [SerializeField] private int id;
    
    [SerializeField] private Text robotName;
    [SerializeField] private Text robotIndex;

    private CanvasGroup _canvasGroup;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        BattleManager.UpdateDeploy += ChangeName;
    }

    private void OnDisable()
    {
        BattleManager.UpdateDeploy -= ChangeName;
    }

    public void ChangeName(int index, Robot robot)
    {
        if (index != id) return;

        robotName.text = robot.Name;
        robotIndex.text = $"Robot {index}";

        _canvasGroup.alpha = robot.HasDeploy ? 0.4f : 1;
    }
}
