using System;
using System.Collections.Generic;
using UnityEngine;

namespace Adefagia.Store
{
    public class StoreController : MonoBehaviour
    {
        public static StoreController instance { get; private set; }

        public Dictionary<string, CategoryItem> virtualShopCategories { get; private set; }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public void Initialize()
        {
            virtualShopCategories = new Dictionary<string, CategoryItem>();

            foreach (var categoryConfig in RemoteConfigManager.instance.virtualShopConfig.categories)
            {
                var virtualShopCategory = new CategoryItem(categoryConfig);
                virtualShopCategories[categoryConfig.id] = virtualShopCategory;
            }
        }
    }
}
