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
    [SerializeField] private float distanceWhileZoom;
    
    [SerializeField] private Vector3 maxZoomPerspective;
    [SerializeField] private Vector3 minZoomPerspective;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Texture2D cursorZoomTexture;


    private Vector3 newZoom;

    private Camera _camera;
    private float _timeOut = 3f, _elapsedTime;

    public static event Action<float> Zooming;

    private bool _isLimitZoom;
    private Vector3 startZoom;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        newZoom = transform.localPosition;
        startZoom = transform.localPosition;
        
        _isLimitZoom = false;
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
            Cursor.SetCursor(cursorZoomTexture, Vector2.zero, CursorMode.Auto);
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        
        var borderZoom = MinMaxZoom(newZoom, minZoomPerspective, maxZoomPerspective);
        
        if (Vector3.Distance(transform.localPosition, borderZoom) < distanceWhileZoom)
        {
            if (!MoveCamera.IsMove)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            } 
        }
        
        if (!_isLimitZoom)
        {
            transform.DOLocalMove(
                borderZoom, 
                scrollScale);
        }
        else
        {
            newZoom = borderZoom;
        }
    }

    private Vector3 MinMaxZoom(Vector3 current, Vector3 min, Vector3 max)
    {
        
        // Min
        if (current.y < min.y)
        {
            Debug.Log("Min" + current);
            _isLimitZoom = true;
            return min + offset;
        }

        // Max
        if (current.y > max.y)
        {
            Debug.Log("Max" + current);
            _isLimitZoom = true;
            return max - offset;
        }

        _isLimitZoom = false;
        return current;
    }
}