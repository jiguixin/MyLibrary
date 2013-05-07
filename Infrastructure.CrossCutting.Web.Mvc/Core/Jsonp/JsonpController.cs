using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Infrastructure.CrossCutting.Web.Mvc.Core.Jsonp
{

    /// <summary>
    /// 默认约定 Action名为 Callback名
    /// </summary>
    public class JsonpController : Controller
    {

        protected internal virtual ActionResult Jsonp(string json)
        {
            return new JsonpResult { Json = json };
        }

        protected internal virtual ActionResult Jsonp(string json, string callback)
        {
            return new JsonpResult { Json = json, Callback = callback };
        }

        protected internal virtual ActionResult Jsonp(object data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return new JsonpResult { Json = serializer.Serialize(data) };
        }

        protected internal virtual ActionResult Jsonp(object data, string callback)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return new JsonpResult { Json = serializer.Serialize(data), Callback = callback };
        }

        protected internal virtual ActionResult Jsonp(object data, string callback, JavaScriptTypeResolver resolver)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer(resolver);
            return new JsonpResult { Json = serializer.Serialize(data), Callback = callback };
        }

        protected internal virtual ActionResult Jsonp(object data, JavaScriptTypeResolver resolver)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer(resolver);
            return new JsonpResult { Json = serializer.Serialize(data) };
        }

        protected internal ViewResult JsonpView(object model)
        {
            return JsonpView(null /* viewName */, model);
        }

        protected internal ViewResult JsonpView(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new JsonpViewResult
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }



    }
}
