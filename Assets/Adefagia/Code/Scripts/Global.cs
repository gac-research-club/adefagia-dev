using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    [HideInInspector] public CubeData CubeDataGlobal;

    private void Start()
    {
        CubeDataGlobal = Cube.CreateOwnScriptableObject<CubeData>();
    }
}
