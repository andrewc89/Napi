using Napi.Models.Interfaces;
using Napi.Modules;

namespace Napi.Example.Modules
{
    public abstract class BaseModule<ModelType> : BaseModule<ModelType, long>
        where ModelType : INapiModel<long>
    {
        public BaseModule(string ModelName)
            : base("/API", ModelName)
        {
        }
    }
}