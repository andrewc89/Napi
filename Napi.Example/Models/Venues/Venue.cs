using System.Collections.Generic;

namespace Napi.Example.Models.Venues
{
    public class Venue : IModel
    {
        public string DisplayName { get; set; }

        public List<Event> Events { get; set; }

        public override string ToString ()
        {
            return DisplayName;
        }
    }
}