using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;

namespace AspNet.Security.OAuth.WorldId
{
    public class WorldIdAuthenticationOptions : OAuthOptions
    {
        public WorldIdAuthenticationOptions()
        {
            ClaimsIssuer = WorldIdAuthenticationDefaults.Issuer;
            CallbackPath = WorldIdAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = WorldIdAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WorldIdAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WorldIdAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("openid");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        }
    }
}