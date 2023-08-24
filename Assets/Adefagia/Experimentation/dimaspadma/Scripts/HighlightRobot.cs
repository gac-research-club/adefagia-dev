using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRobot : MonoBehaviour
{
    [SerializeField] private List<Renderer> partRenderers;
    [SerializeField] private Material material;
    [SerializeField] private Color color;

    private Color _defaultColor;
    private Material _newMaterial;

    void Start()
    {
        _newMaterial = new Material(material);
        _defaultColor = _newMaterial.color;
        
        foreach (var partRenderer in partRenderers)
        {
            partRenderer.material = _newMaterial;
        }
    }

    public void ChangeColor()
    {
        _newMaterial.color = color;
    }

    public void ResetColor()
    {
        _newMaterial.color = _defaultColor;
    }
}
