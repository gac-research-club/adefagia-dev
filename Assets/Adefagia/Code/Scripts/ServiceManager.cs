using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

using Adefagia.Authentication;

namespace Adefagia
{
    public class ServiceManager : MonoBehaviour
    {
        public static ServiceManager Instance;
        // Service Unity Manager
        public AuthManager authManager;

        private async void Awake()
        {
            authManager = new AuthManager();
            // UnityServices.InitializeAsync() will initialize all service that subscribed to core
            try
            {
                await UnityServices.InitializeAsync();
                // Debug.Log(UnityServices.State);

                authManager.SetupEvents();

                await authManager.SignInAnonymouslyAsync();


                Singleton();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Singleton()
        {
            // Ensure only one Instance is in the hierarchy
            // If there is no instance then do Instantiation
            if (Instance == null)
            {
                Instance = this;
            }
            // If already instantiation, destroy new duplicate singleton
            // to make sure only one
            else
            {
                Destroy(this);
            }

            // Can access from any scene
            DontDestroyOnLoad(Instance);
        }


    }
}
