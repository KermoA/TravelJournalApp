using DocumentFormat.OpenXml.Drawing.Charts;

namespace TravelJournalApp.Views;

public partial class StartPage : ContentPage
{
    int count = 0;
    public StartPage()
	{
		InitializeComponent();
	}

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    if (this.AnimationIsRunning("TransitionAnimation"))
    //    {
    //        return;
    //    }

    //    var parentAnimation = new Animation();

    //    //StartPageAnimation
    //    parentAnimation.Add(0, 0.2, new Animation(v => imgTest.Opacity = v, 0, 1, Easing.CubicIn));
    //    parentAnimation.Commit(this, "TransitionAnimation", 16, 3000, null, null);

    //    // Call TravelMain without parameters
    //    await TravelMain();
    //}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        var parentAnimation = new Animation();

        // Fade-in animation for opacity
        parentAnimation.Add(0, 0.1, new Animation(v => imgTest.Opacity = v, 0, 1, Easing.CubicIn));

        // Get screen width and height dynamically
        double screenWidth = 540;
        double screenHeight = 1000;

        // Airplane image width, assuming it's already sized
        double imgWidth = imgTest.WidthRequest;
        double imgHeight = imgTest.HeightRequest;

        parentAnimation.Add(0, 0.4, new Animation(v => imgIcon.Opacity = v, 0, 1, Easing.CubicIn));
        // First phase: Move the airplane image to the right, horizontally
        parentAnimation.Add(0.1, 1, new Animation(v =>
        {
            // Liiguta vasakule
            imgTest.TranslationX = screenWidth - v * 1.7;

            // Liiguta üles
            imgTest.TranslationY = screenHeight + imgHeight - v * 3.4 ;
        }, 0, screenWidth + imgWidth, Easing.Linear));

        parentAnimation.Commit(this, "TransitionAnimation", 10, 4000, null , null);

        await TravelMain();
    }

    async Task TravelMain()
    {
        // Oota 10 sekundit
        await Task.Delay(5000);

        // Suuna leht ümber
        Application.Current.MainPage = new NavigationPage(new Main());
    }
}