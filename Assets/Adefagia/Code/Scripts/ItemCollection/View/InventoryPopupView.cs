using System;
using System.Threading.Tasks;
using UnityEngine;
using Adefagia.Store;

namespace Adefagia.ItemCollection
{
    public class InventoryPopupView : MonoBehaviour
    {
        public async Task Show()
        {
            gameObject.SetActive(true);

            await EconomyManager.instance.RefreshInventory();
        }

        public void OnCloseButtonPressed()
        {
            Close();
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
