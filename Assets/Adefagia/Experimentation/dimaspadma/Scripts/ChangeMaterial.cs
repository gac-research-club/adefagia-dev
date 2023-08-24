using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{

    [SerializeField] private Color color;
    
    [SerializeField] private Material material;

    private Color _startColor;
    
    // Start is called before the first frame update
    void Start()
    {
        // _startColor = material.color;
        var render = GetComponentInChildren<Renderer>();
        foreach ( var mat in render.materials)
        {
            Debug.Log(mat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // material.color = color;
    }

    private void OnDisable()
    {
        // material.color = _startColor;
    }
}
