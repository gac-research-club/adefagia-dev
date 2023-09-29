using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Adefagia.Store;

namespace Adefagia.ItemCollection
{
    public class CategoryButton : MonoBehaviour
    {
        public Button targetButton;
        public List<TextMeshProUGUI> textList;
        public Color defaultTextColor;
        public Material defaultTextMaterial;
        public Color activeTextColor;
        public Material activeTextMaterial;

        StoreManager m_VirtualShopSceneManager;

        public void Initialize(StoreManager virtualShopSceneManager, string category)
        {
            m_VirtualShopSceneManager = virtualShopSceneManager;
            foreach (TextMeshProUGUI text in textList)
            {
                text.text = category;
            }
            
        }

        public void UpdateCategoryButtonUIState(string selectedCategoryId)
        {
             foreach (TextMeshProUGUI text in textList)
            {
                targetButton.interactable = text.text != selectedCategoryId;
                text.color = text.text == selectedCategoryId ? activeTextColor : defaultTextColor;
                text.fontMaterial = text.text == selectedCategoryId ? activeTextMaterial : defaultTextMaterial;
            }
            
        }

        public void OnClick()
        {
            m_VirtualShopSceneManager.OnCategoryButtonClicked(textList[0].text);
        }
    }
}
