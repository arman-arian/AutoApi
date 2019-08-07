using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TServer.Api
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            var authToken = actionContext.Request.Headers.Authorization.Parameter;

            // decoding authToken we get decode value in 'Username:Password' format  
            var decodeAuthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            // splitting decode auth Token using ':'   
            var arrUserNameAndPassword = decodeAuthToken.Split(':');

            // at 0th position of array we get username and at 1st we get password  
            if (IsAuthorizedUser(arrUserNameAndPassword[0], arrUserNameAndPassword[1]))
            {
                // setting current principle  
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(arrUserNameAndPassword[0]), null);
            }
            else
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public static bool IsAuthorizedUser(string username, string password)
        {
            // In this method we can handle our database logic here...  
            return username == "Test" && password == "1";
        }
    }
}