using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Adefagia.SelectObject
{
    public class Select : MonoBehaviour
    {
        public List<Camera> cameras;
        public LayerMask gridMask;
        
        public GameObject objectHit;

        public UnityGameObjectEvent mouseHover;
        public UnityEvent mouseHoverNotHit;
        public UnityGameObjectEvent mouseRightClick;

        private Camera _cameraSelected;
        private int _cameraSelectedIndex;

        private void Start()
        {
            // Initialize camera
            _cameraSelected = cameras[0];
        }

        void Update()
        {
            if (Input.GetMouseButton(3))
            {
                Debug.Log("Test");
            }
            // Check if the mouse was clicked over a UI element
            // if (EventSystem.current.IsPointerOverGameObject())
            // {
            //     return;
            //     // Debug.Log("Clicked on the UI");
            // }
            
            // Hover Event
            mouseHover.Invoke(objectHit);

            if (RayHitObject(CameraRay(_cameraSelected), gridMask, _cameraSelected))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    MouseRightClick(objectHit);
                }
            }
            else
            {
                // Ray not hit object
                mouseHoverNotHit.Invoke();
            }
        }

        public T GetObjectHit<T>()
        {
            try
            {
                return objectHit.GetComponent<T>();
            }
            catch (NullReferenceException)
            {
                return default;
            }
        } 
        
        /*------------------------------------------------------------------------------------
         * Shot ray from mouse pointer position to relative main camera
         *------------------------------------------------------------------------------------*/
        private Ray CameraRay(Camera cameraSelect)
        {
            return cameraSelect.ScreenPointToRay(Input.mousePosition);
        }

        /*------------------------------------------------------------------------------------
         * Check whatever ray hit object or not
         * true if hit gameObject
         * false if not
         *------------------------------------------------------------------------------------*/
        private bool RayHitObject(Ray ray, LayerMask mask, Camera cameraSelect)
        {
            // If ray hit gameObject, set to Highlight
            if (Physics.Raycast(ray, out var hit, cameraSelect.farClipPlane, mask))
            {
                objectHit = hit.transform.gameObject;
                return true;
            }
            
            objectHit = null;
            
            return false;
        }

        /*------------------------------------------------------------------------------------
         * Delegate mouse Right Click action for selected gameObject
         *------------------------------------------------------------------------------------*/
        private void MouseRightClick(GameObject gameObjectSelected)
        {
            // Make sure gameObject not null
            if (gameObjectSelected.IsUnityNull()) return;

            mouseRightClick.Invoke(gameObjectSelected);
        }
        
        //=======================================
        // Change camera
        public void ChangeCamera()
        {
            // save last camera
            var lastCamera = cameras[_cameraSelectedIndex];
            lastCamera.tag = "Untagged";
                
            // Increment camera index
            _cameraSelectedIndex++;

            // index must not greater then array
            if (_cameraSelectedIndex >= cameras.Count)
            {
                _cameraSelectedIndex = 0;
            }
            
            // Change camera
            _cameraSelected = cameras[_cameraSelectedIndex];
            _cameraSelected.enabled = true;
            _cameraSelected.tag = "MainCamera";
            
            // Hide previous camera
            lastCamera.enabled = false;
        }
    }
    
    [System.Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> {}

}
