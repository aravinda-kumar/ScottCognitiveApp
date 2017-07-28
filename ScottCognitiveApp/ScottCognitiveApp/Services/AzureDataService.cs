using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using ScottCognitiveApp.Models;
using ScottCognitiveApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureDataService))]
namespace ScottCognitiveApp.Services
{
    public class AzureDataService
    {
        public MobileServiceClient MobileClient { get; set; } = null;
        IMobileServiceSyncTable<ChatMessage> messageTable;
        

        public async Task Initialize()
        {
            if (MobileClient?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://scottbot04.azurewebsites.net";
            

            MobileClient = new MobileServiceClient(appUrl);

            //InitialzeDatabase for path
            var path = InitializeDatabase();
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<ChatMessage>();

            //Initialize SyncContext
            await MobileClient.SyncContext.InitializeAsync(store);

            //Get our sync table that will call out to azure
            messageTable = MobileClient.GetSyncTable<ChatMessage>();
            
            
        }

        private string InitializeDatabase()
        {
#if __ANDROID__ || __IOS__
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
#endif
            SQLitePCL.Batteries.Init();

            var path = "syncstore.db";

#if __ANDROID__
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), path);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
#endif

            return path;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessages()
        {
            await Initialize();
            await SyncMessage();
            return await messageTable.ToEnumerableAsync();
        }

        public async Task<ChatMessage> GetMessage(string id)
        {
            await Initialize();

            await SyncMessage();

            return await messageTable.LookupAsync(id);
        }

        public async Task<ChatMessage> AddMessage(ChatMessage message)
        {
            await Initialize();
            
            await messageTable.InsertAsync(message);
            await SyncMessage();
            return message;
        }

        public async Task UpdateMessage(ChatMessage message)
        {
            await Initialize();

            await messageTable.UpdateAsync(message);
            await SyncMessage();
        }

        public async Task RemoveMessage(ChatMessage message) 
        {
            await Initialize();

            await messageTable.DeleteAsync(message);
            await SyncMessage();
        }


        public async Task SyncMessage()
        {
            await Initialize();

            try
            {

                await messageTable.PullAsync($"allMessage", messageTable.CreateQuery());
                await MobileClient.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during Sync occurred: {ex.Message}");
            }
        }

    }
}
