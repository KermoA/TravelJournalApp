using TravelJournalApp.Data;
using TravelJournalApp.Models;

namespace TravelJournalApp.Views;

public partial class TravelDetailPage : ContentPage
{
    public TravelDetailPage(TravelViewModel travelViewModel)
    {
        InitializeComponent();
        BindingContext = travelViewModel;
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}