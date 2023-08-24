using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamListener : MonoBehaviour
{

    [SerializeField] private Text turnText;
    
    void Start()
    {
        BattleManager.ChangeTurn += OnChangeTurn;
    }

    private void OnChangeTurn(string teamName)
    {
        var template = "{0} TURN";
        turnText.text = string.Format(template, teamName.ToUpper());
    }

}
