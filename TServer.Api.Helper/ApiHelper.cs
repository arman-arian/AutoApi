using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TServer.Api.Helper
{
    public static class ApiHelper
    {
        public static bool ValidateToken()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return false;

            if (TokenHelper.Decode(token) == null)
                return false;

            return true;
        }

        public static IDictionary<string, object> GetTokenPayload()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenObj = TokenHelper.Decode(token);
            return tokenObj;
        }

        private static string GetToken()
        {
            var stream = HttpContext.Current?.Request.InputStream;
            if (stream == null)
                return string.Empty;

            HttpContext.Current.Request.InputStream.Seek(0, SeekOrigin.Begin);

            var messageProperties = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            if(string.IsNullOrEmpty(messageProperties))
                return string.Empty;

            return JObject.Parse(messageProperties)["Token"].Value<string>();
        }

        public static string GetClientIp()
        {
            var request = HttpContext.Current?.Request;
            if (request == null)
                return string.Empty;

            return request.UserHostAddress;
        }
    }
}
