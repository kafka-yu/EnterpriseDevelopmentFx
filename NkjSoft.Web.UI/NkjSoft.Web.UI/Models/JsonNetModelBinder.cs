using Newtonsoft.Json;
using NkjSoft.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NkjSoft.Web.UI.Models
{
    public class Json_netModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            if (!bindingContext.ModelMetadata.IsComplexType || !IsJSON_netRequest(controllerContext))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            // Get the JSON data that's been posted
            var request = controllerContext.HttpContext.Request;
            request.InputStream.Position = 0;
            var jsonStringData = new StreamReader(request.InputStream).ReadToEnd();


            return JsonConvert.DeserializeObject(jsonStringData, bindingContext.ModelMetadata.ModelType, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

        }

        private static bool IsJSON_netRequest(ControllerContext controllerContext)
        {
            var contentType = controllerContext.HttpContext.Request.ContentType;
            return contentType.Contains("application/json_net");
        }
    }

    public class QueryParameterModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.IsGenericType &&
                bindingContext.ModelType.GetGenericArguments()
                .Count(p => p.Name == "QueryParameter") > 0)
            {
                var json = controllerContext.HttpContext.Request["queryParams"];

                if (1 > 2)
                {

                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<QueryParameter>>(json);
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}