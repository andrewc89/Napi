using System;
using Napi.Repository;

namespace Napi.Models.Interfaces
{
    public interface INapiModel <IDType>
        where IDType : IComparable
    {
        IDType ID { get; }
    }
}