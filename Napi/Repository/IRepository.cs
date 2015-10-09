using System;
using System.Collections.Generic;
using Napi.Models.Interfaces;

namespace Napi.Repository
{
    public interface IRepository<ModelType, IDType>
        where ModelType : INapiModel<IDType>
        where IDType : IComparable
    {
        IEnumerable<ModelType> All ();

        ModelType Get (IDType ID);

        ModelType Create (ModelType Model);

        ModelType Update (ModelType Model);

        bool Delete (IDType ID);
    }
}