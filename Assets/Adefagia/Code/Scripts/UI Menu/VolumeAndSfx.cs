using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeAndSfx : MonoBehaviour
{
    public Slider _volumeSlider;

    // Change volume use slider in unity
    public void MusicVolume() {
        SoundFunction.Instance.MusicVolume(_volumeSlider.value);
    }

    //Mute the volume by press volume above slider
    public void MuteMusic() {
        SoundFunction.Instance.MuteMusic();
    }

    // while click give sfx
    public void OnClick() {
        SoundFunction.Instance.SfxPlay("mouse_click");
    }
}
