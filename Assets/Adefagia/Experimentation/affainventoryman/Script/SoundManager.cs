using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider MusicSlider;

    [SerializeField] Slider SFXSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = MusicSlider.value;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void Load()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
}
