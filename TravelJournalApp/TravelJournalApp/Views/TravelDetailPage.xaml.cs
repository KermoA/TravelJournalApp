using System.Timers;
using TravelJournalApp.Data;
using TravelJournalApp.Models;
using Timer = System.Timers.Timer;

namespace TravelJournalApp.Views;

public partial class TravelDetailPage : ContentPage
{
    private Timer _scrollTimer;
    private bool _scrollingRight = true;
    private bool _isScrollingPaused = false;
    private List<string> _imageSources;
    private int _currentImageIndex;

    public TravelDetailPage(TravelViewModel travelViewModel)
    {
        InitializeComponent();
        BindingContext = travelViewModel;
        StartScrolling();
    }

    private void StartScrolling()
    {
        _scrollTimer = new Timer(20); // Adjust the interval as needed
        _scrollTimer.Elapsed += OnScrollTimerElapsed;
        _scrollTimer.Start();
    }

    private async void OnScrollTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Use Dispatcher to invoke the UI update on the main thread
        this.Dispatcher.Dispatch(async () =>
        {
            // If scrolling is paused, do not perform any scrolling
            if (_isScrollingPaused)
                return;

            // Get the current scroll position
            double currentScrollX = TitleScrollable.ScrollX;

            // Get the width of the content
            double contentWidth = ((Label)TitleScrollable.Content).Width;

            // Check if we need to reset the scrolling position
            if (_scrollingRight)
            {
                if (currentScrollX >= contentWidth - TitleScrollable.Width)
                {
                    // Pause scrolling and reset to the left edge
                    _isScrollingPaused = true;
                    await Task.Delay(1000); // 1 second delay
                    TitleScrollable.ScrollToAsync(0, 0, false);
                    _isScrollingPaused = false; // Resume scrolling
                }
                else
                {
                    TitleScrollable.ScrollToAsync(currentScrollX + 1, 0, false); // Scroll right
                }
            }
            else
            {
                if (currentScrollX <= 0)
                {
                    // Pause scrolling and reset to the right edge
                    _isScrollingPaused = true;
                    await Task.Delay(1000); // 1 second delay
                    TitleScrollable.ScrollToAsync(contentWidth, 0, false);
                    _isScrollingPaused = false; // Resume scrolling
                }
                else
                {
                    TitleScrollable.ScrollToAsync(currentScrollX - 1, 0, false); // Scroll left
                }
            }
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _scrollTimer?.Stop();
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
