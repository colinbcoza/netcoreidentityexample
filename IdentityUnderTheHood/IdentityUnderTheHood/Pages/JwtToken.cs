using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Newtonsoft.Json;

namespace IdentityUnderTheHood.Pages
{
    internal class JwtToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expires_a")]
        public DateTime ExpiresAt { get; set; }
    }
}