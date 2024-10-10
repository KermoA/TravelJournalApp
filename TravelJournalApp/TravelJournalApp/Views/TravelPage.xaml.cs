using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace TravelJournalApp.Views
{
    public partial class TravelPage : ContentPage
    {
        public TravelPage()
        {
            InitializeComponent();
            BindingContext = new TravelJournalListViewModel(); // ViewModel-i sidumine
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("TravelPage appeared - refreshing data.");
            (BindingContext as TravelJournalListViewModel)?.RefreshCommand.Execute(null);
        }

        private async void Add_Travel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTravelPage());
        }
    }
}
