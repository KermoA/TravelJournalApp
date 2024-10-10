﻿using SQLite;
using Data;
using Model ;
using ModelImage;
using ViewModel;
using ListViewModel;

namespace Model
{
    [Table("TravelJournal")]
    public class TravelJournal
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public
        DateTime TravelDate{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
