﻿using BertScout2019.Models;
using BertScout2019.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2019.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectEventTeamMatchPage : ContentPage
    {
        SelectEventTeamMatchesViewModel viewModel;

        public SelectEventTeamMatchPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectEventTeamMatchesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EventTeamMatchesListView.SelectedItem = null;
            MatchNumberLabelValue.Text = (App.highestMatchNumber + 1).ToString();
        }

        private async void EventTeamsListMatchView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as EventTeamMatch;
            if (item == null)
                return;

            App.currMatchNumber = item.MatchNumber;
            await Navigation.PushAsync(new EditEventTeamMatchPage(item));
        }

        private void AddMatch_Minus_Clicked(object sender, System.EventArgs e)
        {
            int value = int.Parse(MatchNumberLabelValue.Text);
            if (value > 1)
            {
                MatchNumberLabelValue.Text = (value - 1).ToString();
                App.highestMatchNumber = value;
            }
        }

        private void AddMatch_Plus_Clicked(object sender, System.EventArgs e)
        {
            int value = int.Parse(MatchNumberLabelValue.Text);
            if (value < 999)
            {
                MatchNumberLabelValue.Text = (value + 1).ToString();
                App.highestMatchNumber = value;
            }
        }

        private void AddNewMatch_Clicked(object sender, System.EventArgs e)
        {
            int value = int.Parse(MatchNumberLabelValue.Text);
            foreach (EventTeamMatch oldMatch in viewModel.Matches)
            {
                if (oldMatch.MatchNumber == value)
                {
                    return;
                }
            }
            EventTeamMatch newMatch = new EventTeamMatch();
            newMatch.EventKey = App.currFRCEventKey;
            newMatch.TeamNumber = App.currTeamNumber;
            newMatch.MatchNumber = value;
            App.database.SaveEventTeamMatchAsync(newMatch);
            if (App.highestMatchNumber < value)
            {
                App.highestMatchNumber = value;
            }
            // add new match into list in proper order
            bool found = false;
            for (int i = 0; i < viewModel.Matches.Count; i++)
            {
                if (viewModel.Matches[i].MatchNumber > value)
                {
                    found = true;
                    viewModel.Matches.Insert(i, newMatch);
                    break;
                }
            }
            if (!found)
            {
                viewModel.Matches.Add(newMatch);
            }
        }

        private async void TeamDetails_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TeamDetailsPage());
        }
    }
}