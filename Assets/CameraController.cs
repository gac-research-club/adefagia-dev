using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Kecepatan gerakan kamera
    public float sensitivityX = 2.0f; // Sensitivitas horizontal (ke kiri/kanan)
    public float sensitivityY = 2.0f; // Sensitivitas vertikal (atas/bawah)

    private float rotationX = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mendapatkan input dari mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Menghitung rotasi kamera pada sumbu X (atas-bawah)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // Mengatur rotasi kamera dengan quaternion
        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y + mouseX, 0.0f);

        // Mendapatkan input dari tombol keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Menghitung pergerakan dalam sistem koordinat lokal kamera
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);

        // Menggerakkan kamera dengan input keyboard
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
