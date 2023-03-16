using UnityEngine;

namespace Adefgia.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public float speed;
        private void Update()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            var horizontalDistance = horizontalInput * speed * Time.deltaTime;
            var verticalDistance = verticalInput * speed * Time.deltaTime;
        
            transform.Translate(Vector3.right * horizontalDistance);
            transform.Translate(Vector3.up * verticalDistance);
        }
    }
}