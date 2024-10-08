using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using TravelJournalApp.Models;
using Data;


namespace TravelJournalApp.Views
{
    public partial class TravelPage : ContentPage
    {
        private readonly DatabaseContext _databaseContext;

        public TravelPage()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext(); // Andmebaasi konteksti loomine
            LoadTravelEntries(); // Lae reisiandmed
            // S�ndmus kirje valimiseks
            TravelListView.ItemTapped += OnItemTapped;
        }


        private async void LoadTravelEntries()
        {
            var travels = await _databaseContext.GetAllAsync<TravelJournal>();
            TravelListView.ItemsSource = travels; // M��ra reisiandmed ListView jaoks
        }

        private async void Add_Travel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTravelPage());
        }

        // S�ndmus, mis avab DetailsPage
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var selectedTravel = e.Item as TravelJournal;

                // Avame DetailsPage ja edastame valitud reisi objektina
                await Navigation.PushAsync(new DetailsPage(selectedTravel));

                // Eemaldame valiku, et kasutaja n�eks valitud kirjet uuesti
                TravelListView.SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadTravelEntries(); // Lae reisiandmed iga kord, kui leht ilmub
        }
    }
}
