using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider masterMusicSlider;
    public Slider soundFxSlider;
    public AudioSource musicAudioSource;

    private void Start()
    {
        if (masterMusicSlider != null)
        {
            // Mengatur nilai awal slider dengan volume yang disimpan sebelumnya
            masterMusicSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            // float volume = PlayerPrefs.GetFloat("Volume", 1f);

            // Menetapkan nilai volume ke AudioSource saat memulai
            SetMusicVolume(masterMusicSlider.value);

            // Menambahkan listener saat nilai slider berubah
            masterMusicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            // Menetapkan nilai volume ke AudioSource saat memulai
            SetMusicVolume(PlayerPrefs.GetFloat("Volume", 1f));
        }
    }

    private void SetMusicVolume(float value)
    {
        // Mengatur volume pada AudioSource sesuai dengan nilai slider
        musicAudioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
