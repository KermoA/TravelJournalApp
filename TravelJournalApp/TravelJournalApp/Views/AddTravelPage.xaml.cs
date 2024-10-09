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
        private TravelJournal travelJournal;
        private string tempImagePath; // To store the temporary image path
        private string newFilePath;

        public AddTravelPage()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
            travelJournal = new TravelJournal();
        }

        private async void OnPickPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    // Store the temporary image path
                    tempImagePath = photo.FullPath;

                    // Display a preview of the original photo
                    PreviewImage.Source = ImageSource.FromFile(tempImagePath);
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Foto valimisel tekkis viga: {ex.Message}";
            }
        }

        private async void SaveTravelClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text;
            var description = DescriptionEditor.Text;
            var location = LocationEntry.Text;


            if (string.IsNullOrWhiteSpace(title))
            {
                StatusLabel.Text = "Title is required.";
                return;
            }

            // Kontrollime, kas ajutine pildi tee on olemas
            if (!string.IsNullOrEmpty(tempImagePath))
            {
                newFilePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(tempImagePath));

                // Kopeerime pildi sihtkohta ainult siis, kui pilt on valitud
                File.Copy(tempImagePath, newFilePath, true);

                // M‰‰rame kopeeritud faili teekonna
                travelJournal.ImageFileId = newFilePath;
            }

            travelJournal = new TravelJournal
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                Location = location,
                ImageFileId = newFilePath // Siin m‰‰rame kopeeritud faili tee
            };

            // Salvestame andmebaasi
            bool result = await _databaseContext.AddItemAsync(travelJournal);

            if (result)
            {
                StatusLabel.Text = "Travel saved successfully!";
                StatusLabel.TextColor = Color.FromArgb("#00FF00");
                TitleEntry.Text = string.Empty;
                DescriptionEditor.Text = string.Empty;

                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                await Task.Delay(2000);

                activityIndicator.IsRunning = false;
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

        private async void DescriptionEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            await MyScrollView.ScrollToAsync(DescriptionEditor, ScrollToPosition.End, true);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}