using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private Transform quad;

    private MeshRenderer _renderer;

    public void ChangeColor(Color color)
    {
        _renderer = quad.GetComponent<MeshRenderer>();
        _renderer.material.SetColor("_Color", color);
    }
}
