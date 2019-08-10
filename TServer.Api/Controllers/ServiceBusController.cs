using System;
using System.Linq;
using System.Reflection.Emit;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TServer.Api.Helper;
using TServer.Api.Model;

namespace TServer.Api.Controllers
{
    public class ServiceBusController : ApiController
    {
        public MethodOutput Post([FromBody]MethodInput value)
        {
             return CallMethodInternal(value);
        }

        private static MethodOutput CallMethodInternal(MethodInput inputs)
        {
            //TODO: LOG

            var output = new MethodOutput();
            if (inputs == null)
            {
                output.Error = "Inputs not found.";
                return output;
            }

            if(ServiceConfigs.TokenIgnoreList.Contains($"{inputs.ServiceName}.{inputs.MethodName}") == false &&
               ApiHelper.ValidateToken() == false)
            {
                output.Error = "Token is not valid.";
                return output;
            }

            var service = Type.GetType($"{ServiceConfigs.Namespace}.{inputs.ServiceName}, {ServiceConfigs.AssemblyName}");
            if (service == null)
            {
                output.Error = "Service not found.";
                return output;
            }

            var method = service.GetMethod(inputs.MethodName);
            if (method == null)
            {
                output.Error = "Method not found.";
                return output;
            }

            object obj;
            try
            {
                var ctor = service.GetConstructor(Type.EmptyTypes);
                var methodName = service.Name + "Ctor";
                var dm = new DynamicMethod(methodName, service, Type.EmptyTypes, typeof(Activator), true);
                var lgen = dm.GetILGenerator();
                lgen.Emit(OpCodes.Newobj, ctor ?? throw new InvalidOperationException());
                lgen.Emit(OpCodes.Ret);
                obj = ((Func<object>) dm.CreateDelegate(typeof(Func<object>)))();
            }
            catch (Exception e)
            {
                output.Error = $"Ctor not found. Error: {e.Message}";
                return output;
            }

            try
            {
                var parameters = method.GetParameters();
                if (parameters.Length != inputs.Parameters.Length)
                {
                    output.Error = "Parameters not matched";
                    return output;
                }

                for (var i = 0; i < parameters.Length; i++)
                {
                    var methodParamType = parameters[i].ParameterType;

                    if (inputs.Parameters[i] == null)
                        continue;

                    var inputParamType = inputs.Parameters[i].GetType();

                    if (inputParamType == typeof(JObject))
                    {
                        inputs.Parameters[i] = ((JObject)inputs.Parameters[i]).ToObject(methodParamType);
                    }
                    else if (inputParamType == typeof(JArray))
                    {
                        inputs.Parameters[i] = ((JArray)inputs.Parameters[i]).ToObject(methodParamType);
                    }
                    else if (inputParamType != methodParamType)
                    {
                        var paramType = Nullable.GetUnderlyingType(methodParamType) ?? methodParamType;
                        inputs.Parameters[i] = Convert.ChangeType(inputs.Parameters[i], paramType);
                    }
                }
            }
            catch (Exception e)
            {
                output.Error = $"Parameters cast Failed. Error: {e.Message}";
                return output;
            }

            try
            {
                output.Result = method.Invoke(obj, inputs.Parameters);
            }
            catch (Exception e)
            {
                output.Error = e.InnerException == null ? $"Method invoke failed. Error: {e.Message}" : e.InnerException.Message;
            }

            return output;
        }
    }
}