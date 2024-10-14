using TravelJournalApp.Data;

namespace TravelJournalApp.Views;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(TravelJournalTable selectedTravel)
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}