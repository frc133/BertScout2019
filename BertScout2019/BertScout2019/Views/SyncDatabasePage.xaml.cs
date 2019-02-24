﻿using BertScout2019.Services;
using BertScout2019Data.Models;
using Common.JSON;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2019.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncDatabasePage : ContentPage
    {
        public IDataStore<EventTeamMatch> SqlDataEventTeamMatches;
        public IDataStore<EventTeamMatch> WebDataEventTeamMatches;

        private readonly string _mediaType = "application/json";

        private static bool _initialSetup = false;
        private static bool _isBusy = false;

        public SyncDatabasePage()
        {
            InitializeComponent();
            Title = "Sync local data with website";
            Entry_IpAddress.Text = App.syncIpAddress;
            SqlDataEventTeamMatches = new SqlDataStoreEventTeamMatches(App.currFRCEventKey);
            WebDataEventTeamMatches = new WebDataStoreEventTeamMatches();
        }

        private void PrepareSync()
        {
            if (!_initialSetup)
            {
                _initialSetup = true;

                // save ip address
                App.syncIpAddress = Entry_IpAddress.Text;
                Entry_IpAddress.IsEnabled = false;

                string uri = App.syncIpAddress;
                if (!uri.EndsWith("/"))
                {
                    uri += "/";
                }
                if (!uri.Contains(":"))
                {
                    uri += "bertscout2019/";
                }
                if (!uri.StartsWith("http"))
                {
                    uri = $"http://{uri}";
                }
                if (!uri.EndsWith("/"))
                {
                    uri += "/";
                }

                App.client = new HttpClient(); ;
                App.client.BaseAddress = new Uri(uri);
                App.client.DefaultRequestHeaders.Accept.Clear();
                App.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            }
        }

        private void Button_Upload_Clicked(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }
            _isBusy = true;
            PrepareSync();

            Label_Results.Text = "Uploading data...";
            int addedCount = 0;
            int updatedCount = 0;

            List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

            foreach (EventTeamMatch item in matches)
            {
                if (item.Changed % 2 == 1)
                {
                    item.Changed++; // change odd to even = no upload next time
                    if (item.Changed <= 2) // first time sending
                    {
                        WebDataEventTeamMatches.AddItemAsync(item);
                        addedCount++;
                    }
                    else
                    {
                        WebDataEventTeamMatches.UpdateItemAsync(item);
                        updatedCount++;
                    }
                    // save it so .Changed is updated
                    App.database.SaveEventTeamMatchAsync(item);
                }
            }

            Label_Results.Text += $"\n\nAdded: {addedCount} - Updated: {updatedCount}";

            _isBusy = false;
        }

        private void Button_Download_Clicked(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }
            _isBusy = true;

            PrepareSync();

            int addedCount = 0;
            int updatedCount = 0;
            int notChangedCount = 0;

            Label_Results.Text = "Downloading data...";

            HttpResponseMessage response = App.client.GetAsync("api/EventTeamMatches").Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                JArray results = JArray.Parse(result);
                foreach (JObject obj in results)
                {
                    EventTeamMatch oldItem = null;
                    EventTeamMatch item = EventTeamMatch.Parse(obj.ToString());

                    oldItem = SqlDataEventTeamMatches.GetItemAsync(item.Uuid).Result;

                    if (oldItem == null)
                    {
                        SqlDataEventTeamMatches.AddItemAsync(item);
                        addedCount++;
                    }
                    else if (oldItem.Changed < item.Changed)
                    {
                        SqlDataEventTeamMatches.UpdateItemAsync(item);
                        updatedCount++;
                    }
                    else
                    {
                        notChangedCount++;
                    }
                }
            }

            Label_Results.Text += $"\n\nAdded: {addedCount} - Updated: {updatedCount} - Not Changed: {notChangedCount}";

            _isBusy = false;
        }
    }
}
