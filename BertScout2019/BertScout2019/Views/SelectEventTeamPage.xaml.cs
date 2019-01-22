﻿using BertScout2019.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2019.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectEventTeamPage : ContentPage
    {
        SelectEventTeamsViewModel viewModel;

        public SelectEventTeamPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectEventTeamsViewModel();
        }

        private async void EventTeamsListView_ItemSelected(object sender, EventArgs e)
        {
            //todo
            await Navigation.PushAsync(new TeamMatchListPage());
        }
    }
}