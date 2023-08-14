using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] private float scrollScale;
    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private Vector3 maxZoomPerspective;
    [SerializeField] private Vector3 minZoomPerspective;

    private Vector3 newZoom;

    private Camera _camera;

    public static event Action<float> Zooming;

    private bool _isLimitZoom;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        newZoom = transform.localPosition;
        
    }

    private void Update()
    {
        // Get Scroll mouse input
        HandleInput();
        HandleZoomInput();
    }

    private void HandleInput()
    {
        if (_camera.orthographic)
        {
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
        }
    }
    
    private void HandleZoomInput()
    {
        if(_camera.orthographic) return;
        
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        
        // MinMaxZoom(newZoom, minZoomPerspective, maxZoomPerspective);

        transform.DOLocalMove(
            newZoom, 
            scrollScale);
        
    }

    private void MinMaxZoom(Vector3 current, Vector3 min, Vector3 max)
    {
        var direction = current - max;
        Debug.Log(direction.normalized);
    }
}