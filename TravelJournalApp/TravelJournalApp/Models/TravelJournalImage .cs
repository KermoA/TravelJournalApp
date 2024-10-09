using System;
using System.Collections.Generic;
using System.Text;

namespace TravelJournalApp.Models
{
    public class TravelJournalImage
    {
        public Guid Id { get; set; }
        public Guid TravelJournalId { get; set; } // Foreign key
        public string ImagePath { get; set; }
    }
}


