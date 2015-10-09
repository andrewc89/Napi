using System;

namespace Napi.Example.Models
{
    public class Event : BaseModel<Event>
    {
        public DateTime DateTime { get; set; }
    }
}