using System.Collections;
using System.Collections.Generic;
using Adefagia;
using UnityEngine;

public class UiLookAtCamer : MonoBehaviour
{
    /*---------------------------------------
     * Ui Script to look at the main camera
     *----------------------------------------*/
    private void LateUpdate()
    {
        var mainCamera = GameManager.instance.gridManager.mainCamera;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
