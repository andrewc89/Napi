using System.Collections.Generic;

namespace Napi.Example.Models.Venues
{
    public class Venue : IModel
    {
        public List<Event> Events { get; set; }
    }
}