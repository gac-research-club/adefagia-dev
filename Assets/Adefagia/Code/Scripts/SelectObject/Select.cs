using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Adefagia.SelectObject
{
    public class Select : MonoBehaviour
    {
        public Camera mainCamera;
        public LayerMask layerMask;
        
        public GameObject objectHit;

        public UnityGameObjectEvent mouseHover;
        public UnityEvent mouseHoverNotHit;
        public UnityGameObjectEvent mouseRightClick;

        void Update()
        {
            if (RayHitObject(CameraRay()))
            {
                // Hover Event
                mouseHover.Invoke(objectHit);

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
        private Ray CameraRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        /*------------------------------------------------------------------------------------
         * Check whatever ray hit object or not
         * true if hit gameObject
         * false if not
         *------------------------------------------------------------------------------------*/
        private bool RayHitObject(Ray ray)
        {
            // If ray hit gameObject, set to Highlight
            if (Physics.Raycast(ray, out var hit, mainCamera.farClipPlane, layerMask))
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
    }
    
    [System.Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> {}

}
