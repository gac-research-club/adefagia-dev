using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace adefagia.Graph
{
    public class Select : MonoBehaviour
    {
        public Camera mainCamera;
        public LayerMask layerMask;
        
        // public float zoomSpeed;
        public GameObject highlightGameObject;

        public UnityGameObjectEvent mouseHover;
        public UnityGameObjectEvent mouseRightClick;

        // public float tileSize = 1;

        // public int gridIndex;
        private Grid _gridSelected;
    
        // private Transform _hitObject;
        private float _timeElapsed;

        private GridManager _gridManager;

        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
        }

        void Update()
        {
            if (RayHitObject(CameraRay()))
            {
                // CameraZoom(mainCamera, CameraRay(), zoomSpeed);
                // var gridHighlight = HighlightSelected();
                mouseHover.Invoke(highlightGameObject);

                if (Input.GetMouseButtonDown(0))
                {
                    StoreSelectedGrid(highlightGameObject);
                }
            }
        }

    

        Ray CameraRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        bool RayHitObject(Ray ray)
        {
            // If ray hit gameObject, set to Highlight
            if (!Physics.Raycast(ray, out var hit, mainCamera.farClipPlane, layerMask)) return false;
            
            highlightGameObject = hit.transform.gameObject;
            
            return true;

        }

        void StoreSelectedGrid(GameObject selected)
        {
            if (selected.IsUnityNull()) return;

            mouseRightClick.Invoke(selected);
        }

        void CameraZoom(Camera cam, Ray ray, float speed)
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

        void MoveSelected(Vector3 point)
        {
            int x = (int) point.x;
            int y = (int) point.y;
            int z = (int) point.z;
        
            // var location = new Vector3(x, y+0.01f, z) * tileSize;
            // gameObjectHitByRay.transform.position = location;
        }

        void IncrementTimeElapsed()
        {
            _timeElapsed += Time.deltaTime;
        }

        void ResetTimeElapsed(float second)
        {
            if (_timeElapsed > second)
            {
                _timeElapsed = 0f;
            }
        }
    
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
