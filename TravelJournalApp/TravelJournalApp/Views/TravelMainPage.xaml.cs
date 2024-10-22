using DocumentFormat.OpenXml.InkML;
using System.Diagnostics;
using System.Timers;
using TravelJournalApp.Models;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TravelJournalApp.Views
{
    public partial class TravelMainPage : ContentPage
    {
        private Timer _scrollTimer;
        private bool _scrollingRight = true;
        private bool _isScrollingPaused = false;
        public ListViewModel Vm => BindingContext as ListViewModel;

        public TravelMainPage()
        {
            InitializeComponent();

            // ViewModel-i sidumine
            BindingContext = new ListViewModel();
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

                if (_scrollingRight)
                {
                    if (currentScrollX >= contentWidth - TitleScrollable.Width)
                    {
                        _isScrollingPaused = true;
                        await Task.Delay(1000);
                        TitleScrollable.ScrollToAsync(0, 0, false);
                        _isScrollingPaused = false;
                    }
                    else
                    {
                        TitleScrollable.ScrollToAsync(currentScrollX + 1, 0, false);
                    }
                }
                else
                {
                    if (currentScrollX <= 0)
                    {
                        _isScrollingPaused = true;
                        await Task.Delay(1000);
                        TitleScrollable.ScrollToAsync(contentWidth, 0, false);
                        _isScrollingPaused = false;
                    }
                    else
                    {
                        await Task.Delay(1000);
                        TitleScrollable.ScrollToAsync(currentScrollX - 1, 0, false);
                    }
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("TravelPage appeared - refreshing data.");
            Vm?.RefreshCommand.Execute(null);
        }

        private async void Add_Travel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TravelAddPage());
        }

    }
}
