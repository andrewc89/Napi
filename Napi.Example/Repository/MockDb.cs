using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Napi.Example.Models.Venues;

namespace Napi.Example.Repository
{
    public static class MockDb
    {
        public static List<Venue> _venues = new List<Venue>
        {
            new Venue
            {
                ID = 1,
                Active = true,
                DisplayName = "Concert Theater",
                Events = new List<Event>
                {
                    new Event
                    {
                        ID = 1,
                        Active = true,
                        DateTime = new DateTime(2016, 2, 1),
                        DisplayName = "Tchaikovsky's Swan Lake",
                        VenueID = 1
                    },

                    new Event
                    {
                        ID = 2,
                        Active = true,
                        DateTime = new DateTime(2016, 3, 2),
                        DisplayName = "Elgar's Cello Concerto in E minor",
                        VenueID = 1
                    }
                }
            },
            new Venue
            {
                ID = 2,
                Active = true,
                DisplayName = "The Underground",
                Events = new List<Event>
                {
                    new Event
                    {
                        ID = 3,
                        Active = true,
                        DateTime = new DateTime(2016, 2, 14),
                        DisplayName = "Lunice",
                        VenueID = 2
                    },

                    new Event
                    {
                        ID = 4,
                        Active = true,
                        DateTime = new DateTime(2016, 3, 21),
                        DisplayName = "Don Caballero",
                        VenueID = 2
                    }
                }
            }
        };

        public static List<Event> _events = _venues.Select(x => x.Events).SelectMany(x => x).ToList();
    }
}