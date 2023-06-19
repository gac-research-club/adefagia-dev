using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource musicAudioSource;

    private void Start()
    {
        // Mengatur nilai awal slider dengan volume yang disimpan sebelumnya
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        // Menetapkan nilai volume ke AudioSource saat memulai
        SetMusicVolume(volumeSlider.value);

        // Menambahkan listener saat nilai slider berubah
        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void SetMusicVolume(float value)
    {
        // Mengatur volume pada AudioSource sesuai dengan nilai slider
        musicAudioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
