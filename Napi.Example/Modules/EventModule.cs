using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Napi.Example.Models;

namespace Napi.Example.Modules
{
    public class EventModule : BaseModule<Event>
    {
        public EventModule()
            : base("Event")
        {
        }
    }
}