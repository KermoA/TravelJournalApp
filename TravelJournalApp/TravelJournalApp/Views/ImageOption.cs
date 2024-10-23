using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelJournalApp.Views
{
    public class ImageOption : INotifyPropertyChanged
    {
        private bool _isHeroImage;

        public string ImageSourcePath { get; set; }

        public ImageSource ImageSource => ImageSource.FromFile(ImageSourcePath);

        public bool IsHeroImage
        {
            get => _isHeroImage;
            set
            {
                if (_isHeroImage != value)
                {
                    _isHeroImage = value;
                    OnPropertyChanged(nameof(IsHeroImage));
                    OnPropertyChanged(nameof(ButtonLabel)); // Notify that the button label has changed
                }
            }
        }

        public string ButtonLabel => _isHeroImage ? "Added!" : "Add as Hero Image";
        public Color ButtonBackgroundColor => _isHeroImage
            ? Color.FromArgb("#012f36")
            : Color.FromArgb("#00525e");

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
