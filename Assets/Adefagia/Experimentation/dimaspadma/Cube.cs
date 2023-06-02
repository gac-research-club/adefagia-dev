using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("Public Global")] public Global Global;
    
    [Header("Static Data from ScriptableObject")]
    public CubeData CubeData;

    private void Start()
    {
        if (Global != null)
        {
            ReferenceScriptableObject(Global.CubeDataGlobal);
            CubeData.ID = GetInstanceID().ToString();
            Debug.Log("Instance Cube" + CubeData.ID);
        }
    }

    public static T CreateOwnScriptableObject<T>() where T: CubeData
    {
        T cubeData = (T)ScriptableObject.CreateInstance<CubeData>();
        Debug.Log("Scriptable Instance: " + cubeData.GetInstanceID());
        cubeData.SizeScale = 1;

        return cubeData;
    }

    private void ReferenceScriptableObject(CubeData globalCubeData)
    {
        CubeData = globalCubeData;
        Debug.Log("Scriptable Instance Global: " + CubeData.GetInstanceID());
        CubeData.ID = GetInstanceID().ToString();
        CubeData.SizeScale = 1;
    }
}
