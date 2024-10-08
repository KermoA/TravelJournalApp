using System;
using System.Collections.Generic;
using System.Text;

namespace TravelJournalApp.Models
{
    public class TravelJournal
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string ImageFileId {  get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

    }
}
