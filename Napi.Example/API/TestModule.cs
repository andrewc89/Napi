using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Napi.Example.API
{
    public class TestModule : NancyModule
    {
        public TestModule()
        {
            Get["/API/test"] = Params =>
            {
                return "Good job!";
            };
        }
    }
}