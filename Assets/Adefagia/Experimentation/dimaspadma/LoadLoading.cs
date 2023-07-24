using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadLoading : MonoBehaviour
{
    [SerializeField] private AssetReference scene;

    public static AsyncOperationHandle<SceneInstance> handleLoading;

    private void Start()
    {
        LoadAddressableScene.Loading += LoadAsyncLoadingScene;
        LoadAddressableScene.Unloading += UnloadAsyncLoadingScene;
    }
    
    public void LoadAsyncLoadingScene()
    {
        scene.LoadSceneAsync(LoadSceneMode.Additive).Completed += (handle) =>
        {
            handleLoading = handle;
            LoadAddressableScene.isInvokeLoad = false;
        };
    }

    public void UnloadAsyncLoadingScene()
    {
        Addressables.UnloadSceneAsync(handleLoading).Completed += (_) =>
        {
            LoadAddressableScene.isInvokeUnload = false;
        };
    }
}
