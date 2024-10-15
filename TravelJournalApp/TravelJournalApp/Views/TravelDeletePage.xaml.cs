using TravelJournalApp.Data;
using TravelJournalApp.Models;


namespace TravelJournalApp.Views
{
    public partial class TravelDeletePage : ContentPage
    {
        public TravelDeletePage(TravelViewModel travel)
        {
            InitializeComponent();
            BindingContext = travel;
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
