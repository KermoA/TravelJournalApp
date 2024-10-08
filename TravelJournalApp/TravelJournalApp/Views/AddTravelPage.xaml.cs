using System;
using Microsoft.Maui.Controls;
using TravelJournalApp.Models;
using System.Threading.Tasks;
using Data;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using TravelJournalApp.Models;

namespace TravelJournalApp.Views
{
    public partial class AddTravelPage : ContentPage
    {
        private readonly DatabaseContext _databaseContext;
        private TravelJournal travelJournal;

        public AddTravelPage()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext(); // Andmebaasi konteksti loomine
            travelJournal = new TravelJournal();  // Initsialiseeri travelJournal siin
        }

        private async void OnSaveTravelClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text;
            var description = DescriptionEditor.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                StatusLabel.Text = "Title is required.";
                return;
            }

            // Uue reisi objekti loomine
            travelJournal = new TravelJournal
            {
                Id = Guid.NewGuid(), // Uus ID
                Title = title,
                Description = description,
                ImageFileId = travelJournal.ImageFileId
            };

            // Andmete lisamine andmebaasi
            bool result = await _databaseContext.AddItemAsync(travelJournal);
            if (result)
            {
                StatusLabel.Text = "Travel saved successfully!";
                StatusLabel.TextColor = Color.FromArgb("#00FF00");
                TitleEntry.Text = string.Empty;
                DescriptionEditor.Text = string.Empty;

                activityIndicator.IsRunning = true; // N‰ita edenemist
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
        private async void OnPickPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    // Salvesta foto faili tee
                    travelJournal.ImageFileId = photo.FullPath;

                    // Loo uus failinimi rakenduse kaustas
                    string newFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                    // Kopeeri fail uude asukohta
                    File.Copy(photo.FullPath, newFilePath);

                    // Salvesta uue faili tee
                    travelJournal.ImageFileId = newFilePath;

                    // Kuva valitud foto eelvaadet
                    PreviewImage.Source = ImageSource.FromFile(newFilePath); // Kasuta uut faili teed
                }
            }
            catch (Exception ex)
            {
                // K‰sitle erindeid (nt. juurdep‰‰suıiguste puudumine)
                StatusLabel.Text = $"Foto valimisel tekkis viga: {ex.Message}";
            }
        }
    }
}
