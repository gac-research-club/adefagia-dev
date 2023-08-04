using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private AssetReference managersScene;
    [SerializeField] private AssetReference menuToLoad;

    private void Start()
    {
        //Load the persistent managers scene
        managersScene.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<SceneInstance> obj)
    {
        SceneManager.UnloadSceneAsync(0);
    }
}
