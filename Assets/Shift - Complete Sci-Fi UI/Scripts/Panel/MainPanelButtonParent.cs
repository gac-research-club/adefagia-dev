using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Shift
{
    public class MainPanelButtonParent : MonoBehaviour
    {
        private List<Animator> mainButtons = new List<Animator>();

        void Awake()
        {
            foreach (Transform child in transform) { mainButtons.Add(child.GetComponent<Animator>()); }
            for (int i = 0; i < mainButtons.Count; ++i) { mainButtons[i].Play("Normal to Dissolve"); }

            Canvas _canvas = gameObject.AddComponent<Canvas>();
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = 999;
            gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
    }
}