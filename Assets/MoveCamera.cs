using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 borderMax;
    [SerializeField] private Vector3 borderMin;
    
    
    private float _currentX, _currentY;

    private float _maxX, _minX, _x;
    private float _maxY, _minY, _y;

    private void Start()
    {
        ZoomCamera.Zooming += UpdateBorder;
    }

    void Update()
    {
        //Detect if the middle mouse button is pressed
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Vector3 mousePos = Input.mousePosition;
            
            _maxX = Screen.width - (mousePos.x + mousePos.x * 0.5f);
            _minX = 0 - (mousePos.x + mousePos.x * 0.5f);
            _x = mousePos.x;
            
            _maxY = Screen.height - (mousePos.y + mousePos.y * 0.5f);
            _minY = 0 - (mousePos.y + mousePos.y * 0.5f);
            _y = mousePos.y;
        }
        
        if (Input.GetKey(KeyCode.Mouse2))
        {
            
            Vector3 mousePos = Input.mousePosition;
            // var x = (mousePos.x - Screen.width * 0.5f) / (Screen.width * 0.5f) * scale;
            // var y = (mousePos.y - Screen.height * 0.5f) / (Screen.height * 0.5f) * scale;

            var x = (mousePos.x - _x) / (_maxX-_minX);
            var y = (mousePos.y - _y) / (_maxY-_minY);
            
            {
                Debug.Log(new Vector2(x, y));
            }
            
            transform.Translate(Vector3.right * (x * Time.deltaTime * speed), Space.World);
            transform.Translate(Vector3.forward * (y * Time.deltaTime * speed), Space.World);

            var position = transform.position;
            if (position.x < borderMin.x)
            {
                position.x = borderMin.x;
            }

            if (position.z < borderMin.z)
            {
                position.z = borderMin.z;
            }

            if (position.x > borderMax.x)
            {
                position.x = borderMax.x;
            }

            if (position.z > borderMax.z)
            {
                position.z = borderMax.z;
            }

            transform.position = position;
        }
    }

    private void UpdateBorder(float scroll)
    {
        scroll *= 0.5f;
        
        borderMax.x += scroll;
        borderMax.z += scroll;
        
        borderMin.x -= scroll;
        borderMin.z -= scroll;
    }
}
