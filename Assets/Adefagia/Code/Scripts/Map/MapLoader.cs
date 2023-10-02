using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.MapBase
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField]
        private string mapName;

        [SerializeField]
        private Map map;

        public static bool successLoad;

        private void OnEnable()
        {
            // mapName = PlayerPrefs.GetString("Map");
            // mapName = "adexe123123";
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
}
