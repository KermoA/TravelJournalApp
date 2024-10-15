using TravelJournalApp.Models;

namespace TravelJournalApp.Views;

public partial class TravelUpdatePage : ContentPage
{
    public TravelUpdatePage(TravelViewModel travel)
    {
        InitializeComponent();
        BindingContext = travel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}