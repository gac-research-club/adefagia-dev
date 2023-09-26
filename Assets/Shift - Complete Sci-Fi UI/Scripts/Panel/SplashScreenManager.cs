using UnityEngine;

namespace Michsky.UI.Shift
{
    public class SplashScreenManager : MonoBehaviour
    {
        [Header("Resources")]
        public GameObject splashScreen;
        public GameObject mainPanels;

        private Animator splashScreenAnimator;
        private Animator mainPanelsAnimator;
        private TimedEvent ssTimedEvent;

        [Header("Settings")]
        public bool disableSplashScreen;
        public bool enablePressAnyKeyScreen;
        public bool enableLoginScreen;
        public bool showOnlyOnce = true;

        MainPanelManager mpm;

        void OnEnable()
        {
            if (showOnlyOnce && GameObject.Find("[Shift UI - Splash Screen Helper]") != null) { disableSplashScreen = true; }
            if (splashScreenAnimator == null) { splashScreenAnimator = splashScreen.GetComponent<Animator>(); }
            if (ssTimedEvent == null) { ssTimedEvent = splashScreen.GetComponent<TimedEvent>(); }
            if (mainPanelsAnimator == null) { mainPanelsAnimator = mainPanels.GetComponent<Animator>(); }
            if (mpm == null) { mpm = gameObject.GetComponent<MainPanelManager>(); }

            if (disableSplashScreen == true)
            {
                splashScreen.SetActive(false);
                mainPanels.SetActive(true);

                mainPanelsAnimator.Play("Start");
                mpm.OpenFirstTab();
            }

            if (enableLoginScreen == false && enablePressAnyKeyScreen == true && disableSplashScreen == false)
            {
                splashScreen.SetActive(true);
                mainPanelsAnimator.Play("Invisible");
            }

            if (enableLoginScreen == true && enablePressAnyKeyScreen == true && disableSplashScreen == false)
            {
                splashScreen.SetActive(true);
                mainPanelsAnimator.Play("Invisible");
            }

            if (enableLoginScreen == true && enablePressAnyKeyScreen == false && disableSplashScreen == false)
            {
                splashScreen.SetActive(true);
                mainPanelsAnimator.Play("Invisible");
                splashScreenAnimator.Play("Login");
            }

            if (enableLoginScreen == false && enablePressAnyKeyScreen == false && disableSplashScreen == false)
            {
                splashScreen.SetActive(true);
                mainPanelsAnimator.Play("Invisible");
                splashScreenAnimator.Play("Loading");
                ssTimedEvent.StartIEnumerator();
            }

            if (showOnlyOnce == true)
            {
                GameObject tempHelper = new GameObject();
                tempHelper.name = "[Shift UI - Splash Screen Helper]";
                DontDestroyOnLoad(tempHelper);
            }
        }

        public void LoginScreenCheck()
        {
            if (enableLoginScreen == true && enablePressAnyKeyScreen == true)
                splashScreenAnimator.Play("Press Any Key to Login");

            else if (enableLoginScreen == false && enablePressAnyKeyScreen == true)
            {
                splashScreenAnimator.Play("Press Any Key to Loading");
                ssTimedEvent.StartIEnumerator();
            }

            else if (enableLoginScreen == false && enablePressAnyKeyScreen == false)
            {
                splashScreenAnimator.Play("Loading");
                ssTimedEvent.StartIEnumerator();
            }
        }
    }
}