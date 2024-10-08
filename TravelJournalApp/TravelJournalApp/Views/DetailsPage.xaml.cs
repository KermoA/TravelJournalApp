using TravelJournalApp.Models;

namespace TravelJournalApp.Views;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(TravelJournal selectedTravel)
    {
        InitializeComponent();

        // Kuvame valitud reisi andmed
        //TitleLabel.Text = selectedTravel.Title;
        //DescriptionLabel.Text = selectedTravel.Description;

        // Kui pilt on olemas, kuvame selle
        if (!string.IsNullOrEmpty(selectedTravel.ImageFileId))
        {
            //TravelImage.Source = ImageSource.FromFile(selectedTravel.ImageFileId);
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}