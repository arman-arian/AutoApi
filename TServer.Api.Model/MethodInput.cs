namespace TServer.Api.Model
{
    public sealed class MethodInput
    {
        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public object[] Parameters { get; set; }

        public string Token { get; set; }
    }
}
