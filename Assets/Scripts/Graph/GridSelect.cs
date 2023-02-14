using Unity.VisualScripting;
using UnityEngine;

namespace adefagia.Graph
{
    public class GridSelect : MonoBehaviour
    {
        public Camera mainCamera;
        public float zoomSpeed;
        public GameObject highlightPrefab;

        public LayerMask layerMask;
        public GameObject gameObjectHitByRay;

        public float tileSize = 1;

        public int gridIndex;
        private Grid _gridSelected;
    
        private Transform _hitObject;
        private float _timeElapsed;

        // Update is called once per frame
        void Update()
        {
            if (RayHitObject(CameraRay()))
            {
                CameraZoom(mainCamera, CameraRay(), zoomSpeed);
                HighlightSelected();

                if (!Input.GetMouseButtonDown(0)) return;
                
                _gridSelected = SelectGrid();
                // gridIndex = _gridSelected.index;

            }
        }

    

        Ray CameraRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        bool RayHitObject(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, mainCamera.farClipPlane, layerMask)) return false;
            
            _hitObject = hit.transform;
            // Debug.Log(hit.point);

            // SelectGrid(hit.point);
            // WaitForIt(1, () => Debug.Log("Hit point : " + hit.point));
            return true;

        }

        Grid SelectGrid()
        {
            var grid = GridManager.instance.GetGridByTransform(_hitObject);
            return grid.IsUnityNull() ? null : grid;
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

        void HighlightSelected()
        {
            var highlight = SelectGrid();
            
            if (highlight.IsUnityNull()) return;
            highlightPrefab.transform.position = highlight.GetLocation();
        }

        void MoveSelected(Vector3 point)
        {
            int x = (int) point.x;
            int y = (int) point.y;
            int z = (int) point.z;
        
            var location = new Vector3(x, y+0.01f, z) * tileSize;
            gameObjectHitByRay.transform.position = location;
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
}
