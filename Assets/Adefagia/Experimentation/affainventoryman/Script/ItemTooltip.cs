using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;

    private void Awake()
	{
		gameObject.SetActive(false);
	}

    public void ShowTooltip(Item item)
    {
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemDescriptionText.text = item.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
