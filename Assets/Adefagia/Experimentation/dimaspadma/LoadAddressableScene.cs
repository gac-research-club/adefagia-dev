using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadAddressableScene : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private AssetReference scene;
    [SerializeField] private bool isSingleMode = true;
    [SerializeField] private bool loadOnAwake = true;

    public static UnityAction<float, float, float> UpdateSlider;
    public static UnityAction Loading, Unloading;
    
    private AsyncOperationHandle<SceneInstance> _handle;

    private bool loadingLoad, loadingUnload;
    public static bool isInvokeLoad, isInvokeUnload;

    void Awake()
    {
        if (loadOnAwake)
        {
            LoadAsyncScene();
        }
    }

    private void Update()
    {
        if (!_handle.IsDone)
        {
            UpdateSlider?.Invoke(0f, _handle.PercentComplete, 1f);
        }

        // ========== Loading ===============
        if (loadingUnload || loadingLoad)
        {
            if (!isInvokeLoad)
            {
                isInvokeLoad = true;
                Debug.Log("test loading...");
                // Loading?.Invoke();
            }
        }

        if (!loadingLoad && !loadingUnload)
        {
            if (!isInvokeUnload)
            {
                isInvokeUnload = true;
                Debug.Log("test unload!");
                // Unloading?.Invoke();
            }
        }
        // ====================================
    }

    public void LoadAsyncScene(int idTarget)
    {
        if(idTarget != id) return;
        LoadAsyncScene();
    }
    
    public void LoadAsyncScene()
    {
        loadingLoad = true;
        
        _handle = scene.LoadSceneAsync(isSingleMode ? LoadSceneMode.Single : LoadSceneMode.Additive);
        _handle.Completed += (_) =>
        {
            loadingLoad = false;
        };
    }
    
    public void UnloadAsyncScene(int idTarget)
    {
        if(idTarget != id) return;
        UnloadAsyncScene(_handle);
    }

    public void UnloadAsyncScene(AsyncOperationHandle<SceneInstance> handle)
    {
        loadingUnload = true;
        Addressables.UnloadSceneAsync(handle).Completed += (_) =>
        {
            loadingUnload = false;
        };
    }
    
}
