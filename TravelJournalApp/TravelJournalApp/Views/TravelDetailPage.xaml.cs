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