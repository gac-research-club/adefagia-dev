using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickListener : MonoBehaviour
{

    [SerializeField] private ClickType clickType;
    private Button _button;

    public static event Action<ClickType> ClickEvent;
    public static event Action<int> ItemEvent;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void Click()
    {
        ClickEvent?.Invoke(clickType);    
    }

    public void Item(int item)
    {
        ItemEvent?.Invoke(item);
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
