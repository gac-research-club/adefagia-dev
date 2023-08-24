using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoTileListener : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text description;

    private CanvasGroup _canvasGroup;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        TileSelector.TileSelecting += UpdateDescription;
    }

    private void UpdateDescription(Tile tile)
    {
        if (tile == null)
        {
            Hide();
            return;
        }

        Show();
        title.text = tile.ToString();
        description.text = tile.Description;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0;
    }

    private void Show()
    {
        _canvasGroup.alpha = 1;
    }
}
