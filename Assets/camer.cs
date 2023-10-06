using UnityEngine;

public class camer : MonoBehaviour
{
    public Camera mainCamera; // Kamera utama
    public Camera secondCamera; // Kamera kedua

    void Start()
    {
        // Aktifkan kamera utama dan nonaktifkan kamera kedua saat permainan dimulai
        mainCamera.enabled = true;
        secondCamera.enabled = false;
    }

    void Update()
    {
        // Pindah kamera dengan tombol "V"
        if (Input.GetKeyDown(KeyCode.V))
        {
            mainCamera.enabled = !mainCamera.enabled;
            secondCamera.enabled = !secondCamera.enabled;
        }
    }
}
