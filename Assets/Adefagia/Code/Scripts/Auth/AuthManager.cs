using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEditor;

namespace Adefagia.Authentication
{
    public class AuthManager
    {

        private string _playerId;
        private string _accessToken;
        public string AccessToken => _accessToken;
        public string PlayerID => _playerId;

        // Setup authentication events handlers if desired
        public void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                // Shows how to get a playerID
                // Debug.Log($"Player ID : {AuthenticationService.Instance.PlayerId}");
                _playerId = AuthenticationService.Instance.PlayerId;

                // Shows how to get an access token
                // Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

                Debug.Log(AuthenticationService.Instance.Profile);

                _accessToken = AuthenticationService.Instance.AccessToken;
            };

            AuthenticationService.Instance.SignInFailed += (err) =>
            {
                EditorUtility.DisplayDialog(err.ToString(), "OK", "Cancel");
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("Player signed Out");
            };
        }



        public async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        public async Task SignInWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log("SignIn is successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }



        public async Task SignUpWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                Debug.Log("Username and Password has been registered");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }



        public async Task AddUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
                Debug.Log("Username and password added.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        public bool IsSignedIn()
        {
            return AuthenticationService.Instance.IsSignedIn;
        }

        public void SignOut()
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    AuthenticationService.Instance.SignOut();
                }
                Debug.Log("User has SignOut");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }

        }

    }


}
