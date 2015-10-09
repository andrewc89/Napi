using Napi.Example.Models;
using Napi.Example.Repository;

namespace Napi.Example.Modules
{
    public class EventModule : BaseModule<Event>
    {
        public EventModule()
            : base("Event")
        {
            Repository = new EventRepository();
        }
    }
}