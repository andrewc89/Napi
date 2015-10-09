using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Napi.Example.Models;
using Napi.Repository;

namespace Napi.Example.Repository
{
    public class EventRepository : IRepository<Event, long>
    {
        private static List<Event> _events = new List<Event>
        {
            new Event
            {
                ID = 1,
                Active = true,
                DateTime = new DateTime(2016, 2, 1),
                DisplayName = "Death Grips",
            },

            new Event
            {
                ID = 3,
                Active = true,
                DateTime = new DateTime(2016, 2, 14),
                DisplayName = "Dosh",
            },

            new Event
            {
                ID = 2,
                Active = true,
                DateTime = new DateTime(2016, 3, 2),
                DisplayName = "Deafheaven",
            },

            new Event
            {
                ID = 4,
                Active = true,
                DateTime = new DateTime(2016, 3, 21),
                DisplayName = "Don Caballero",
            }
        };

        private static long GetID()
        {
            return _events.Select(x => x.ID).Max() + 1;
        }

        public IEnumerable<Event> All()
        {
            return _events;
        }

        public Event Create(Event Model)
        {
            Model.ID = GetID();
            _events.Add(Model);
            return Model;
        }

        public bool Delete(long ID)
        {
            _events.RemoveAll(x => x.ID == ID);
            return true;
        }

        public Event Get(long ID)
        {
            return _events.First(x => x.ID == ID);
        }

        public Event Update(Event Model)
        {
            _events.RemoveAll(x => x.ID == Model.ID);
            Model.ID = GetID();
            _events.Add(Model);
            return Model;
        }
    }
}