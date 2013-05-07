using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure.CrossCutting.Web.Mvc.Core.Jsonp
{
    public class JsonpViewResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            //if
            //context.HttpContext.Request.UrlReferrer.Host=="YourDomain"
            //return ContentType="text/Html"
            //else:
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/x-javascript";
        }
    }
}
