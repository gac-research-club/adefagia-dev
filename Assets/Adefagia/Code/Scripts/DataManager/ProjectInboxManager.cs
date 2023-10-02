using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Adefagia.DataManager
{
    public class ProjectInboxManager : MonoBehaviour
    {
        public static int maxInboxSize { get; private set; }

        [SerializeField]
        ProjectInboxView projectInboxView;

        DateTime m_InboxLastCheckedTime;

        public async Task InitializeStart()
        {
            try
            {
                LocalSaveManager.Initialize();

                await InitializeUnityServices();

                // Check that scene has not been unloaded while processing async await to prevent throw.
                if (this == null) return;

                await FetchExistingInboxData();
                if (this == null) return;

                UpdateInboxState();

                projectInboxView.Initialize();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        async Task InitializeUnityServices()
        {
            await UnityServices.InitializeAsync();
            if (this == null) return;

            Debug.Log("Services Initialized.");

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signing in...");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (this == null) return;
            }

            LocalSaveManager.AddNewPlayerId(AuthenticationService.Instance.PlayerId);
            Debug.Log($"Player ID on Project Inbox Manager: {AuthenticationService.Instance.PlayerId}");
        }

        async Task FetchExistingInboxData()
        {
            // InitializeInboxState relies on DownloadContentCatalog which relies on FetchConfigs
            await RemoteConfigManager.FetchConfigs();
            if (this == null) return;

            await AddressablesManager.instance.DownloadContentCatalog();
            if (this == null) return;

            InboxStateManager.InitializeInboxState();

            maxInboxSize = RemoteConfigManager.maxInboxCount;
        }

        bool UpdateInboxState()
        {
            var inboxStateWasUpdated = InboxStateManager.UpdateInboxState();
            m_InboxLastCheckedTime = DateTime.Now;

            return inboxStateWasUpdated;
        }

        void Update()
        {
            // Checks that the lastCheckedTime is initialized and the inbox was last checked 1 minute ago or more
            if (m_InboxLastCheckedTime <= DateTime.MinValue || m_InboxLastCheckedTime > DateTime.Now.AddMinutes(-1))
            {
                return;
            }

            var inboxStateWasUpdated = UpdateInboxState();

            if (inboxStateWasUpdated)
            {
                projectInboxView.UpdateInboxView();
            }
        }

        public void DeleteMessage(string messageId)
        {
            InboxStateManager.DeleteMessage(messageId);
            projectInboxView.DeleteMessagePreview(messageId);
        }

        public void SelectMessage(MessagePreviewView messagePreviewView, InboxMessage message)
        {
            // SamplesEditorAnalytics.SendProjectInboxMessageOpenedEvent(message.messageId, localSaveManager.playerIdsLocalCache);

            Debug.Log(message.messageId);

            InboxStateManager.MarkMessageAsRead(message.messageId);
            projectInboxView.UpdateViewForNewMessageSelected(messagePreviewView, message);
        }
    }
}
