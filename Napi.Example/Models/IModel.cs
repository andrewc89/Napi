using Napi.Models.Interfaces;

namespace Napi.Example.Models
{
    internal interface IModel : INapiModel<long>
    {
        public long ID;
        public bool Active;
    }
}