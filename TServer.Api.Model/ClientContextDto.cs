namespace TServer.Api.Model
{
    public sealed class ClientContextDto
    {
        public int PerCode { get; set; }

        public int OrganCode { get; set; }

        public ClientContextDto(int perCode, int organCode)
        {
            PerCode = perCode;
            OrganCode = organCode;
        }
    }
}
