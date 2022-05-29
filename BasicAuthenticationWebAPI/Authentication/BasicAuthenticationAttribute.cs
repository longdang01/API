using BasicAuthenticationWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthenticationWebAPI.Authentication
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private const string SCHEME = "BASIC";
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);

                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.WwwAuthenticate
                        .Add(new AuthenticationHeaderValue(SCHEME));
                }
            } else
            {
                string authenticationToken = actionContext.Request.Headers
                                            .Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if (UsersSecurity.Login(username, password))
                {
                    var principal = new GenericPrincipal(
                        new GenericIdentity(username), null);
                    
                    Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                        HttpContext.Current.User = principal;
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}