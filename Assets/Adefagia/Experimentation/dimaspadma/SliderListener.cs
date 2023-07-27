using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderListener : MonoBehaviour
{
    private Slider _slider;
    
    void Start()
    {
        _slider = GetComponent<Slider>();
        
        LoadAddressableScene.UpdateSlider += (min,value,max) =>
        {
            _slider.minValue = min;
            _slider.value = value;
            _slider.maxValue = max;
        };
    }
}
