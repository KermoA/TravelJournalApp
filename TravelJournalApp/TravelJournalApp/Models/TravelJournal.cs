using System;
using System.Collections.Generic;
using System.Text;

namespace TravelJournalApp.Models
{
    public class TravelJournal
    {
        public Guid? Id { get; set; }
        public string TravelName { get; set; }
        public string Description { get; set; }
    }
}
