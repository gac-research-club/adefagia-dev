using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLookAtCamer : MonoBehaviour
{
    /*---------------------------------------
     * Ui Script to look at the main camera
     *----------------------------------------*/
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
