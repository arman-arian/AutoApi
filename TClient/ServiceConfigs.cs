namespace TClient
{
    public static class ServiceConfigs
    {
        public static string ServiceUri = "https://localhost:44396/Api/ServiceBus/";

        public static string Username = "Test";

        public static string Password = "1";

        public static string Token = "";

        public static void SetParams(string serviceUri, string username, string password, string token)
        {
            ServiceUri = serviceUri;
            Username = username;
            Password = password;
            Token = token;
        }

        public static void SetClientContext(int organCode, int perCode)
        {
            Token = $"{organCode},{perCode}";
        }
    }
}
