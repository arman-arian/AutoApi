using System;
using System.Net;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using TServer.Api.Model;

namespace TServer.ExternalApi.Controllers
{
    public class ServiceBusController : ApiController
    {
        public MethodOutput Post([FromBody]MethodInput value)
        {
            try
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(ServiceConfigs.Username + ":" + ServiceConfigs.Password));
                var parameters = JsonConvert.SerializeObject(value);

                using (var wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers["ApiVersion"] = "1.0";
                    wc.Headers[HttpRequestHeader.ContentType] = "application/JSON";
                    wc.Headers[HttpRequestHeader.CacheControl] = "no-cache";
                    wc.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
                    var jsonResult = wc.UploadString(ServiceConfigs.ServiceUrl, "POST", parameters);
                    return JsonConvert.DeserializeObject<MethodOutput>(jsonResult);
                }
            }
            catch (Exception e)
            {
                return new MethodOutput
                {
                    Error = $"External service error: {e.Message}"
                };
            }
        }
    }
}