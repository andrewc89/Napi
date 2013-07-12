using System;

namespace Napi.Models.Interfaces
{
    public interface INapiModel<IDType>
        where IDType : IComparable
    {
        IDType ID { get; }
    }
}