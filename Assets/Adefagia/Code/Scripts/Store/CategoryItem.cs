using System;
using System.Collections.Generic;

namespace Adefagia.Store
{
    public class CategoryItem
    {
        public string id { get; private set; }
        public bool enabledFlag { get; private set; }
        public List<StoreItem> virtualShopItems { get; private set; }

        public CategoryItem(RemoteConfigManager.CategoryConfig categoryConfig)
        {
            id = categoryConfig.id;
            enabledFlag = categoryConfig.enabledFlag;
            virtualShopItems = new List<StoreItem>();

            foreach (var item in categoryConfig.items)
            {
                virtualShopItems.Add(new StoreItem(item));
            }
        }

        public override string ToString()
        {
            return $"\"{id}\" enabled:{enabledFlag} items:{virtualShopItems?.Count}";
        }
    }
}
