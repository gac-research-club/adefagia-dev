using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    public void ChangeResolutionHD()
    {
        Screen.SetResolution(1080, 720, true);
        Debug.Log($"Res: {Screen.currentResolution.width} x {Screen.currentResolution.height}");
    }
    
    public void ChangeResolutionFHD()
    {
        Screen.SetResolution(1920, 1080, true);
        Debug.Log($"Res: {Screen.currentResolution.width} x {Screen.currentResolution.height}");
    }
}
