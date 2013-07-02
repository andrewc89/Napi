using System;
using System.Diagnostics.Contracts;
using System.Web;

namespace Napi.Extensions
{
    public static class HttpContextExtensions
    {
        public static Uri AbsoluteRoot (this HttpContext Context)
        {
            try
            {
                if (Context.Items["absoluteurl"] == null)
                    Context.Items["absoluteurl"] = new Uri(Context.Request.Url.GetLeftPart(UriPartial.Authority) + Context.RelativeRoot());
                return Context.Items["absoluteurl"] as Uri;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string RelativeRoot (this HttpContext Context)
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }
    }
}