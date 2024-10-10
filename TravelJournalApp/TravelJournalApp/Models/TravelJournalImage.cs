using SQLite;
using Data;
using Model;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace ModelImage
{
    [Table("TravelJournalImage")]
    public class TravelJournalImage
    {
        public Guid Id { get; set; }
        public Guid TravelJournalId { get; set; } // Foreign key
        public string ImagePath { get; set; }
    }
}
