using System;

namespace Napi.Example.Models
{
    public class Event : IModel<Event>
    {
        public DateTime DateTime { get; set; }
    }
}