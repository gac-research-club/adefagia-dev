using System;
using UnityEngine;

namespace Playground.Grid
{
    public class GridSelect : MonoBehaviour
    {
        public Camera mainCamera;
        public float zoomSpeed;

        public LayerMask quadLayer;
        public GameObject selectedQuad;

        public float tileSize = 1;
        public Vector3 tilling;

        public int gridIndex;
    
        private Transform _hitObject;
        private float timeElapsed;

        // Update is called once per frame
        void Update()
        {
            timeElapsed += Time.deltaTime;
            CameraRay();
        }

    

        void CameraRay()
        {
            try
            {
                Ray rayCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
            
                SelectQuad(rayCamera);
                CameraZoom(mainCamera, rayCamera, zoomSpeed);
            }
            catch (UnassignedReferenceException)
            {
                WaitForIt(3, () => Debug.LogWarning("! Camera belum diset di inspector " + gameObject.name));
            }

        }

        void SelectQuad(Ray ray)
        {
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit, mainCamera.farClipPlane, quadLayer))
            {
                _hitObject = hit.transform;
                // MoveSelected(hit.point + tilling);
                selectedQuad = _hitObject.gameObject;
                // Debug.Log(hit.point);
            
                SelectGrid(hit.point);
                // WaitForIt(1, () => Debug.Log("Hit point : " + hit.point));
            }
        }

        void SelectGrid(Vector3 point)
        {
            int x = (int)(point.x + 0.5f);
            int y = (int)(point.z + 0.5f);

            Grid grid = GridManager.GetNode(new Vector2(x, y));
            gridIndex = grid.index;
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
        
            var location = new Vector3(x, y+0.01f, z) * tileSize;
            selectedQuad.transform.position = location;
        }

        void WaitForIt(float second, Action action)
        {
            if (timeElapsed > second)
            {
                action();
                timeElapsed = 0f;
            }
        }
    
        void LocalAndWorld()
        {
            Debug.Log("Local Space : " + Vector3.forward);
            var direction = transform.TransformDirection(Vector3.forward);
            Debug.Log("World Space : " + direction);
        }
    }
}
