using System.Collections.Generic;
using TModel;
using TServer.Api.Helper;

namespace TApplication
{
    public class LoginService
    {
        public LoginOutputs Login(LoginInputs inputs)
        {
            if (inputs.Username != "Arman" || inputs.Password != "332332")
            {
                return new LoginOutputs {ErrorMessage = "username or password is not valid."};
            }

            var payload = new Dictionary<string, object>
            {
                { "UserId", inputs.Username },
                { "OrganId", 1 }
            };

            return new LoginOutputs {Token = TokenHelper.Encode(payload)};
        }
    }
}
