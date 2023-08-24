using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHighlight : MonoBehaviour
{
    [SerializeField] private CharacterAction characterAction;
    
    
    private TileSelector _tileSelector;

    void Start()
    {
        _tileSelector = GetComponent<TileSelector>();
    }

    private void Update()
    {
        ChangeHighlightAction(characterAction);
    }

    private void ChangeHighlightAction(CharacterAction action)
    {
        // action.actionType;
    }
}
