using Newtonsoft.Json;

namespace GitHubRepoTrackerFE.Models
{
    public class AccessToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime ExpirationTime { get; set; }
    }
}
