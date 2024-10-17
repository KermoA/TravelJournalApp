using System.Collections.ObjectModel;
using System.Diagnostics;
using TravelJournalApp.Data;
using TravelJournalApp.Models;


namespace TravelJournalApp.Views;

public partial class TravelUpdatePage : ContentPage
{
	private readonly DatabaseContext _databaseContext;
	private TravelViewModel _travelViewModel;
	public ObservableCollection<ImageViewModel> ImageViewModels { get; set; }


	public TravelUpdatePage(TravelViewModel travel)
	{
		InitializeComponent();
		BindingContext = travel;
		_databaseContext = new DatabaseContext();
		_travelViewModel = travel;

		LoadImagePreviews();
	}

	private void LoadImagePreviews()
	{
		ImageViewModels = new ObservableCollection<ImageViewModel>();
		foreach (var image in _travelViewModel.TravelImages)
		{
			if (File.Exists(image.FilePath)) // Check if the file exists
			{
				var imageViewModel = new ImageViewModel
				{
					FilePath = image.FilePath,
					ImageSource = ImageSource.FromFile(image.FilePath),
					IsSelected = image.IsSelected,
				};
				ImageViewModels.Add(imageViewModel);
			}
			else
			{
				Debug.WriteLine($"Image file not found: {image.FilePath}");
			}
		}
		ImagesCollectionView.ItemsSource = ImageViewModels;
	}



	private async void OnUpdateButtonClicked(object sender, EventArgs e)
	{
		var title = TitleEntry.Text;
		var description = DescriptionEditor.Text;
		var location = LocationEntry.Text;
		var startDate = DateStartEntry.Date;
		var endDate = DateEndEntry.Date;

		//Kui reisipealkiri tühi
		if (string.IsNullOrWhiteSpace(title))
		{
			TitleEntry.Text = "Title is required.";
			return;
		}

		// Muuda travelJournali sissekannet
		var travelJournal = new TravelJournalTable
		{
			Id = _travelViewModel.Id,
			Title = title,
			Description = description,
			Location = location,
			TravelStartDate = startDate,
			TravelEndDate = endDate,
			LastUpdatedAt = DateTime.Now
		};

		// Uuenda travelJournali andmebaasis
		bool result = await _databaseContext.UpdateItemAsync(travelJournal);

		// Uuenda pildid, mis on ImageViewModeli põhjal
		foreach (var imageViewModel in ImageViewModels)
		{
			var imageTable = new ImageTable
			{
				TravelJournalId = travelJournal.Id,
				FilePath = imageViewModel.FilePath,
				IsSelected = imageViewModel.IsSelected,
				ImageIndex = ImageViewModels.IndexOf(imageViewModel)
			};

			// Save or update the image in the database as needed
			await _databaseContext.SaveImageAsync(imageTable);
		}

		// Check the result of the update operation
		if (result)
		{
			StatusLabel.Text = "Travel updated successfully!";
			StatusLabel.TextColor = Color.FromRgba("#00525e");
			await Navigation.PopAsync(); // Navigate back to the previous page
		}
		else
		{
			StatusLabel.Text = "Failed to update travel.";
			StatusLabel.TextColor = Color.FromArgb("#FF6347");
		}
	}

	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}