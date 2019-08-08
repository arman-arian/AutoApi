using System.IO;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using TServer.Api.Model;

namespace TServer.Api.Helper
{
    public static class ApiHelper
    {
        private static string GetToken()
        {
            var stream = HttpContext.Current?.Request?.InputStream;
            if (stream == null)
                return string.Empty;

            HttpContext.Current.Request.InputStream.Seek(0, SeekOrigin.Begin);

            var messageProperties = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            if(string.IsNullOrEmpty(messageProperties))
                return string.Empty;

            return JObject.Parse(messageProperties)["Token"].Value<string>();
        }

        public static int GetCurrentUserId()
        {
            return GetClientContext().PerCode;
        }

        public static int GetCurrentOrganCode()
        {
            return GetClientContext().OrganCode;
        }

        public static ClientContextDto GetClientContext()
        {
            var token = GetToken();
            if (token.Contains(",") == false)
                return new ClientContextDto(9999, 1);

            var context = token.Split(',');
            if(context.Length > 2)
                return new ClientContextDto(9999, 1);

            if(int.TryParse(context[1], out var perCode) == false)
                return new ClientContextDto(9999, 1);

            if (int.TryParse(context[0], out var organCode) == false)
                return new ClientContextDto(9999, 1);

            return new ClientContextDto(perCode, organCode);
        }

        //public static MethodTokenInfo GetMethodToken()
        //{
        //    var messageProperties = new System.IO.StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
        //    var ret = JsonConvert.DeserializeObject<MethodTokenInfo>(JObject.Parse(messageProperties)["Token"].Value<string>());
        //    return ret;
        //}

        public static string GetClientIp()
        {
            var request = HttpContext.Current?.Request;
            if (request == null)
                return string.Empty;

            return request.UserHostAddress;
        }
    }
}
