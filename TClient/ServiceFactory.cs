using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TServer.Api.Model;

namespace TClient
{
    public static class ServiceFactory
    {
        public static T CallMethod<T>(string service, string method, params object[] parameters)
        {
            var methodInputs = new MethodInput
            {
                MethodName = method,
                ServiceName = service,
                Parameters = parameters,
                Token = ServiceConfigs.Token
            };

            var result = CallService(methodInputs);
            var resultModel = JsonConvert.DeserializeObject<MethodOutput>(result);

            if (string.IsNullOrEmpty(resultModel.Error))
            {
                if (resultModel.Result == null)
                {
                    return default(T);
                }
                if (resultModel.Result.GetType() == typeof(JObject))
                {
                    return (T)((JObject)resultModel.Result).ToObject(typeof(T));
                }
                if (resultModel.Result.GetType() == typeof(JArray))
                {
                    return (T)((JArray)resultModel.Result).ToObject(typeof(T));
                }
                 
                return (T)Convert.ChangeType(resultModel.Result, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
            }
            throw new Exception(resultModel.Error);
        }

        public static void CallMethod(string service, string method, params object[] parameters)
        {
            var methodInputs = new MethodInput
            {
                MethodName = method,
                ServiceName = service,
                Parameters = parameters,
                Token = ServiceConfigs.Token
            };

            var result = CallService(methodInputs);
            var resultModel = JsonConvert.DeserializeObject<MethodOutput>(result);

            if (!string.IsNullOrEmpty(resultModel.Error))
            {
                throw new Exception(resultModel.Error);
            }
        }

        private static string CallService(MethodInput inputs)
        {
            var myParameters = JsonConvert.SerializeObject(inputs);
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(ServiceConfigs.Username + ":" + ServiceConfigs.Password));

            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                wc.Headers["ApiVersion"] = "1.0";
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.CacheControl] = "no-cache";
                wc.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
                return wc.UploadString(ServiceConfigs.ServiceUri, myParameters);
            }
        }
    }
}