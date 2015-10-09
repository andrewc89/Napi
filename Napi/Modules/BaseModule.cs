using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using Napi.Extensions;
using Napi.Models;
using Napi.Models.Interfaces;
using Napi.Repository;

namespace Napi.Modules
{
    public abstract class BaseModule<ModelType, IDType> : NancyModule
        where ModelType : INapiModel<IDType>
        where IDType : IComparable
    {
        #region Properties

        /// <summary>
        /// blacklisted properties that should not be updated/set via the API
        /// </summary>
        protected static string[] BlacklistedProperties;

        /// <summary>
        /// model name used for url
        /// </summary>
        protected string ModelName;

        protected IRepository<ModelType, IDType> Repository;

        #endregion

        #region Constructor

        /// <summary>
        /// setup Repository, ModelName, routes
        /// </summary>
        /// <param name="Route">base nancy route</param>
        /// <param name="ModelName">model name used for url</param>
        public BaseModule (string Route, string ModelName)
            : base(Route)
        {
            this.ModelName = ModelName;
            SetupRoutes();
        }

        #endregion

        #region SetupRoutes

        /// <summary>
        /// sets up default routes for model
        /// </summary>
        private void SetupRoutes ()
        {
            _GetAll();
            _GetSingle();
            _Post();
            _Put();
            _Patch();
            _Delete();
        }

        #region Get

        public virtual void _GetAll ()
        {
            Get["/" + ModelName] = Params =>
            {
                return Json<ModelType>(Repository.All().Cast<INapiModel<IDType>>());
            };
        }

        public virtual void _GetSingle ()
        {
            Get["/" + ModelName + "/{ModelID}"] = Params =>
            {
                return Json<ModelType>(Repository.Get((IDType)Params.ModelID));
            };
        }

        #endregion

        #region Post

        public virtual void _Post ()
        {
            Post["/" + ModelName] = Params =>
            {
                return Try(() =>
                {
                    List<ModelType> Models = this.Bind<List<ModelType>>(BlacklistedProperties);
                    if (Models.Count == 0)
                    {
                        Models.Add(this.Bind<ModelType>(BlacklistedProperties));
                    }
                    Models.ForEach(x => Repository.Create(x));
                    return Json<ModelType>(Models.Cast<INapiModel<IDType>>());
                });
            };
        }

        #endregion

        #region Put

        public virtual void _Put ()
        {
            Put["/" + ModelName + "/{ModelID}"] = Params =>
            {
                return Try(() =>
                {
                    var Model = Repository.Get((IDType)Params.ModelID);
                    this.BindTo(Model, BlacklistedProperties);
                    Repository.Create(Model);
                    return Json<ModelType>(Model);
                });
            };
        }

        #endregion

        #region Patch

        public virtual void _Patch ()
        {
            Patch["/" + ModelName + "/{ModelID}"] = Params =>
            {
                return Error("Not yet implemented");
            };
        }

        #endregion

        #region Delete

        public virtual void _Delete ()
        {
            Delete["/" + ModelName + "/{ModelID}"] = Params =>
            {
                return Try(() =>
                {
                    Repository.Delete((IDType)Params.ModelID);
                    return "true";
                });
            };
        }

        #endregion

        #endregion

        #region IEnumerableProperty

        /// <summary>
        /// sets up routes for an IEnumerable property
        /// ex: /Venue/{ID}/Events
        /// </summary>
        /// <typeparam name="PropertyType">Type of IEnumerable<></typeparam>
        /// <param name="PropertyName">property name to be used in URL</param>
        /// <param name="GetProperty">Func to select model property</param>
        public void IEnumerableProperty<PropertyType> (string PropertyName, IRepository<PropertyType, IDType> PropertyRepository, Func<ModelType, IEnumerable<PropertyType>> GetProperty)
            where PropertyType : INapiModel<IDType>, new()
        {
            Get["/" + ModelName + "/{ModelID}/" + PropertyName] = Params =>
            {
                var Model = Repository.Get((IDType)Params.ModelID);
                return Json<PropertyType>(GetProperty(Model).Cast<INapiModel<IDType>>());
            };

            Get["/" + ModelName + "/{ModelID}/" + PropertyName + "/{PropertyID}"] = Params =>
            {
                var Model = Repository.Get((IDType)Params.ModelID);
                return Json<PropertyType>(GetProperty(Model).First(x => x.ID.Equals((IDType)Params.PropertyID)));
            };

            Post["/" + ModelName + "/{ModelID}/" + PropertyName] = Params =>
            {
                var Model = Repository.Get((IDType)Params.ModelID);
                return AddToIEnumerable(Model, PropertyRepository, GetProperty(Model));
            };

            Put["/" + ModelName + "/{ModelID}/" + PropertyName + "/{PropertyID}"] = Params =>
            {
                return Try(() =>
                {
                    var Model = PropertyRepository.Get((IDType)Params.PropertyID);
                    this.BindTo(Model, BlacklistedProperties);
                    PropertyRepository.Update(Model);
                    return Json<PropertyType>(Model);
                });
            };

            Patch["/" + ModelName + "/{ModelID}/" + PropertyName + "/{PropertyID}"] = Params =>
            {
                return Error("Not yet implemented");
            };

            Delete["/" + ModelName + "/{ModelID}/" + PropertyName + "/{PropertyID}"] = Params =>
            {
                return Try(() =>
                {
                    PropertyRepository.Delete((IDType)Params.PropertyID);
                    return "true";
                });
            };
        }

        #endregion

        #region Property

        /// <summary>
        /// sets up routes for a property
        /// ex: /Events/{ID}/Venue
        /// </summary>
        /// <typeparam name="PropertyType">property Type</typeparam>
        /// <param name="PropertyName">property name to be used in URL</param>
        /// <param name="GetProperty">Func to select model property</param>
        public void Property<PropertyType> (string PropertyName, IRepository<PropertyType, IDType> PropertyRepository, Func<ModelType, PropertyType> GetProperty)
            where PropertyType : INapiModel<IDType>, new()
        {
            Get["/" + ModelName + "/{ModelID}/" + PropertyName] = Params =>
            {
                return Json<PropertyType>(GetProperty(Repository.Get((IDType)Params.ModelID)));
            };

            Put["/" + ModelName + "/{ModelID}/" + PropertyName] = Params =>
            {
                return Try(() =>
                {
                    var Model = this.Bind<PropertyType>(BlacklistedProperties);
                    PropertyRepository.Update(Model);
                    return Json<PropertyType>(Model);
                });
            };

            Patch["/" + ModelName + "/{ModelID}/" + PropertyName] = Params =>
            {
                return Error("Not yet implemented");
            };
        }

        #endregion

        #region Json

        /// <summary>
        /// converts JSON string to JSON Response
        /// </summary>
        /// <param name="JsonString">JSON string</param>
        /// <returns>Nancy.Response</returns>
        protected Response Json (string JsonString)
        {
            var JsonBytes = Encoding.UTF8.GetBytes(JsonString);
            return new Response
            {
                ContentType = "application/json",
                Contents = s => s.Write(JsonBytes, 0, JsonBytes.Length)
            };
        }

        /// <summary>
        /// converts model to JSON Response
        /// </summary>
        /// <param name="Model">model to convert</param>
        /// <returns>Nancy.Response</returns>
        protected Response Json<JsonModelType> (INapiModel<IDType> Model)
            where JsonModelType : INapiModel<IDType>
        {
            return Json(Model.ToJson<ModelType, IDType>(ParseFields(), ParseEmbed()));
        }

        /// <summary>
        /// converts list of models to JSON Response
        /// </summary>
        /// <param name="Models">IEnumerable of models</param>
        /// <returns>Nancy.Response</returns>
        protected Response Json<JsonModelType> (IEnumerable<INapiModel<IDType>> Models)
            where JsonModelType : INapiModel<IDType>
        {
            return Json(Models.ToJson<ModelType, IDType>(ParseFields(), ParseEmbed()));
        }

        #endregion

        #region Error

        /// <summary>
        /// return error, trips JS AJAX error callback
        /// </summary>
        /// <param name="Message">error message to return</param>
        /// <returns>Nancy.Response to trip JS AJAX error callback</returns>
        protected Response Error (string Message)
        {
            var Bytes = Encoding.UTF8.GetBytes(Message);
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ContentType = "application/json",
                Contents = s => s.Write(Bytes, 0, Bytes.Length)
            };
        }

        #endregion

        #region Try

        /// <summary>
        /// wrap a the execution of a Func in a try/catch
        /// </summary>
        /// <param name="Do">Func to wrap in try/catch</param>
        /// <returns>returns return from Func or error message if error occurs</returns>
        protected Response Try (Func<Response> Do)
        {
            try
            {
                return Do();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }

        #endregion

        #region ParseFields

        /// <summary>
        /// parse a generic url parameter, convert to string[]
        /// </summary>
        /// <param name="Arg">generic url parameter key</param>
        /// <returns>string[] of url parameter value</returns>
        private string[] ParseArg (string Arg)
        {
            return string.IsNullOrEmpty(Arg) ? new string[] { } : Arg.Split(',').Select(x => x.ToLower()).ToArray();
        }

        /// <summary>
        /// check if any Fields specified, convert to string[]
        /// </summary>
        /// <returns>string[] of whitelisted Fields</returns>
        internal string[] ParseFields ()
        {
            return ParseArg(Context.Request.Query.Fields);
        }

        /// <summary>
        /// check if any property models need to be embedded, convert list to string[]
        /// </summary>
        /// <returns></returns>
        internal string[] ParseEmbed ()
        {
            return ParseArg(Context.Request.Query.Embed);
        }

        #endregion

        #region AddToIEnumerable

        /// <summary>
        /// adds a model of PropertyType to an IEnumerable property of ModelType
        /// used with POST to /Venue/{ID}/Events
        /// </summary>
        /// <typeparam name="PropertyType">property Type</typeparam>
        /// <param name="Model">model containing the IEnumerable proeprty to which to add the item of type PropertyType</param>
        /// <param name="List">IEnumerable of PropertyType to which to add new model of type PropertyType</param>
        /// <returns>if successful: model added, else: error message</returns>
        protected Response AddToIEnumerable<PropertyType> (ModelType Model, IRepository<PropertyType, IDType> PropertyRepository, IEnumerable<PropertyType> List)
            where PropertyType : INapiModel<IDType>, new()
        {
            return Try(() =>
            {
                List<PropertyType> SubModels = this.Bind<List<PropertyType>>(BlacklistedProperties);
                if (SubModels.Count == 0)
                {
                    SubModels.Add(this.Bind<PropertyType>(BlacklistedProperties));
                }
                SubModels.ForEach(x => PropertyRepository.Create(x));
                ((List<PropertyType>)List).AddRange(SubModels);
                Repository.Update(Model);
                return Json<PropertyType>(SubModels.Cast<INapiModel<IDType>>());
            });
        }

        #endregion
    }
}