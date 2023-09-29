using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Adefagia.Authentication
{
    public class ProfileController : MonoBehaviour
    {
        [SerializeField] private Button logoutButton;
        [SerializeField] private GameObject profileCard;

        private void Start()
        {

        }

        private void Update()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                SceneManager.LoadScene("LoginForm");
                if (this == null) return;
            }
        }


        public void SignOutClick()
        {
            AuthenticationService.Instance.SignOut();
        }
    }
}