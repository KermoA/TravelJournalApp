using System.Globalization;
using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace TravelJournalApp.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTravel = value as TravelJournalViewModel; // või whatever on sinu mudel

            if (selectedTravel != null && selectedTravel.Images.Any())
            {
                return selectedTravel.Images[0].ImagePath; // Tagasta esimese pildi tee
            }

            return "defaultImage.png"; // Vastasel juhul tagasta vaikimisi pilt
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

