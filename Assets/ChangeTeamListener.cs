using System;
using System.Collections;
using Adefagia.BattleMechanism;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamListener : MonoBehaviour
{

    [SerializeField] private Text turnText;
    [SerializeField] private float lerpDuration = 1;
    
    private CanvasGroup _canvasGroup;
    private bool _change;
    
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        BattleManager.ChangeTurn += OnChangeTurn;
    }

    private void OnDisable()
    {
        BattleManager.ChangeTurn -= OnChangeTurn;
    }

    private void OnChangeTurn(string teamName)
    {
        _change = true;
        var template = "{0} TURN";
        turnText.text = string.Format(template, teamName.ToUpper());
        
        DOVirtual.Float(0, 1, lerpDuration*0.5f, v => _canvasGroup.alpha = v);
        StartCoroutine(Fadeout());

    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(lerpDuration);
        DOVirtual.Float(1, 0, lerpDuration*0.5f, v => _canvasGroup.alpha = v);
    }

}
