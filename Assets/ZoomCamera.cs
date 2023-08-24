using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] private float scrollScale;
    
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;

    private Camera _camera;

    public static event Action<float> Zooming;

    private bool _isLimitZoom;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Get Scroll mouse input
        _camera.orthographicSize -= Input.mouseScrollDelta.y * scrollScale;
        
        if (_camera.orthographicSize < minZoom)
        {
            _camera.orthographicSize = minZoom;
            _isLimitZoom = true;
        }

        if (_camera.orthographicSize > maxZoom)
        {
            _camera.orthographicSize = maxZoom;
            _isLimitZoom = true;
        }

        if (_camera.orthographicSize > minZoom && _camera.orthographicSize < maxZoom)
        {
            _isLimitZoom = false;
        }
        
        if (!_isLimitZoom)
        {
            Zooming?.Invoke(Input.mouseScrollDelta.y * scrollScale);
        }
    }
}