using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioSource audioSource;
    public Slider volumeSlider;

    private float defaultVolume;

    private void Start()
    {
        if (audioSource != null)
        {
            defaultVolume = PlayerPrefs.GetFloat("Volume", audioSource.volume);
            audioSource.volume = defaultVolume;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = defaultVolume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void ClickedSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    private void UpdateVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void ResetVolume()
    {
        audioSource.volume = defaultVolume;
        // volumeSlider.value = defaultVolume;
        PlayerPrefs.SetFloat("Volume", defaultVolume);
    }
}
