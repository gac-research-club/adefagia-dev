using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileSelectListener : MonoBehaviour
{
    void Start()
    {
        TileSelector.TileSelecting += MoveToSelect;
    }

    private void MoveToSelect(Tile tile)
    {
        if (tile == null || tile.CannotMove)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            transform.DOMove(tile.Position, 0.05f);
        }
    }
}
