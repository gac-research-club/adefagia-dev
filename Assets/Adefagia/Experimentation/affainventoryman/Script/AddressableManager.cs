using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private AssetReference optionScene;

    public static AsyncOperationHandle<SceneInstance> handlerOptionScene;

    // Update is called once per frame
    public void LoadAddressableOptionScene()
    {
        Addressables.LoadSceneAsync(optionScene, LoadSceneMode.Additive).Completed += (handle =>
        {
            handlerOptionScene = handle;
        });
    }

    public void ReleaseAddressableOptionScene()
    {
        Addressables.UnloadSceneAsync(handlerOptionScene);
    }
}