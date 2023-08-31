using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ButtonClickListener : MonoBehaviour
{

    [SerializeField] private ClickType clickType;
    public static event Action<ClickType> ClickEvent;
    public static event Action<int> ItemEvent;
    
    private Button _button;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _button.onClick.AddListener(Click);
    }

    private void Update()
    {
        if (BattleManager.battleState == BattleState.SelectRobot)
        {
            if (clickType == ClickType.Cancel)
            {
                Hide(true);
            }
        }
        else
        {
            if (clickType == ClickType.Cancel)
            {
                Hide(false);
            }
        }
    }

    private void OnEnable()
    {
        BattleManager.SelectRobot += UpdateButton;
    }

    private void OnDisable()
    {
        BattleManager.SelectRobot -= UpdateButton;
    }

    private void Click()
    {
        ClickEvent?.Invoke(clickType);
    }

    public void Item(int item)
    {
        ItemEvent?.Invoke(item);
    }

    void UpdateButton(Robot robot)
    {
        switch (clickType)
        {
            case ClickType.Move:
                _button.interactable = !robot.HasMove;
                break;
            case ClickType.Attack:
                _button.interactable = !robot.HasAttack;
                break;
        }
    }

    void Hide(bool hide)
    {
        _canvasGroup.interactable = !hide;
        _canvasGroup.alpha = hide ? 0 : 1;
    }
}

public enum ClickType
{
    Move,
    Attack,
    EndTurn,
    Cancel,
    Item,
    Skill,
}
