using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TravelJournalApp.Data;
using Microsoft.Maui.Graphics;

namespace TravelJournalApp.Views
{
    public partial class TravelAddPage : ContentPage
    {
        private readonly DatabaseContext _databaseContext;
        private TravelJournalTable travelJournal;
        private List<string> selectedTempImagePaths = new List<string>(); // Temporary images path list
        private List<string> selectedImagePaths = new List<string>(); // To store selected image paths

        // Change the ObservableCollection to hold ImageOption objects
        public ObservableCollection<ImageOption> ImageOptions { get; set; } = new ObservableCollection<ImageOption>();
        private string _heroImageFile; // Property to store the hero image file path

        public TravelAddPage()
        {
            InitializeComponent();
            BindingContext = this;
            _databaseContext = new DatabaseContext();
            travelJournal = new TravelJournalTable();
        }

        private async void OnPickPhotosClicked(object sender, EventArgs e)
        {
            try
            {
                var pickResult = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select Images"
                });

                if (pickResult != null)
                {
                    ImageOptions.Clear(); // Clear previous selections
                    selectedImagePaths.Clear(); // Clear previous image paths
                    selectedTempImagePaths.Clear(); // Clear previous temp selections

                    foreach (var image in pickResult)
                    {
                        // Store the temporary image path
                        selectedTempImagePaths.Add(image.FullPath);

                        // Create an ImageOption for each image
                        ImageOptions.Add(new ImageOption
                        {
                            ImageSourcePath = image.FullPath,
                            IsHeroImage = false // Default to not selected as hero image
                        });
                    }

                    // Update the UI with the new ImageOptions
                    ImagesCollectionView.ItemsSource = ImageOptions;
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error picking images: {ex.Message}";
            }
        }

        private async void SaveTravelClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text?.Trim();
            var description = DescriptionEditor.Text?.Trim();
            var location = LocationEntry.Text?.Trim();
            var startDate = DateStartEntry.Date;
            var endDate = DateEndEntry.Date;

            if (string.IsNullOrWhiteSpace(title))
            {
                StatusLabel.Text = "Title is required.";
                return;
            }

            int heroImageIndex = -1; // Initialize with an invalid index
            foreach (var option in ImageOptions)
            {
                if (option.IsHeroImage)
                {
                    heroImageIndex = ImageOptions.IndexOf(option);
                    _heroImageFile = option.ImageSourcePath; // Save hero image path
                    break;
                }
            }

            // Check if temporary image paths are available
            if (selectedTempImagePaths.Count > 0)
            {
                foreach (var tempImagePath in selectedTempImagePaths)
                {
                    var newFilePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(tempImagePath));

                    try
                    {
                        File.Copy(tempImagePath, newFilePath, true);
                        selectedImagePaths.Add(newFilePath);
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = $"Error copying images: {ex.Message}";
                        return;
                    }
                }
            }

            // Create travel journal entry
            travelJournal = new TravelJournalTable
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                Location = location,
                TravelStartDate = startDate,
                TravelEndDate = endDate,
                //HeroImageFile = heroImageIndex, // Set HeroIndex here
                HeroImageFile = _heroImageFile // Save hero image file path
            };

            // Save journal entry and images
            bool journalSaved = await _databaseContext.AddItemAsync(travelJournal);
            bool imagesSaved = true;

            // Save images related to the travel journal
            for (int indexImages = 0; indexImages < selectedTempImagePaths.Count; indexImages++)
            {
                var tempImagePath = selectedTempImagePaths[indexImages];
                var newFilePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(tempImagePath));

                var travelJournalImage = new ImageTable
                {
                    Id = Guid.NewGuid(),
                    TravelJournalId = travelJournal.Id,
                    FilePath = newFilePath,
                    ImageIndex = indexImages
                };

                imagesSaved &= await _databaseContext.AddItemAsync(travelJournalImage);
            }

            // Update UI based on save result
            if (journalSaved && imagesSaved)
            {
                StatusLabel.Text = "Travel saved successfully!";
                StatusLabel.TextColor = Color.FromArgb("#00FF00");
                ClearInputs();
                await Task.Delay(2000);
                await Navigation.PopAsync();
            }
            else
            {
                StatusLabel.Text = "Failed to save travel.";
                StatusLabel.TextColor = Color.FromArgb("#FF0000");
                await Task.Delay(2000);
            }
        }

        private void ClearInputs()
        {
            TitleEntry.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;
            LocationEntry.Text = string.Empty;
            StatusLabel.Text = string.Empty; // Clear status label
            selectedTempImagePaths.Clear();
            selectedImagePaths.Clear();
            ImageOptions.Clear();
        }

        private bool _isUpdating = false; // Temporary flag to avoid recursion

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ImageOption selectedOption)
            {
                selectedOption.IsHeroImage = !selectedOption.IsHeroImage;

                foreach (var option in ImageOptions)
                {
                    if (option != selectedOption)
                    {
                        option.IsHeroImage = false;
                    }
                    // Värskenda ka teiste nuppude värvi:
                    option.OnPropertyChanged(nameof(option.ButtonBackgroundColor));
                }

                OnPropertyChanged(nameof(ImageOptions));
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
