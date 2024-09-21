using AspNet.Security.OAuth.WorldId;
using Microsoft.AspNetCore.Authentication;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class WorldIdAuthenticationExtensions
    {
        public static AuthenticationBuilder AddWorldId(this AuthenticationBuilder builder)
        {
            return builder.AddWorldId(WorldIdAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        public static AuthenticationBuilder AddWorldId(
            this AuthenticationBuilder builder,
            Action<WorldIdAuthenticationOptions> configuration)
        {
            return builder.AddWorldId(WorldIdAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        public static AuthenticationBuilder AddWorldId(
            this AuthenticationBuilder builder,
            string scheme,
            Action<WorldIdAuthenticationOptions> configuration)
        {
            return builder.AddWorldId(scheme, WorldIdAuthenticationDefaults.DisplayName, configuration);
        }

        public static AuthenticationBuilder AddWorldId(
            this AuthenticationBuilder builder,
            string scheme,
            string caption,
            Action<WorldIdAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<WorldIdAuthenticationOptions, WorldIdAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}