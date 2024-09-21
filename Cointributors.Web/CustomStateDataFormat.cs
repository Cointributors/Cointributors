using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace Cointributors.Web
{
    public class CustomStateDataFormat : ISecureDataFormat<AuthenticationProperties>
    {
        private readonly IMemoryCache _cache;

        public CustomStateDataFormat(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string Protect(AuthenticationProperties data)
        {
            var guid = Guid.NewGuid().ToString();
            _cache.Set(guid, data, TimeSpan.FromMinutes(5)); // Adjust the expiration as needed
            return guid;
        }

        public string Protect(AuthenticationProperties data, string? purpose)
        {
            var guid = Guid.NewGuid().ToString();
            _cache.Set(guid, data, TimeSpan.FromMinutes(5)); // Adjust the expiration as needed
            return guid;
        }

        public AuthenticationProperties Unprotect(string protectedText)
        {
            _cache.TryGetValue(protectedText, out AuthenticationProperties properties);
            return properties;
        }

        public AuthenticationProperties? Unprotect(string? protectedText, string? purpose)
        {
            _cache.TryGetValue(protectedText, out AuthenticationProperties properties);
            return properties;
        }
    }
}
