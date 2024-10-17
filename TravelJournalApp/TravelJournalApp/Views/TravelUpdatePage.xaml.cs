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
	private List<string> selectedTempImagePaths = new List<string>(); 


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
				selectedTempImagePaths.Clear(); // Kustuta eelnev valik

				foreach (var image in pickResult)
				{
					// Säilita ajutine pildi path
					selectedTempImagePaths.Add(image.FullPath);

					// Näita eelnevaid selekteerituid pilte
					var imageViewModel = new ImageViewModel
					{
						FilePath = image.FullPath,
						ImageSource = ImageSource.FromFile(image.FullPath),
						IsSelected = true
					};
					ImageViewModels.Add(imageViewModel);
				}

				// Värskenda UI-d
				ImagesCollectionView.ItemsSource = ImageViewModels;
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error picking images: {ex.Message}";
		}
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
		foreach (var tempImagePath in selectedTempImagePaths)
		{
			var newFilePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(tempImagePath));

			try
			{
				// Kopeeri pit directory-sse
				File.Copy(tempImagePath, newFilePath, true);
				var imageTable = new ImageTable
				{
					TravelJournalId = travelJournal.Id,
					FilePath = newFilePath,
					ImageIndex = ImageViewModels.Count // Lisa uued pildid lõppu
				};

				// Salvesta pilt andmebaasi
				await _databaseContext.AddItemAsync(imageTable);
			}
			catch (Exception ex)
			{
				StatusLabel.Text = $"Error copying images: {ex.Message}";
				return;
			}
		}

		// Uuenda olemasolevad pildid ImageVieModeli põhjal
		foreach (var imageViewModel in ImageViewModels)
		{
			var imageTable = new ImageTable
			{
				TravelJournalId = travelJournal.Id,
				FilePath = imageViewModel.FilePath,
				IsSelected = imageViewModel.IsSelected,
				ImageIndex = ImageViewModels.IndexOf(imageViewModel)
			};

			// Salvesta või uuenda pilt andmebaasis
			await _databaseContext.SaveImageAsync(imageTable);
		}

		// Kontrolli uuendatud tulemust
		if (result)
		{
			StatusLabel.Text = "Travel updated successfully!";
			StatusLabel.TextColor = Color.FromRgba("#00525e");
			await Navigation.PopAsync();
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