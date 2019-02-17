﻿using BertScout2019Data.Models;
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
        public SyncDatabasePage()
        {
            InitializeComponent();
            Title = "Sync local data with website";
            Entry_IpAddress.Text = App.syncIpAddress;
        }

        private void Entry_IpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Sync_Clicked(object sender, EventArgs e)
        {
            // save ip address
            App.syncIpAddress = Entry_IpAddress.Text;
            // sync to database
            RunAsync();
        }

        private void RunAsync()
        {
            //List<EventTeamMatch> items;

            try
            {
                // Update port # in the following line.
                App.client.BaseAddress = new Uri(App.syncIpAddress);
                App.client.DefaultRequestHeaders.Accept.Clear();
                App.client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string result = "";
                HttpResponseMessage response = App.client.GetAsync("api/FRCEvents/1").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                Label_Results.Text = result;

                //// show all frcevents
                //Console.WriteLine("All FRC Events:");
                //items = await GetFRCEventsAsync();
                //foreach (FRCEvent showItem in items)
                //{
                //    ShowFRCEvent(showItem);
                //}
                //Console.WriteLine();

                //// Create a new FRCEvent
                //FRCEvent item = new FRCEvent
                //{
                //    Name = "Gizmo Event7",
                //    Location = "Anytown, ME",
                //    EventKey = "GIZMOS7",
                //    Changed = 1,
                //};

                //var url = await CreateFRCEventAsync(item);
                //Console.WriteLine($"Created at {url}");

                //// Get the FRCEvent
                //item = await GetFRCEventAsync(url.PathAndQuery);
                //ShowFRCEvent(item);

                //// show all frcevents
                //Console.WriteLine("All FRC Events:");
                //items = await GetFRCEventsAsync();
                //foreach (FRCEvent showItem in items)
                //{
                //    ShowFRCEvent(showItem);
                //}
                //Console.WriteLine();

                //EventTeamMatch eventTeamMatch = new EventTeamMatch();
                //eventTeamMatch.EventKey = "WEEKZERO";
                //eventTeamMatch.TeamNumber = 133;
                //eventTeamMatch.MatchNumber = 17;
                //eventTeamMatch.Changed = 1;

                //var urlEtm = await CreateEventTeamMatchAsync(eventTeamMatch);
                //Console.WriteLine($"Created at {urlEtm}");

                //// Update the FRCEvent
                //Console.WriteLine("Updating Location...");
                //item.Location = "Anothertown, ME";
                //await UpdateFRCEventAsync(item);

                //// Get the updated FRCEvent
                //item = await GetFRCEventAsync(url.PathAndQuery);
                //ShowFRCEvent(item);

                //// Delete the FRCEvent
                //var statusCode = await DeleteFRCEventAsync(item.Id);
                //Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception ex)
            {
                // todo any error here?
                Label_Results.Text = ex.Message;
            }
        }
    }
}