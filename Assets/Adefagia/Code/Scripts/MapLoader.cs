using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private string mapName;
    [SerializeField] private Map map;

    public static bool successLoad;
    
    private void Start()
    {
        mapName = PlayerPrefs.GetString("Map");
        if (map.LoadMap(mapName))
        {
            successLoad = true;
            Debug.Log("Success Load Map");
        }
        else
        {
            Debug.LogWarning("Map not found");
        }
    }
}
