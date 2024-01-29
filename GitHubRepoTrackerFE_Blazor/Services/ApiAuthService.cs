using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Newtonsoft.Json;
using System.Text;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class ApiAuthService : IApiAuthService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public ApiAuthService(HttpClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;

        
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string token = "";

            var user = new User()
            {
                userName = _configuration.GetValue<string>("UserName"),

                password = _configuration.GetValue<string>("Password")
            };

            var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAccessToken"));
            var url = builder.ToString();

            var userJson = JsonConvert.SerializeObject(user);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");


            var res = await _client.PostAsync(url, data);

            if (res.IsSuccessStatusCode)
            {


                var result = await res.Content.ReadAsStringAsync();
                var deserializedRes = JsonConvert.DeserializeObject<AccessToken>(result);
                token = deserializedRes.Token;


            }
            return token;
        }

    }
}
