using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Adefagia.DataManager;
using Adefagia.Authentication;
using Adefagia.Store;

namespace Michsky.UI.Shift
{
    public class TimedEvent : MonoBehaviour
    {
        [Header("Timing (seconds)")]
        public float timer = 4;
        public bool enableAtStart;

        [Header("Timer Event")]
        public UnityEvent timerAction;

        public ProjectInboxManager projectInboxManager;
        public StoreManager storeManager;

        private AuthController authController;

        void Start()
        {
            if (enableAtStart == true)
            {
                StartCoroutine("TimedEventStart");
            }
            authController = GetComponent<AuthController>();
        }

        IEnumerator TimedEventStart()
        {
            authController.SignInMethod();
            yield return new WaitForSeconds(timer);
            timerAction.Invoke();
        }

        public async void LoginEventStart()
        {
            await authController.SignInMethod();
            await projectInboxManager.InitializeStart();
            await storeManager.InitializeStart();
            timerAction.Invoke();
        }

        public void StartIEnumerator()
        {
            StartCoroutine("TimedEventStart");
        }

        public void StopIEnumerator()
        {
            StopCoroutine("TimedEventStart");
        }
    }
}
