using System;
using Microsoft.Maui.Controls;
using TravelJournalApp.Models;
using System.Threading.Tasks;
using Data;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace TravelJournalApp.Views
{
    public partial class AddTravelPage : ContentPage
    {
        private readonly DatabaseContext _databaseContext;

        public AddTravelPage()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext(); // Andmebaasi konteksti loomine
        }

        private async void OnSaveTravelClicked(object sender, EventArgs e)
        {
            var travelName = TravelNameEntry.Text;
            var description = DescriptionEditor.Text;

            if (string.IsNullOrWhiteSpace(travelName))
            {
                StatusLabel.Text = "Travel name is required.";
                return;
            }

            // Uue reisi objekti loomine
            var travelJournal = new TravelJournal
            {
                Id = Guid.NewGuid(), // Uus ID
                TravelName = travelName,
                Description = description
            };

            // Andmete lisamine andmebaasi
            bool result = await _databaseContext.AddItemAsync(travelJournal);
            if (result)
            {
                StatusLabel.Text = "Travel saved successfully!";
                StatusLabel.TextColor = Color.FromArgb("#00FF00");
                TravelNameEntry.Text = string.Empty;
                DescriptionEditor.Text = string.Empty;

                activityIndicator.IsRunning = true; // Näita edenemist
                activityIndicator.IsVisible = true;

                await Task.Delay(2000);

                activityIndicator.IsRunning = false; // Peida edenemisindikaator
                activityIndicator.IsVisible = false;

                await Navigation.PopAsync();
            }
            else
            {
                StatusLabel.Text = "Failed to save travel.";
                StatusLabel.TextColor = Color.FromArgb("#FF0000");

                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                await Task.Delay(2000);

                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }

        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
