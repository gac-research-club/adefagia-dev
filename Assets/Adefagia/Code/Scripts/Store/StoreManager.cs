using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using Adefagia.ItemCollection;

namespace Adefagia.Store
{
    public class StoreManager : MonoBehaviour
    {
        const int k_EconomyPurchaseCostsNotMetStatusCode = 10504;

        public StoreView virtualShopSampleView;
        public Sprite defaultSprite;

        public async Task InitializeStart()
        {
            try
            {
                await UnityServices.InitializeAsync();

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (this == null) return;
                }

                Debug.Log($"Player id:{AuthenticationService.Instance.PlayerId}");

                await EconomyManager.instance.RefreshEconomyConfiguration();
                if (this == null) return;

                EconomyManager.instance.InitializeVirtualPurchaseLookup();

                // Note: We want these methods to use the most up to date configuration data, so we will wait to
                // call them until the previous two methods (which update the configuration data) have completed.
                await Task.WhenAll(AddressablesManager.instance.PreloadAllEconomySprites(),
                    RemoteConfigManager.instance.FetchConfigs(),
                    EconomyManager.instance.RefreshCurrencyBalances());
                if (this == null) return;

                // Read all badge addressables
                // Note: must be done after Remote Config values have been read (above).
                await AddressablesManager.instance.PreloadAllShopBadgeSprites(
                    RemoteConfigManager.instance.virtualShopConfig.categories);

                // Initialize all shops.
                // Note: must be done after all other initialization has completed (above).
                StoreController.instance.Initialize();

                virtualShopSampleView.Initialize(StoreController.instance.virtualShopCategories);

                var firstCategoryId = RemoteConfigManager.instance.virtualShopConfig.categories[0].id;
                if (!StoreController.instance.virtualShopCategories.TryGetValue(
                        firstCategoryId, out var firstCategory))
                {
                    throw new KeyNotFoundException($"Unable to find shop category {firstCategoryId}.");
                }

                virtualShopSampleView.ShowCategory(firstCategory);

                Debug.Log("Initialization and sign in complete.");

                EnablePurchases();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void EnablePurchases()
        {
            virtualShopSampleView.SetInteractable();
        }

        public void OnCategoryButtonClicked(string categoryId)
        {
            var virtualShopCategory = StoreController.instance.virtualShopCategories[categoryId];
            virtualShopSampleView.ShowCategory(virtualShopCategory);
        }

        public async Task OnPurchaseClicked(StoreItem virtualShopItem)
        {
            try
            {
                var result = await EconomyManager.instance.MakeVirtualPurchaseAsync(virtualShopItem.id);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
                if (this == null) return;

                ShowRewardPopup(result.Rewards);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void OnGainCurrencyDebugButtonClicked()
        {
            try
            {
                await EconomyManager.instance.GrantDebugCurrency("AURICH", 30);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void ShowRewardPopup(Rewards rewards)
        {
            var addressablesManager = AddressablesManager.instance;

            var rewardDetails = new List<RewardDetail>();
            foreach (var inventoryReward in rewards.Inventory)
            {
                var spriteInventory = addressablesManager.preloadedSpritesByEconomyId.ContainsKey(inventoryReward.Id) ? addressablesManager.preloadedSpritesByEconomyId[inventoryReward.Id] : defaultSprite;
                rewardDetails.Add(new RewardDetail
                {
                    id = inventoryReward.Id,
                    quantity = inventoryReward.Amount,
                    sprite = spriteInventory
                });
            }

            foreach (var currencyReward in rewards.Currency)
            {
                var spriteCurr = addressablesManager.preloadedSpritesByEconomyId.ContainsKey(currencyReward.Id) ? addressablesManager.preloadedSpritesByEconomyId[currencyReward.Id] : defaultSprite;
                rewardDetails.Add(new RewardDetail
                {
                    id = currencyReward.Id,
                    quantity = currencyReward.Amount,
                    sprite = spriteCurr
                });
            }

            virtualShopSampleView.ShowRewardPopup(rewardDetails);
        }
    }
}
