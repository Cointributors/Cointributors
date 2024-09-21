namespace AspNet.Security.OAuth.WorldId
{
    public static class WorldIdAuthenticationDefaults
    {
        public const string AuthenticationScheme = "WorldId";

        public static readonly string DisplayName = "World ID";

        public static readonly string Issuer = "WorldId";

        public static readonly string CallbackPath = "/signin-worldid";

        public static readonly string AuthorizationEndpoint = "https://id.worldcoin.org/authorize";

        public static readonly string TokenEndpoint = "https://id.worldcoin.org/token";

        public static readonly string UserInformationEndpoint = "https://id.worldcoin.org/userinfo";
    }
}