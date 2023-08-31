using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using UnityEngine;

public class ButtonPrevNextListener : MonoBehaviour
{
    [SerializeField] private ButtonType type;
    
    // Start is called before the first frame update
    private CanvasGroup _canvasGroup;
    private int _currentIndex;
    private int _totalRobot;
    
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _totalRobot = GameManager.instance.GetComponent<TeamManager>().totalRobot;
    }

    private void OnEnable()
    {
        Character.PrevNextEvent += UpdateButton;
    }

    private void OnDisable()
    {
        Character.PrevNextEvent -= UpdateButton;
    }

    void UpdateButton(int index)
    {
        _currentIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: dynamic count list robot team
        if (type == ButtonType.Next)
        {
            _canvasGroup.alpha = (_currentIndex == _totalRobot-1) ? 0 : 1;
        }
        else
        {
            _canvasGroup.alpha = (_currentIndex == 0) ? 0 : 1;
        }
    }
    
    enum ButtonType
    {
        Next,
        Prev,
    }
}
