using System;

namespace Napi.Example.Models.Venues
{
    public class Event : IModel
    {
        public long VenueID { get; set; }
        public DateTime DateTime { get; set; }
    }
}