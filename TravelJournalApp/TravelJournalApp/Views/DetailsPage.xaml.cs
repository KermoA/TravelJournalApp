using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace TravelJournalApp.Views;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(TravelJournal selectedTravel)
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}