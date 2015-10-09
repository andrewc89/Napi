using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json;

namespace Napi.Extensions
{
    public static class ExpandoObjectExtensions
    {
        public static string Flatten (this ExpandoObject Expando)
        {
            var Builder = new StringBuilder();
            var Contents = new List<string>();
            var Dict = Expando as IDictionary<string, object>;
            Builder.Append("{");
            foreach (KeyValuePair<string, object> Pair in Dict)
            {
                var ValueType = Pair.Value.GetType();
                string Key = Pair.Key.TrimEnd('_');
                string Content;
                if (ValueType.IsGenericType)
                {
                    Content = string.Format("\"{0}\":{1}", Key, FlattenList((IEnumerable<ExpandoObject>)Pair.Value));
                }
                else if (ValueType == typeof(ExpandoObject))
                {
                    Content = string.Format("\"{0}\":{1}", Key, Flatten((ExpandoObject)Pair.Value));
                }
                else
                {
                    Content = string.Format("\"{0}\":\"{1}\"", Key, JsonConvert.SerializeObject(Pair.Value).Replace("\"", "").Replace("\\", "\'"));
                }
                Contents.Add(Content);
            }
            Builder.Append(string.Join(",", Contents.ToArray()));
            Builder.Append("}");
            return Builder.ToString();
        }

        public static string FlattenList (this IEnumerable<ExpandoObject> List)
        {
            var Builder = new StringBuilder();
            var Items = new List<string>();
            Builder.Append("[ ");
            foreach (var Item in List)
            {
                Items.Add(Item.Flatten());
            }
            Builder.Append(string.Join(", ", Items));
            Builder.Append(" ]");
            return Builder.ToString();
        }
    }
}