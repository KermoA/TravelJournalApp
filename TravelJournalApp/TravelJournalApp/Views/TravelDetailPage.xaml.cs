using TravelJournalApp.Data;
using TravelJournalApp.Models;

namespace TravelJournalApp.Views;

public partial class TravelDetailPage : ContentPage
{
    private List<string> _imageSources;
    private int _currentImageIndex;
    public TravelDetailPage(TravelViewModel travelViewModel)
    {
        InitializeComponent();
        BindingContext = travelViewModel;
    }

    private void OnImageTapped(object sender, EventArgs e)
    {
        // Get the tapped image source from the CommandParameter
        var tappedImage = sender as Image;
        var tapGesture = tappedImage.GestureRecognizers.OfType<TapGestureRecognizer>().FirstOrDefault();
        var imageSource = tapGesture?.CommandParameter?.ToString();

        if (imageSource != null)
        {
            // Set the image sources and current index
            _imageSources = (BindingContext as TravelViewModel)?.TravelImages.Select(img => img.FilePath).ToList();
            _currentImageIndex = _imageSources.IndexOf(imageSource);

            // Set the source of the zoomed image
            ZoomedImage.Source = imageSource;

            // Show the overlay
            ZoomOverlay.IsVisible = true;
        }
    }

    private void OnOverlayTapped(object sender, EventArgs e)
    {
        // Hide the overlay when tapped
        ZoomOverlay.IsVisible = false;
    }

    private void OnPreviousImageClicked(object sender, EventArgs e)
    {
        if (_imageSources != null && _currentImageIndex > 0)
        {
            _currentImageIndex--;
            ZoomedImage.Source = _imageSources[_currentImageIndex];
        }
    }

    private void OnNextImageClicked(object sender, EventArgs e)
    {
        if (_imageSources != null && _currentImageIndex < _imageSources.Count - 1)
        {
            _currentImageIndex++;
            ZoomedImage.Source = _imageSources[_currentImageIndex];
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        var travel = (TravelViewModel)BindingContext;
        await Navigation.PushAsync(new TravelUpdatePage(travel));
    }
    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var travel = (TravelViewModel)BindingContext;
        await Navigation.PushAsync(new TravelDeletePage(travel));
    }
}