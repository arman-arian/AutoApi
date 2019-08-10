using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TServer.Api
{
    public static class ServiceConfigs
    {
        public const string Namespace = "TApplication";

        public const string AssemblyName = "TApplication";

        public static string[] TokenIgnoreList = {"LoginService.Login"};
    }
}