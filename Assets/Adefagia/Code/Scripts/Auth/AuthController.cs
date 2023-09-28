using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Adefagia.DataManager;

namespace Adefagia.Authentication
{
    public class AuthController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField  usernameTxt;
        [SerializeField] private TMP_InputField  passwordTxt;

        private async void Start()
        {
            
            // UnityServices.InitializeAsync() will initialize all service that subscribed to core
            try
            {
                await UnityServices.InitializeAsync();

                // Check that scene has not been unloaded while processing async await to prevent throw.
                if (this == null) return;

                SetupEvents();

                // await authManager.SignInAnonymouslyAsync();

                // Check that scene has not been unloaded while processing async await to prevent throw.
                if (this == null) return;

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        public void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                // Shows how to get a playerID
                Debug.Log($"Player ID : {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += (err) =>
            {
                EditorUtility.DisplayDialog("Login Failed!", err.ToString(), "OK");
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("Player signed Out");
            };
        }


        public async Task SignInMethod()
        {
            string username = usernameTxt.text.ToString();
            string password = passwordTxt.text.ToString();

            // username = "adexe123";
            // password = "@Dexe123";
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            await Task.Delay(1000);
        }

        public async void AnonSignInClick()
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }
}