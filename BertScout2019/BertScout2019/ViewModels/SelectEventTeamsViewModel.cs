﻿using BertScout2019.Models;
using BertScout2019.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BertScout2019.ViewModels
{
    public class SelectEventTeamsViewModel : BaseViewModel
    {
        public IDataStore<EventTeam> DataStoreEventTeam => new XmlDataStoreEventTeams();
        public IDataStore<Team> DataStoreTeam => new XmlDataStoreTeams();

        public List<EventTeam> EventTeams { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public Command LoadEventTeamsCommand { get; set; }

        public SelectEventTeamsViewModel()
        {
            App app = Application.Current as App;

            Title = app.CurrentFRCEvent;
            EventTeams = new List<EventTeam>();
            Teams = new ObservableCollection<Team>();
            ExecuteLoadEventTeamsCommand();
            //LoadEventTeamsCommand = new Command(async () => await ExecuteLoadEventTeamsCommand());
        }

        //async Task ExecuteLoadEventTeamsCommand()
        public void ExecuteLoadEventTeamsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            App app = Application.Current as App;

            try
            {
                EventTeams.Clear();
                Teams.Clear();
                var items = DataStoreEventTeam.GetItemsAsync(true).Result;
                var teams = DataStoreTeam.GetItemsAsync(true).Result;
                //var items = await DataStoreEventTeam.GetItemsAsync(true);
                //var teams = await DataStoreTeam.GetItemsAsync(true);
                foreach (var team in teams)
                {
                    foreach (var item in items)
                    {
                        if (item.EventID == app.CurrentFRCEventID
                            && item.TeamID == team.Id)
                        {
                            Teams.Add(team);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
