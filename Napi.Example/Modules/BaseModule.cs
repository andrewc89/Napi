using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Napi.Models.Interfaces;

namespace Napi.Example.Modules
{
    public class BaseModule<ModelType> : Napi.Modules.BaseModule<ModelType, long>
        where ModelType : INapiModel<long>
    {
        public BaseModule(string ModelName)
            : base("/API", ModelName)
        {
        }
            
    }
}