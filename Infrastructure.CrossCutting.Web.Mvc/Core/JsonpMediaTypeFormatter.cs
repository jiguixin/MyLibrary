
namespace Infrastructure.CrossCutting.Web.Mvc.Core
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Web;

    /*  示例
     * 页面
     * $(function ()
           {
               $.ajax({
                   Type       : "GET",
                   url: "http://localhost:4252/api/contacts", //WEB API的网站
                   dataType   : "jsonp",
                   success    : listContacts
               });
           }); 
           function listContacts(contacts) {
               $.each(contacts, function (index, contact) { 
                   $("#contacts").append($(html));
               });
           }
     * 
     * Application_Start
     *  GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());
     */
    /// <summary>
    /// 客户端用JSONP的方法进行调用。 
    /// </summary>
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public string Callback { get; private set; }

        public JsonpMediaTypeFormatter(string callback = null)
        {
            this.Callback = callback;
        }

        public override Task WriteToStreamAsync(
            Type type,
            object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(this.Callback))
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
            try
            {
                this.WriteToStream(type, value, writeStream, content);
                return Task.FromResult<AsyncVoid>(new AsyncVoid());
            }
            catch (Exception exception)
            {
                TaskCompletionSource<AsyncVoid> source = new TaskCompletionSource<AsyncVoid>();
                source.SetException(exception);
                return source.Task;
            }
        }

        private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            JsonSerializer serializer = JsonSerializer.Create(this.SerializerSettings);
            using (StreamWriter streamWriter = new StreamWriter(writeStream, this.SupportedEncodings.First()))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter) { CloseOutput = false })
            {
                jsonTextWriter.WriteRaw(this.Callback + "(");
                serializer.Serialize(jsonTextWriter, value);
                jsonTextWriter.WriteRaw(")");
            }
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(
            Type type,
            HttpRequestMessage request,
            MediaTypeHeaderValue mediaType)
        {
            if (request.Method != HttpMethod.Get)
            {
                return this;
            }
            string callback;
            if (
                request.GetQueryNameValuePairs()
                    .ToDictionary(pair => pair.Key, pair => pair.Value)
                    .TryGetValue("callback", out callback))
            {
                return new JsonpMediaTypeFormatter(callback);
            }
            return this;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct AsyncVoid
        {
        }
    }
}
