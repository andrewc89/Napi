using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Napi.Example.Models;
using Napi.Example.Models.Venues;
using Napi.Repository;

namespace Napi.Example.Repository
{
    public class ModelRepository : IRepository<Venue, long>
    {
        public IEnumerable<Venue> All()
        {
            return MockDb._venues;
        }

        public Venue Get(long ID)
        {
            return MockDb._venues.First(x => x.ID == ID);
        }

        public Venue Create(Venue Model)
        {
            Model.ID = MockDb._venues.Select(x => x.ID).Max() + 1;
            Model.Active = true;
            Model.Events = new List<Event>();
            MockDb._venues.Add(Model);
            return Model;
        }

        public Venue Update(Venue Model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long ID)
        {
            throw new NotImplementedException();
        }
    }
}