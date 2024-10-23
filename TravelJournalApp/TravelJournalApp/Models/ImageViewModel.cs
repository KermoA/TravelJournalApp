using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TravelJournalApp.Data;
using System.IO;
using System.Threading.Tasks;


namespace TravelJournalApp.Models
{
    public class ImageViewModel : INotifyPropertyChanged
    {

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private bool _isHeroImage;
        public bool IsHeroImage
        {
            get => _isHeroImage;
            set
            {
                if (_isHeroImage != value)
                {
                    _isHeroImage = value;
                    OnPropertyChanged(nameof(IsHeroImage));
                    OnPropertyChanged(nameof(ButtonLabel));
                }
            }
        }
        public string FilePath { get; set; } // Lisasin FilePath omaduse
        public ImageSource ImageSource { get; set; }
        //public string ImageSource { get; set; } // This property holds the image source path

        public string ButtonLabel => _isHeroImage ? "Hero Image" : "Add as Hero Image";
        public Color ButtonBackgroundColor => _isHeroImage
            ? Color.FromArgb("#012f36")
            : Color.FromArgb("#00525e");


        public ICommand DeleteImageByOneCommand { get; }

		private ObservableCollection<ImageViewModel> _imageViewModels;
		private DatabaseContext _databaseContext;

		public ImageViewModel(ObservableCollection<ImageViewModel> imageViewModels, DatabaseContext databaseContext)
		{
			_imageViewModels = imageViewModels;
			_databaseContext = databaseContext;
			DeleteImageByOneCommand = new Command(async () => await DeleteImage());
		}

		private async Task DeleteImage()
		{
			// Remove from the database
			var image = await _databaseContext.GetImageByFilePathAsync(FilePath);
			if (image != null)
			{
				await _databaseContext.DeleteImageAsync(image);
			}

			// Remove from the collection
			_imageViewModels.Remove(this);

			// Optionally, delete the image file from the file system
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
			}
		}

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}