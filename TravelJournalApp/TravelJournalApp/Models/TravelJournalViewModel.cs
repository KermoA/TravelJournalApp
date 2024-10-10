using System.ComponentModel;
using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace ViewModel
{
    public class TravelJournalViewModel 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public List<TravelJournalImage> Images { get; set; } = new List<TravelJournalImage>();

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}  : INotifyPropertyChanged
    }
}