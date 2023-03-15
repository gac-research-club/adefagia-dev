using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Adefagia.SelectObject
{
    public class Select : MonoBehaviour
    {
        public Camera mainCamera;
        public LayerMask layerMask;
        
        public GameObject highlightGameObject;

        public UnityGameObjectEvent mouseHover;
        public UnityEvent mouseHoverNotHit;
        public UnityGameObjectEvent mouseRightClick;

        void Update()
        {
            if (RayHitObject(CameraRay()))
            {
                // Hover Event
                mouseHover.Invoke(highlightGameObject);

                if (Input.GetMouseButtonDown(0))
                {
                    MouseRightClick(highlightGameObject);
                }
            }
            else
            {
                // Ray not hit object
                mouseHoverNotHit.Invoke();
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
                highlightGameObject = hit.transform.gameObject;
                return true;
            }
            
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

        /*------------------------------------------------------------------------------------
         * Zoom in zoom out if ray hit object
         *------------------------------------------------------------------------------------*/
        private void CameraZoom(Camera cam, Ray ray, float speed)
        {
            float zoomDistance = speed * Input.mouseScrollDelta.y * Time.deltaTime;
        
            if (cam.orthographic)
            {
                CameraOrthographicZoom(cam, zoomDistance);
            }
            else
            {
                CameraPerspectiveZoom(cam, ray, zoomDistance);
            }
        }

        // Zoom in Perspective view
        void CameraPerspectiveZoom(Camera cam, Ray ray, float speed)
        {
            cam.transform.Translate(ray.direction * speed, Space.World);
        }
    
        // Zoom in Orthographic view
        void CameraOrthographicZoom(Camera cam, float speed)
        {
            cam.orthographicSize -= speed;
        }

        /*------------------------------------------------------------------------------------
         * Change local coordinate to world coordinate
         *------------------------------------------------------------------------------------*/
        void LocalAndWorld()
        {
            Debug.Log("Local Space : " + Vector3.forward);
            var direction = transform.TransformDirection(Vector3.forward);
            Debug.Log("World Space : " + direction);
        }
    }
    
    [System.Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> {}

}
