using System;
using Microsoft.Maui.Controls;
using TravelJournalApp.Models;
using System.Threading.Tasks;
using Data;

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
                TravelNameEntry.Text = string.Empty; // Tühjenda sisendväljad
                DescriptionEditor.Text = string.Empty;

                // Tagasi TravelPage-le
                await Navigation.PopAsync(); // Tagasi eelmisele lehele
            }
            else
            {
                StatusLabel.Text = "Failed to save travel.";
            }
        }
    }
}
