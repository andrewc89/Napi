using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using Napi.Models.Interfaces;

namespace Napi.Extensions
{
    public static class IModelExtensions
    {
        public static string ToJson<ModelType, IDType> (this INapiModel<IDType> Model, string[] Fields, string[] Embed)
            where ModelType : INapiModel<IDType>
            where IDType : IComparable
        {
            return Model.ToExpando<ModelType, IDType>(Fields, Embed, true).Flatten();
        }

        public static string ToJson<ModelType, IDType> (this IEnumerable<INapiModel<IDType>> List, string[] Fields, string[] Embed)
            where ModelType : INapiModel<IDType>
            where IDType : IComparable
        {
            return List.Select(x => x.ToExpando<ModelType, IDType>(Fields, Embed)).FlattenList();
        }

        public static ExpandoObject ToExpando<ModelType, IDType> (this INapiModel<IDType> Model, string[] Fields, string[] Embed, bool Recurse = false)
            where ModelType : INapiModel<IDType>
            where IDType : IComparable
        {
            return Model.ToExpando(typeof(ModelType), Fields, Embed, Recurse);
        }

        public static ExpandoObject ToExpando<IDType> (this INapiModel<IDType> Model, Type ModelType, string[] Fields, string[] Embed, bool Recurse = false)
            where IDType : IComparable
        {
            var ReturnValue = new ExpandoObject();
            var EmptyArray = new string[] { };
            foreach (var Property in ModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string PropertyName = Property.Name.ToLower();
                if (Fields.Count() == 0 || Fields.Contains(PropertyName))
                {
                    ((ICollection<KeyValuePair<string, object>>)ReturnValue).Add(
                    (Property.CanRead
                        && (Property.PropertyType.IsPrimitive
                            || new List<Type> { typeof(string), typeof(DateTime), typeof(Guid), typeof(DateTimeOffset) }.Contains(Property.PropertyType)))
                    ? new KeyValuePair<string, object>(Property.Name, Model.GetProperty(Property))
                    : new KeyValuePair<string, object>(Property.Name, Embed.Contains(PropertyName)
                        ? (Property.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            ? ((IEnumerable<INapiModel<IDType>>)Model.GetProperty(Property)).Select(y => y.ToExpando(Property.PropertyType.GetGenericArguments()[0], EmptyArray, EmptyArray))
                            : (object)((INapiModel<IDType>)Model.GetProperty(Property)).ToExpando(Property.PropertyType, EmptyArray, EmptyArray))
                        : HttpContext.Current.AbsoluteRoot() + "API/" + ModelType.Name + "/" + Model.ID + "/" + Property.Name));
                }
            }
            return ReturnValue;
        }

        public static object GetProperty<IDType> (this INapiModel<IDType> Model, PropertyInfo Property)
            where IDType : IComparable
        {
            return Property.GetValue(Model, null);
        }

        public static object GetProperty<IDType> (this INapiModel<IDType> Model, Type ModelType, string Property)
            where IDType : IComparable
        {
            return ModelType.GetProperty(Property).GetValue(Model, null);
        }
    }
}