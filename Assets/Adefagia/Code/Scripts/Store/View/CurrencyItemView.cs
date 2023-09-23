
using System;
using TMPro;
using UnityEngine;

namespace Adefagia.Store
{
    public class CurrencyItemView : MonoBehaviour
    {
        public string definitionId;

        public TextMeshProUGUI balanceField;

        public void SetBalance(long balance)
        {
            balanceField.text = balance.ToString();
        }

    }
}