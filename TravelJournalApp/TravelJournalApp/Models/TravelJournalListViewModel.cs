using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace ListViewModel
{
    public class TravelJournalListViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseContext _databaseContext;
        public ObservableCollection<TravelJournalViewModel> Travels { get; set; }
        private bool isRefreshing;
        private TravelJournalViewModel travels;

        public TravelJournalListViewModel()
        {
            _databaseContext = new DatabaseContext();
            Travels = new ObservableCollection<TravelJournalViewModel>();
            PropertyChanged = new PropertyChangedEventHandler((sender, e) => { }); // Omista algväärtus
            LoadTravelEntries();
        }

        private async Task LoadTravelEntries()
        {
            try
            {
                var travels = await _databaseContext.GetAllAsync<TravelJournal>();
                 var selectedTravel = travels;

                Console.WriteLine($"travels.Count(): {travels.Count()}");
                if (travels != null)
                {
                    Console.WriteLine("Data is loaded");
                    foreach (var travel in travels)
                    {
                        var images = await _databaseContext.GetFilteredAsync<TravelJournalImage>(img => img.TravelJournalId == travel.Id);

                        var viewModel = new TravelJournalViewModel
                        {
                            Id = travel.Id,
                            Title = travel.Title,
                            Description = travel.Description,
                            Location = travel.Location,
                            TravelDate = travel.TravelDate,
                            CreatedAt = travel.CreatedAt,
                            LastUpdatedAt = travel.LastUpdatedAt
                        };

                        viewModel.Images.AddRange(images);
                        Travels.Add(viewModel);
                    }
                }
                Console.WriteLine("Data is unloaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading travel entries: {ex.Message}");
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public ICommand RefreshCommand => new Command(async () => await RefreshDataAsync());

        private async Task RefreshDataAsync()
        {
            IsRefreshing = true;
            Travels.Clear();
            await LoadTravelEntries(); // Await the loading of entries
            IsRefreshing = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
