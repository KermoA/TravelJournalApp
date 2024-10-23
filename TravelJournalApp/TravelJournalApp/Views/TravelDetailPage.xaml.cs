using System.Diagnostics;
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
    private int _currentImageIndex = 0;

    public TravelDetailPage(TravelViewModel travelViewModel)
    {
        InitializeComponent();
        BindingContext = travelViewModel;
        StartScrolling();
    }

    private void StartScrolling()
    {
        _scrollTimer = new Timer(20);
        _scrollTimer.Elapsed += OnScrollTimerElapsed;
        _scrollTimer.Start();
    }

    private async void OnScrollTimerElapsed(object sender, ElapsedEventArgs e)
    {
        this.Dispatcher.Dispatch(async () =>
        {
            if (_isScrollingPaused)
                return;

            double currentScrollX = TitleScrollable.ScrollX;
            double contentWidth = ((Label)TitleScrollable.Content).Width;

            // Define how much you want to scroll on each timer tick
            double scrollIncrement = 0.8; // Smaller increments for slower scrolling
            int delayBetweenScrolls = 100; // Increase the delay for slower movement

            if (_scrollingRight)
            {
                if (currentScrollX >= contentWidth - TitleScrollable.Width)
                {
                    _isScrollingPaused = true;
                    await Task.Delay(1000); // Optional pause when reaching the end
                    TitleScrollable.ScrollToAsync(0, 0, false); // Reset scroll to the beginning
                    _isScrollingPaused = false;
                }
                else
                {
                    TitleScrollable.ScrollToAsync(currentScrollX + scrollIncrement, 0, false);
                    await Task.Delay(delayBetweenScrolls); // Slow down the scrolling
                }
            }
            else
            {
                if (currentScrollX <= 0)
                {
                    _isScrollingPaused = true;
                    await Task.Delay(1000); // Optional pause when reaching the start
                    TitleScrollable.ScrollToAsync(contentWidth, 0, false); // Scroll to the end
                    _isScrollingPaused = false;
                }
                else
                {
                    await Task.Delay(1000);
                    TitleScrollable.ScrollToAsync(currentScrollX - scrollIncrement, 0, false);
                    await Task.Delay(delayBetweenScrolls); // Slow down the scrolling
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
		var tappedImage = sender as Image;
		var tapGesture = tappedImage.GestureRecognizers.OfType<TapGestureRecognizer>().FirstOrDefault();
		var imageSource = tapGesture?.CommandParameter?.ToString();

		if (imageSource != null)
		{
			_imageSources = (BindingContext as TravelViewModel)?.TravelImages.Select(img => img.FilePath).ToList();
			_currentImageIndex = _imageSources.IndexOf(imageSource);

			ZoomedImage.Source = imageSource;

			UpdateImageIndexLabel();

			ZoomOverlay.IsVisible = true;
		}
	}

	private void UpdateImageIndexLabel()
	{
		ImageIndexLabel.Text = $"{_currentImageIndex + 1} / {_imageSources.Count}";
	}

	private void OnOverlayTapped(object sender, EventArgs e)
	{
		ZoomOverlay.IsVisible = false;
	}

	private void OnPreviousImageClicked(object sender, EventArgs e)
	{
		if (_imageSources != null && _currentImageIndex > 0)
		{
			_currentImageIndex--;
			ZoomedImage.Source = _imageSources[_currentImageIndex];
			UpdateImageIndexLabel();
		}
	}

	private void OnNextImageClicked(object sender, EventArgs e)
	{
		if (_imageSources != null && _currentImageIndex < _imageSources.Count - 1)
		{
			_currentImageIndex++;
			ZoomedImage.Source = _imageSources[_currentImageIndex];
			UpdateImageIndexLabel();
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
