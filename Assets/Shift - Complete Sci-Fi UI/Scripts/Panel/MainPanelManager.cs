using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Shift
{
    public class MainPanelManager : MonoBehaviour
    {
        [Header("Panel List")]
        public List<PanelItem> panels = new List<PanelItem>();

        [Header("Settings")]
        [SerializeField] private bool useCulling = true;
        public int currentPanelIndex = 0;
        private int currentButtonIndex = 0;
        private int newPanelIndex;

        private GameObject currentPanel;
        private GameObject nextPanel;
        private GameObject currentButton;
        private GameObject nextButton;

        private Animator currentPanelAnimator;
        private Animator nextPanelAnimator;
        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        string panelFadeIn = "Panel In";
        string panelFadeOut = "Panel Out";
        string buttonFadeIn = "Normal to Pressed";
        string buttonFadeOut = "Pressed to Dissolve";
        string buttonFadeNormal = "Pressed to Normal";

        bool firstTime = true;

        [System.Serializable]
        public class PanelItem
        {
            public string panelName;
            public GameObject panelObject;
            public GameObject buttonObject;
        }

        void OnEnable()
        {
            if (firstTime == false && nextPanelAnimator != null && nextPanelAnimator.gameObject.activeInHierarchy)
            {
                nextPanelAnimator.Play(panelFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }

            else if (firstTime == false && currentPanelAnimator != null && currentPanelAnimator.gameObject.activeInHierarchy)
            {
                currentPanelAnimator.Play(panelFadeIn);
                currentButtonAnimator.Play(buttonFadeIn);
            }
        }

        void Awake()
        {
            currentButton = panels[currentPanelIndex].buttonObject;
            currentButtonAnimator = currentButton.GetComponent<Animator>();
            currentButtonAnimator.Play(buttonFadeIn);

            currentPanel = panels[currentPanelIndex].panelObject;
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            firstTime = false;

            if (useCulling == false)
                return;

            StartCoroutine("DisablePreviousPanel");
        }

        public void OpenFirstTab()
        {
            if (currentPanelIndex != 0)
                OpenPanel(panels[0].panelName);
        }

        public void OpenPanel(string newPanel)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelName == newPanel)
                {
                    newPanelIndex = i;
                    break;
                }
            }

            if (newPanelIndex != currentPanelIndex)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;
                currentPanelIndex = newPanelIndex;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.SetActive(true);

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                StartCoroutine("DisablePreviousPanel");

                currentButton = panels[currentButtonIndex].buttonObject;
                currentButtonIndex = newPanelIndex;
                nextButton = panels[currentButtonIndex].buttonObject;

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= panels.Count - 2)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;
                currentButton = panels[currentButtonIndex].buttonObject;
                nextButton = panels[currentButtonIndex + 1].buttonObject;
                currentPanel.gameObject.SetActive(true);

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeNormal);
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex += 1;
                currentButtonIndex += 1;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.gameObject.SetActive(true);

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;
                currentButton = panels[currentButtonIndex].buttonObject;
                nextButton = panels[currentButtonIndex - 1].buttonObject;
                currentPanel.gameObject.SetActive(true);

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeNormal);
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex -= 1;
                currentButtonIndex -= 1;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.gameObject.SetActive(true);

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        IEnumerator DisablePreviousPanel()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            for (int i = 0; i < panels.Count; i++)
            {
                if (i == currentPanelIndex)
                    continue;

                panels[i].panelObject.gameObject.SetActive(false);
            }
        }
    }
}