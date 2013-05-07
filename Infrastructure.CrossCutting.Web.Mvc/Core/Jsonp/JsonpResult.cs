using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure.CrossCutting.Web.Mvc.Core.Jsonp
{
    public class JsonpResult : ActionResult
    {


        public string Json
        {
            get;
            set;
        }

        public string Callback
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/x-javascript";

            if (Callback == null)
            {
                Callback = context.RouteData.Values["action"].ToString();
            }

            response.Write(string.Format("{0}({1})", Callback, Json));

        }
    }
}
